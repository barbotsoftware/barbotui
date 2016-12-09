using System;
using Foundation;
using UIKit;
using BarBot.Core.ViewModels;

namespace BarBot.iOS.View.Order
{
    public partial class RecipeDetailViewController : UIViewController
    {
		public RecipeViewModel ViewModel { get; set; }

		partial void OrderButton_TouchUpInside(UIButton sender)
		{
			//throw new NotImplementedException();
		}

		public RecipeDetailViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = ViewModel.RecipeName;
			//DrinkImage.Image = UIImage.LoadFromData(NSData.FromArray(ViewModel.ImgByteArray));
			IngredientTable.DataSource = new IngredientTableViewDataSource(ViewModel);
		}
    }
}