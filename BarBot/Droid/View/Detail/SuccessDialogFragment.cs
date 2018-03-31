
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Detail
{
	public class SuccessDialogFragment : DialogFragment
	{
		RecipeDetailViewModel ViewModel => App.Locator.Detail;

		public static SuccessDialogFragment NewInstance(Bundle bundle)
		{
			var fragment = new SuccessDialogFragment();
			fragment.Arguments = bundle;
			return fragment;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(Activity, Resource.Style.BarBotTheme_AlertDialog));
			builder.SetTitle(Resource.String.title_success_alert);
			builder.SetMessage(Resource.String.message_success_alert);
			builder.SetPositiveButton(Resource.String.positive_success_alert, (sender, e) =>
			{
				ViewModel.ShowDrinkMenuCommand(false);
				Dismiss();
			});

			return builder.Create();
		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
		}
	}
}
