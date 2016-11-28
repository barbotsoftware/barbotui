using Foundation;
using System;
using UIKit;
using BarBot.ViewModel;

namespace BarBot.iOS.Order
{
    public partial class RecipeDetailViewController : UIViewController
    {
		public RecipeViewModel ViewModel { get; set; }

		public RecipeDetailViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = ViewModel.RecipeName;
		}
    }
}