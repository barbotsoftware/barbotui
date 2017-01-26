
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
	public class IngredientDialogFragment : DialogFragment
	{
		DetailViewModel ViewModel => App.Locator.Detail;

		public static IngredientDialogFragment NewInstance(Bundle bundle)
		{
			var fragment = new IngredientDialogFragment();
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
			builder.SetTitle(Resource.String.title_ingredient_alert);

			LayoutInflater inflater = Activity.LayoutInflater;
			builder.SetView(inflater.Inflate(Resource.Layout.IngredientDialogFragment, null));

			builder.SetPositiveButton(Resource.String.positive_ingredient_alert, (sender, e) =>
			{
				
			});

			builder.SetNegativeButton(Resource.String.negative_ingredient_alert, (sender, e) =>
			{
				Dismiss();
			});

			// Create Dialog
			var dialog = builder.Create();
			dialog.Show();

			var ingredientSpinner = dialog.FindViewById<Spinner>(Resource.Id.ingredientspinner);
			ingredientSpinner.ItemSelected += (sender, e) =>
			{
				
			};
			var adapter = new ArrayAdapter(Context, Resource.Layout.ListViewRow, ViewModel.AvailableIngredients);

			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			ingredientSpinner.Adapter = adapter;

			return dialog;
		}
	}
}
