using Foundation;
using System;
using UIKit;
using BarBot.ViewModel;

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
			IngredientTable.DataSource = new IngredientTableViewDataSource(ViewModel);
		}
    }
}