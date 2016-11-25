using UIKit;
using BarBot.iOS.ViewModel;

namespace BarBot.iOS.Order
{
	public class RecipeDetailViewController : UIViewController
	{
		RecipeViewModel ViewModel;
		UIImageView ImageView;

		public RecipeDetailViewController(RecipeViewModel viewModel)
		{
			ViewModel = viewModel;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = ViewModel.RecipeName;
			ImageView = new UIImageView(ViewModel.DrinkImage);
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
		}
	}
}
