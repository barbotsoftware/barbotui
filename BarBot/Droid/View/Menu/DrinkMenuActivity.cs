using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

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

			// Activate Toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			var mTitle = (TextView)toolbar.FindViewById(Resource.Id.toolbar_title);
			mTitle.Text = ViewModel.Title;
			SetActionBar(toolbar);

			ActionBar.Title = "";
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.top_menus, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			// TODO: Implement Search
			Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
				ToastLength.Short).Show();
			return base.OnOptionsItemSelected(item);
		}
	}
}
