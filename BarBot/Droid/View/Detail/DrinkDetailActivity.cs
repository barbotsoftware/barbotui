
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

using GalaSoft.MvvmLight.Views;

using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Detail
{
	[Activity(Label = "DrinkDetailActivity")]
	public class DrinkDetailActivity : ActivityBase
	{
		private DetailViewModel ViewModel
		{
			get
			{
				return App.Locator.Detail;
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
		}
	}
}
