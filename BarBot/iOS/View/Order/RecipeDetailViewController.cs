using UIKit;
using BarBot.ViewModel;

namespace BarBot.iOS.Order
{
	public class RecipeDetailViewController : UIViewController
	{
		public RecipeViewModel ViewModel { get; private set; }

		public RecipeDetailViewController(RecipeViewModel viewModel)
		{
			ViewModel = viewModel;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = ViewModel.RecipeName;
		}
	}
}
