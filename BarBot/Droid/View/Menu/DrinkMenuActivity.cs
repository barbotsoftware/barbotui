using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using BarBot.Core.ViewModel;

using GalaSoft.MvvmLight.Helpers;
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

			Websockets.Droid.WebsocketConnection.Link();
		}
	}
}
