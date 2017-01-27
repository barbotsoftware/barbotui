
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

using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Detail
{
	public class IngredientDialogFragment : DialogFragment
	{
		DetailViewModel ViewModel => App.Locator.Detail;
		int index;

		public static IngredientDialogFragment NewInstance(Bundle bundle, int position)
		{
			var fragment = new IngredientDialogFragment();
			fragment.Arguments = bundle;
			fragment.index = position;
			return fragment;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			var ingredient = ViewModel.Ingredients[index];
			var quantity = ingredient.Quantity;

			AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(Activity, Resource.Style.BarBotTheme_AlertDialog));
			builder.SetTitle(Resource.String.title_ingredient_alert);

			LayoutInflater inflater = Activity.LayoutInflater;
			builder.SetView(inflater.Inflate(Resource.Layout.IngredientDialogFragment, null));

			builder.SetPositiveButton(Resource.String.positive_ingredient_alert, (sender, e) =>
			{
				ViewModel.Ingredients[index] = ingredient;
				(Activity as DrinkDetailActivity).ReloadListView();
			});

			builder.SetNegativeButton(Resource.String.negative_ingredient_alert, (sender, e) =>
			{
				Dismiss();
			});

			// Create Dialog
			var dialog = builder.Create();
			dialog.Show();

			// add current ingredient to Available Ingredients
			if (index < (ViewModel.Ingredients.Count - 1))
			{
				ViewModel.AvailableIngredients.Insert(0, ViewModel.Recipe.Ingredients[index]);
			}

			// Configure Ingredient Spinner
			var ingredientSpinner = dialog.FindViewById<Spinner>(Resource.Id.ingredientspinner);
			ingredientSpinner.ItemSelected += (sender, e) =>
			{
				ingredient = ViewModel.AvailableIngredients[e.Position];
				ingredient.Quantity = quantity;
			};

			// Ingredient Adapter
			var ingredientAdapter = new ArrayAdapter(Context, Resource.Layout.ListViewRow, ViewModel.AvailableIngredients);
			ingredientAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			ingredientSpinner.Adapter = ingredientAdapter;

			// Configure Quantity Spinner
			var quantitySpinner = dialog.FindViewById<Spinner>(Resource.Id.quantityspinner);
			quantitySpinner.ItemSelected += (sender, e) =>
			{
				quantity = ViewModel.Quantities[e.Position];
				ingredient.Quantity = quantity;
			};

			// Quantity Adapter
			var quantityAdapter = new ArrayAdapter(Context, Resource.Layout.ListViewRow, ViewModel.Quantities);
			quantityAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			quantitySpinner.Adapter = quantityAdapter;
			quantitySpinner.SetSelection(ViewModel.Quantities.IndexOf(quantity));

			return dialog;
		}
	}
}
