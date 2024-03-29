﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Detail
{
	public class IngredientDialogFragment : DialogFragment
	{
		Context context;
		RecipeDetailViewModel ViewModel => App.Locator.Detail;
		int index;

		public static IngredientDialogFragment NewInstance(Bundle bundle, Context c, int position)
		{
			var fragment = new IngredientDialogFragment();
			fragment.Arguments = bundle;
			fragment.context = c;
			fragment.index = position;
			return fragment;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			var ingredient = ViewModel.Recipe.Ingredients[index];
			var quantity = ingredient.Amount;

			AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(Activity, Resource.Style.BarBotTheme_AlertDialog));
			builder.SetTitle(Resource.String.title_ingredient_alert);

			LayoutInflater inflater = Activity.LayoutInflater;
			builder.SetView(inflater.Inflate(Resource.Layout.IngredientDialogFragment, null));

			builder.SetPositiveButton(Resource.String.positive_ingredient_alert, (sender, e) =>
			{
				// commits all changes to ViewModel
				ViewModel.Recipe.Ingredients[index] = ingredient;
				ViewModel.AvailableIngredients.Remove(ingredient);
				ViewModel.IsCustomRecipe = true;
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
			if (index < (ViewModel.Recipe.Ingredients.Count - 1))
			{
				ViewModel.AvailableIngredients.Insert(0, ViewModel.Recipe.Ingredients[index]);
			}

			// Configure Ingredient Spinner
			var ingredientSpinner = dialog.FindViewById<Spinner>(Resource.Id.ingredientspinner);
			ingredientSpinner.ItemSelected += (sender, e) =>
			{
				ingredient = ViewModel.AvailableIngredients[e.Position];
				ingredient.Amount = quantity;
			};

			// Ingredient Adapter
			var ingredientAdapter = new ArrayAdapter(context, Resource.Layout.IngredientSpinnerTextView, ViewModel.AvailableIngredients);
			ingredientAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			ingredientSpinner.Adapter = ingredientAdapter;

			// Configure Quantity TextView
			var quantityTextView = dialog.FindViewById<TextView>(Resource.Id.quantityTextView);
			quantityTextView.Text = ingredient.Amount + " oz";

			// Configure Quantity Increment Button
			var quantityIncrementButton = dialog.FindViewById<Button>(Resource.Id.quantityIncrementButton);
			quantityIncrementButton.Click += (sender, e) =>
			{
				var quantityIndex = ViewModel.Quantities.IndexOf(ingredient.Amount);
				if (quantityIndex < ViewModel.Quantities.Count - 1)
				{
					quantity = ViewModel.Quantities[quantityIndex + 1];
					ingredient.Amount = quantity;
					quantityTextView.Text = ingredient.Amount + " oz";
				}
			};

			// Configure Quantity Decrement Button
			var quantityDecrementButton = dialog.FindViewById<Button>(Resource.Id.quantityDecrementButton);
			quantityDecrementButton.Click += (sender, e) =>
			{
				var quantityIndex = ViewModel.Quantities.IndexOf(ingredient.Amount);
				if (quantityIndex > 0)
				{
					quantity = ViewModel.Quantities[quantityIndex - 1];
					ingredient.Amount = quantity;
					quantityTextView.Text = ingredient.Amount + " oz";
				}
			};

			return dialog;
		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
		}
	}
}
