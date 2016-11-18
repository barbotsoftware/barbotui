using UIKit;
using BarBot.Model;

namespace BarBot.iOS.Order
{
	public class RecipeDetailViewController : UIViewController
	{
		Recipe Recipe { get; set; }

		public RecipeDetailViewController(Recipe recipe)
		{
			Recipe = recipe;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = Recipe.Name.ToUpper();
		}
	}
}
