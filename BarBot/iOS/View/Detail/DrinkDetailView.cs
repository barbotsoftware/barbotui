using System;
using System.Collections.Generic;

using Foundation;
using UIKit;
using CoreGraphics;

using GalaSoft.MvvmLight.Helpers;

using BarBot.Core;
using BarBot.Core.ViewModel;
using BarBot.iOS.Style;
using BarBot.iOS.View.Detail.IngredientTable;
using BarBot.iOS.View.Detail.IngredientTable.Picker;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailView : BaseView
	{
		// Positioning Constants
		const int SWITCH_WIDTH = 51;
		const int SWITCH_HEIGHT = 31;
		const int TEXT_SIZE = 28;
		const int OPTIONS_BOTTOM_OFFSET = 130;
		const int ICE_SWITCH_LEFT_OFFSET = 27;
		const int ICE_BUTTON_LEFT_OFFSET = ICE_SWITCH_LEFT_OFFSET + SWITCH_WIDTH + 10;
		const int GARNISH_SWITCH_LEFT_OFFSET = ICE_BUTTON_LEFT_OFFSET + TEXT_SIZE + 30;
		const int GARNISH_BUTTON_LEFT_OFFSET = GARNISH_SWITCH_LEFT_OFFSET + SWITCH_WIDTH + 10;
		const int ORDER_LEFT_OFFSET = 20;

		// ViewModel
		RecipeDetailViewModel ViewModel => Application.Locator.Detail;

		// Keep track of bindings to avoid premature garbage collection
		private readonly List<Binding> bindings = new List<Binding>();

		// UI Elements
		public UIBarButtonItem edit, done;
		public UINavigationBar NavBar;
		public UIImageView HexagonImageView;
		public UIImageView DrinkImageView;
		public UITableView IngredientTableView;
		public UISwitch IceSwitch;
		public UISwitch GarnishSwitch;
		public UIButton OrderButton;

		// Data Properties
		IngredientTableSource source;

		[Export("initWithFrame:")]
		public DrinkDetailView(CGRect Frame) : base(Frame)
		{		
			ConfigureNavBar();
			ConfigureHexagon();
			ConfigureIceSwitch();
			ConfigureIceButton();
			ConfigureGarnishSwitch();
			ConfigureGarnishButton();
			ConfigureIngredientTable();
			ConfigureOrderButton();
		}

		void ConfigureNavBar()
		{
			NavBar = new UINavigationBar();
			NavBar.Frame = new CGRect(0, 0, Bounds.Width, 64);
			SharedStyles.NavBarStyle(NavBar);

			var topItem = new UINavigationItem();
			var CloseButton = new UIBarButtonItem(UIBarButtonSystemItem.Stop);
			CloseButton.Clicked += (sender, args) =>
			{
				ViewModel.ShowDrinkMenuCommand(true);
			};
			topItem.SetLeftBarButtonItem(CloseButton, false);

			done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
			{
				IngredientTableView.SetEditing(false, true);
				topItem.RightBarButtonItem = edit;
				if (ViewModel.Recipe.Ingredients.Count > 0)
				{
					OrderButton.Enabled = true;
				}
			});

			edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) =>
			{
				if (IngredientTableView.Editing)
					IngredientTableView.SetEditing(false, true); // if we've half-swiped a row

				IngredientTableView.SetEditing(true, true);
				topItem.RightBarButtonItem = done;
				OrderButton.Enabled = false;
			});

			if (ViewModel.Recipe.RecipeId.Equals(Constants.CustomRecipeId))
			{
				topItem.RightBarButtonItem = done;
			}
			else
			{
				topItem.RightBarButtonItem = edit;
			}

			NavBar.PushNavigationItem(topItem, false);
			AddSubview(NavBar);
		}

		void ConfigureHexagon()
		{
			nfloat factor = 174.0f / 200.0f;
			var point = new CGPoint(Center.X - (Bounds.Width / 4), Frame.Y + 84);
			var imgPoint = new CGPoint(point.X, point.Y - 10);
			var size = new CGSize(Bounds.Width / 2, factor * (Bounds.Width / 2));

			HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			DrinkImageView.Frame = new CGRect(imgPoint, size);
			if (ViewModel.ImageContents != null)
			{
				DrinkImageView.Image = UIImage.LoadFromData(NSData.FromArray(ViewModel.ImageContents));
			}
			AddSubview(HexagonImageView);
			AddSubview(DrinkImageView);
		}

		void ConfigureIceSwitch()
		{
			IceSwitch = new UISwitch();
			IceSwitch.OnTintColor = Color.BarBotBlue;
			IceSwitch.Frame = new CGRect(Bounds.Left + ICE_SWITCH_LEFT_OFFSET,
										 Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
										 SWITCH_WIDTH,
										 SWITCH_HEIGHT);
			IceSwitch.On = true;
			AddSubview(IceSwitch);
		}

		void ConfigureIceButton()
		{
			var IceButton = new UIButton();

			IceButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			IceButton.TitleLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", TEXT_SIZE);

			IceButton.SetTitle("ICE", UIControlState.Normal);

			IceButton.Frame = new CGRect(Bounds.Left + ICE_BUTTON_LEFT_OFFSET,
										Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
										IceButton.IntrinsicContentSize.Width,
										TEXT_SIZE);
			IceButton.TouchUpInside += (sender, e) =>
			{
				bool IsOn = IceSwitch.On;
				IceSwitch.SetState(!IsOn, true);
			};

			AddSubview(IceButton);
		}

		void ConfigureGarnishSwitch()
		{
			GarnishSwitch = new UISwitch();
			GarnishSwitch.OnTintColor = Color.BarBotBlue;
			GarnishSwitch.Frame = new CGRect(Bounds.Left + GARNISH_SWITCH_LEFT_OFFSET,
											 Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
											 SWITCH_WIDTH,
											 SWITCH_HEIGHT);
			GarnishSwitch.On = false;
			AddSubview(GarnishSwitch);
		}

		void ConfigureGarnishButton()
		{
			var GarnishButton = new UIButton();

			GarnishButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			GarnishButton.TitleLabel.Font = UIFont.FromName("Microsoft-Yi-Baiti", TEXT_SIZE);

			GarnishButton.SetTitle("LIME WEDGE", UIControlState.Normal);

			GarnishButton.Frame = new CGRect(Bounds.Left + GARNISH_BUTTON_LEFT_OFFSET,
											Bounds.Bottom - OPTIONS_BOTTOM_OFFSET,
											GarnishButton.IntrinsicContentSize.Width,
											TEXT_SIZE);
			GarnishButton.TouchUpInside += (sender, e) =>
			{
				bool IsOn = GarnishSwitch.On;
				GarnishSwitch.SetState(!IsOn, true);
			};

			AddSubview(GarnishButton);
		}

		void ConfigureIngredientTable()
		{
			IngredientTableView = new IngredientTableView();
			IngredientTableView.Frame = new CGRect(0, Bounds.Top + 270, Frame.Width, 250);

			IngredientTableView.RegisterClassForCellReuse(typeof(IngredientTableViewCell), IngredientTableViewCell.CellID);
			IngredientTableView.RegisterClassForCellReuse(typeof(AddIngredientPickerCell), AddIngredientPickerCell.CellID);

			source = new IngredientTableSource();
			IngredientTableView.Source = source;
			IngredientTableView.Delegate = new IngredientTableViewDelegate();

			bindings.Add(
				this.SetBinding(
					() => ViewModel.Recipe.Ingredients,
					() => source.Rows));

			AddSubview(IngredientTableView);
		}

		void ConfigureOrderButton()
		{
			OrderButton = UIButton.FromType(UIButtonType.System);
			OrderButton.Frame = new CGRect(ORDER_LEFT_OFFSET,
										   Bounds.Bottom - 80,
										   Bounds.Width - (ORDER_LEFT_OFFSET * 2),
										   60);
			OrderButton.SetTitle("ORDER DRINK", UIControlState.Normal);
			SharedStyles.StyleButtonText(OrderButton, TEXT_SIZE);
			OrderButton.BackgroundColor = Color.BarBotBlue;
			OrderButton.Layer.CornerRadius = new nfloat(2.0);
			OrderButton.Layer.BorderWidth = new nfloat(0.9);
			OrderButton.Layer.BorderColor = Color.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			OrderButton.TitleEdgeInsets = insets;

			AddSubview(OrderButton);
		}
	}
}
