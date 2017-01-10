using Android.App;
using Android.OS;

using BarBot.Core.ViewModel;

using GalaSoft.MvvmLight.Views;

namespace BarBot.Droid.View.Menu
{
	[Activity(Label = "DrinkMenuActivity", MainLauncher = true)]
	public class DrinkMenuActivity : ActivityBase
	{
		private MenuViewModel ViewModel
		{
			get
			{
				return App.Locator.Menu;
			}
		}


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "DrinkMenu" layout resource
			SetContentView(Resource.Layout.DrinkMenu);
		}
	}
}
