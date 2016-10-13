using System;
using CoreGraphics;
using Foundation;
using UIKit;
using BarBot.Model;
using System.ComponentModel;

namespace BarBot.iOS.Menu
{
	public class RecipeCell : UICollectionViewCell
	{
		private Recipe recipe;
		string Title { get; set; }
		UIImageView ImageView { get; set; }

		[Export("initWithFrame:")]
		public RecipeCell(CGRect Frame) : base(Frame)
		{
		}

		public Recipe Recipe
		{
			get
			{
				return recipe;
			}

			set
			{
				recipe = value;
				OnPropertyChanged("Recipe");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
