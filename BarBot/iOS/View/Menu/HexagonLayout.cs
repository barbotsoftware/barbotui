using System;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Foundation;

namespace BarBot.iOS.View.Menu
{
	public class HexagonLayout : UICollectionViewFlowLayout
	{
		const int SPARE_CELLS = 10;
		public nfloat _gap { get; set; }
		nint _sectionCount;
		nint _cellCount;
		nfloat _cellsPerLine;
		List<nint> _cellCountPerSection;

		public override void PrepareLayout()
		{
			base.PrepareLayout();

			ScrollDirection = UICollectionViewScrollDirection.Vertical;
			MinimumInteritemSpacing = -43.0f;
			MinimumLineSpacing = 7.0f;

			int cellWidth = 200;
			int cellHeight = 174;

			nfloat leftInset = (CollectionView.Bounds.Width - (cellWidth * 2 + MinimumInteritemSpacing)) / 2;
			nfloat rightInset = 0.0f;

			SectionInset = new UIEdgeInsets(20.0f, leftInset, 20.0f, rightInset);
			ItemSize = new CGSize(cellWidth, cellHeight);
			HeaderReferenceSize = new CGSize(0, 0);
			FooterReferenceSize = new CGSize(0, 0);
			_gap = 91.0f;

			// Section count
			_sectionCount = CollectionView.NumberOfSections();

			// Cell counts
			_cellCount = 0;
			var counts = new List<nint>();
			for (int i = 0; i < _sectionCount; i++)
			{
				nint cellCountInSection = CollectionView.NumberOfItemsInSection(i);

				// Update cell count
				_cellCount += cellCountInSection;

				// Update section cell count
				counts.Add(cellCountInSection);
			}
			_cellCountPerSection = counts;

			// Cells per line
			if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
				_cellsPerLine = NMath.Floor(((CollectionView.Bounds).Width - SectionInset.Left - SectionInset.Right) / (ItemSize.Width + MinimumInteritemSpacing));
		}

		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
		{
			return true;
		}

		public override CGSize CollectionViewContentSize
		{
			get
			{
				nfloat width = 0;
				nfloat height = 0;

				// Compute width and height according to scroll direction
				if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
				{
					// Compute width
					width += HeaderReferenceSize.Width * _sectionCount;
					width += (SectionInset.Left + SectionInset.Right) * _sectionCount;
					for (int i = 0; i < _sectionCount; i++)
					{
						width += ItemSize.Width * _cellCountPerSection[i];
						width += MinimumInteritemSpacing * (_cellCountPerSection[i] - 1);
					}
					width += FooterReferenceSize.Width * _sectionCount;

					// Height is collection view height
					height = CollectionView.Bounds.Height;
				}
				else
				{
					// Width is collection view width
					width = CollectionView.Bounds.Width;

					// Compute height
					height += HeaderReferenceSize.Height * _sectionCount;
					height += (SectionInset.Top + SectionInset.Bottom) * _sectionCount;
					for (int i = 0; i < _sectionCount; i++)
						height += CellsHeightForSection(i);
					height += FooterReferenceSize.Height * _sectionCount;
				}

				// Build content size
				var contentSize = new CGSize(width, height);

				return contentSize;
			}
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
		{
			var allAttributes = new List<UICollectionViewLayoutAttributes>();

			// Indexes
			nint start;
			nint end;

			// Compute start/end indexes according to scroll direction
			if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
			{
				start = (nint)NMath.Max((nint)rect.GetMinX() / (ItemSize.Width + MinimumInteritemSpacing) - SPARE_CELLS, 0);
				end = (nint)NMath.Min((nint)rect.GetMaxX() / (ItemSize.Width + MinimumInteritemSpacing) + SPARE_CELLS, _cellCount);
			}
			else
			{
				start = (nint)NMath.Max(NMath.Ceiling((nint)rect.GetMinY() / (ItemSize.Height + MinimumLineSpacing)) * _cellsPerLine - SPARE_CELLS, 0);
				end = (nint)NMath.Min(NMath.Ceiling((nint)rect.GetMaxY() / (ItemSize.Height + MinimumLineSpacing)) * _cellsPerLine + SPARE_CELLS, _cellCount);
			}

			// Find first index item and section
			nint item = 0;
			int section = 0;
			for (int i = 0; i < start; i++)
			{
				// Manage sections and items
				if (item == (_cellCountPerSection[section] - 1))
				{
					section++;
					item = 0;
				}
				else
				{
					item++;
				}
			}

			// Loop over attributes
			for (nint i = start; i != end; ++i)
			{
				//// Section header
				//if (item == 0)
				//{
				//	var attributes1 = LayoutAttributesForSupplementaryView(UICollectionElementKindSection.Header, NSIndexPath.FromItemSection(item, section));
				//	if (attributes1 != null)
				//		allAttributes.Add(attributes1);
				//}

				//// Footer
				//if (item == (_cellCountPerSection[section] - 1))
				//{
				//	var attributes2 = LayoutAttributesForSupplementaryView(UICollectionElementKindSection.Footer, NSIndexPath.FromItemSection(item, section));
				//	if (attributes2 != null)
				//		allAttributes.Add(attributes2);
				//}

				// Build index path
				var indexPath = NSIndexPath.FromItemSection(item, section);

				// Get attributes
				var attributes = LayoutAttributesForItem(indexPath);
				if (attributes != null)
					allAttributes.Add(attributes);

				// Manage sections and items
				if (item >= (_cellCountPerSection[section] - 1))
				{
					section++;
					item = 0;
				}
				else
				{
					item++;
				}
			}

			return allAttributes.ToArray();
		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
		{
			// Get attributes
			UICollectionViewLayoutAttributes attributes = base.LayoutAttributesForItem(indexPath);

			// Compute center
			nfloat x = CenterXForItemAtIndexPath(indexPath);
			nfloat y = CenterYForItemAtIndexPath(indexPath);

			// Update attributes
			attributes.Center = new CGPoint(x, y);

			return attributes;
		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForSupplementaryView(NSString kind, NSIndexPath indexPath)
		{
			// Prevent empty header or footer
			if ((kind.Equals(UICollectionElementKindSection.Header) && HeaderReferenceSize.Equals(new CGSize(0, 0))) ||
				(kind.Equals(UICollectionElementKindSection.Footer) && FooterReferenceSize.Equals(new CGSize(0, 0))))
				return null;

			// Get attributes
			UICollectionViewLayoutAttributes attributes = LayoutAttributesForSupplementaryView(kind, indexPath);

			// Compute center
			nfloat x = CenterXForSupplementaryViewOfKind(kind, indexPath.Section);
			nfloat y = CenterYForSupplementaryViewOfKind(kind, indexPath.Section);

			// Update attributes
			attributes.Center = new CGPoint(x, y);

			return attributes;
		}

		// Utils

		nfloat CenterXForItemAtIndexPath(NSIndexPath indexPath)
		{
			nfloat x = 0;

			// Compute y position according to scroll direction
			if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
			{
				// Section header
				x += HeaderReferenceSize.Width * (indexPath.Section + 1);

				// Section left inset
				x += SectionInset.Left * (indexPath.Section + 1);

				// Previous sections hexagons
				for (int i = 0; i < indexPath.Section; i++)
				{
					// Cells pure width
					x += ItemSize.Width * _cellCountPerSection[i];

					// Inter item spaces
					x += MinimumInteritemSpacing * (_cellCountPerSection[i] - 1);
				}

				// Current section hexagons pure width and inter item spaces
				x += ItemSize.Width * (indexPath.Item + 1) - (ItemSize.Width / 2);
				x += MinimumInteritemSpacing * indexPath.Item;

				// Section right inset
				x += SectionInset.Right * indexPath.Section;

				// Section footer
				x += FooterReferenceSize.Width * indexPath.Section;
			}
			else
			{
				nfloat indexInLine = (indexPath.Item % _cellsPerLine);

				x = ItemSize.Width * indexInLine + (ItemSize.Width / 2);
				x += MinimumInteritemSpacing * indexInLine;
				x += SectionInset.Left;
			}

			return x;
		}

		nfloat CenterYForItemAtIndexPath(NSIndexPath indexPath)
		{
			nfloat y = 0;

			// Compute y position according to scroll direction
			if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
			{
				y = ItemSize.Height / 2 + ((indexPath.Item % 2 == 0) ? 0 : _gap) + SectionInset.Top;
			}
			else
			{
				// Section header
				y += HeaderReferenceSize.Height * (indexPath.Section + 1);

				// Section top inset
				y += SectionInset.Top * (indexPath.Section + 1);

				// Previous sections hexagons
				for (int i = 0; i < indexPath.Section; i++)
					y += CellsHeightForSection(i);

				// Current line
				nfloat currentLine = NMath.Floor(indexPath.Item / (float)_cellsPerLine);

				// Current section hexagons pure height and inter line spaces
				y += ItemSize.Height * currentLine + (ItemSize.Height / 2);
				y += MinimumLineSpacing * currentLine;

				// Shift
				nfloat indexInLine = (indexPath.Item % _cellsPerLine);
				y += (indexInLine % 2 == 0) ? 0 : _gap;

				// Section bottom inset
				y += SectionInset.Bottom * indexPath.Section;

				// Section footer
				y += FooterReferenceSize.Height * indexPath.Section;
			}

			return y;
		}

		nfloat CenterXForSupplementaryViewOfKind(string kind, nint section)
		{
			nfloat x;
			if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
			{
				// Header
				if (kind.Equals(UICollectionElementKindSection.Header))
				{
					x = CenterXForItemAtIndexPath(NSIndexPath.FromItemSection(0, section));

					// Fake first cell half width
					x -= ItemSize.Width / 2;

					// Section inset
					x -= SectionInset.Left;

					// Header half
					x -= HeaderReferenceSize.Width / 2;
				}
				// Footer
				else
				{
					x = CenterXForItemAtIndexPath(NSIndexPath.FromItemSection(_cellCountPerSection[(int)section] - 1, section));

					// Fake last cell half width
					x += ItemSize.Width / 2;

					// Section inset
					x += SectionInset.Right;

					// Footer half
					x += FooterReferenceSize.Width / 2;
				}
			}
			else
			{
				// Header
				if (kind.Equals(UICollectionElementKindSection.Header))
					x = HeaderReferenceSize.Width / 2;
				else
					x = FooterReferenceSize.Width / 2;
			}
			return x;
		}

		nfloat CenterYForSupplementaryViewOfKind(string kind, nint section)
		{
			nfloat y;

			if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
			{
				// Header
				if (kind.Equals(UICollectionElementKindSection.Header))
					y = HeaderReferenceSize.Height / 2;
				else
					y = FooterReferenceSize.Height / 2;
			}
			else
			{
				// Header
				if (kind.Equals(UICollectionElementKindSection.Header))
				{
					y = CenterYForItemAtIndexPath(NSIndexPath.FromItemSection(0, section));

					// First cell half size
					y -= ItemSize.Height / 2;

					// Section inset
					y -= SectionInset.Top;

					// Header half
					y -= HeaderReferenceSize.Height / 2;
				}
				// Footer
				else
				{
					y = CenterYForItemAtIndexPath(NSIndexPath.FromItemSection(0, section));

					// First cell half size
					y -= ItemSize.Height / 2;

					// Section height
					y += CellsHeightForSection((int)section);

					// Section inset
					y += SectionInset.Bottom;

					// Header half
					y += HeaderReferenceSize.Height / 2;
				}
			}

			return y;
		}

		nfloat CellsHeightForSection(int section)
		{
			nfloat height = 0;

			// Compute height according to scroll direction
			if (ScrollDirection == UICollectionViewScrollDirection.Horizontal)
			{
				height = CollectionView.Bounds.Height;
			}
			else
			{
				nint cellsInSection = _cellCountPerSection[section];
				nfloat linesInSection = NMath.Ceiling(cellsInSection / (float)_cellsPerLine);

				if (cellsInSection == 0)
				{
					return 0;
				}
				else if (cellsInSection == 1)
				{
					return ItemSize.Height;
				}
				else if (cellsInSection % _cellsPerLine == 1)
				{
					// All except last line
					height += (linesInSection - 1) * ItemSize.Height;

					// Last line
					height += ItemSize.Height;

					// Space between lines
					height += MinimumLineSpacing * (linesInSection - 1);
				}
				else
				{
					// Item height
					height += linesInSection * ItemSize.Height;

					// Interline space
					height += (linesInSection - 1) * MinimumLineSpacing;

					// Gap for final line
					height += _gap;
				}
			}

			return height;
		}
	}
}