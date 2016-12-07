using MvvmCross.iOS.Views;
using BarBot.Core.ViewModels;

namespace BarBot.iOS.Views
{
	public partial class DrinkDetailView : MvxViewController<RecipeViewModel>
	{
		public DrinkDetailView() : base("DrinkDetailView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

