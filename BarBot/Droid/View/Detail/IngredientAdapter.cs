using System.Collections.Generic;

using Android.Content;
using Android.Views;
using Android.Widget;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Detail
{
	public class IngredientAdapter : ArrayAdapter<Ingredient>
	{
		RecipeDetailViewModel ViewModel => App.Locator.Detail;

		public IngredientAdapter(Context context, List<Ingredient> ingredients) : base(context, 0, ingredients)
		{
		}

		public override int Count
		{
			get
			{
				return ViewModel.Recipe.Ingredients.Count;
			}
		}

		public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

            var ingredient = ViewModel.Recipe.Ingredients[position];

			// Check if an existing view is being reused, otherwise inflate the view
			if (convertView == null)
			{
				convertView = inflater.Inflate(Resource.Layout.ListViewRow, null);
				var rmBtn = convertView.FindViewById<Button>(Resource.Id.listview_removebutton);
				rmBtn.Click += (sender, e) =>
				{
                    ViewModel.Recipe.Ingredients.RemoveAt(position);
					Remove(position);
					NotifyDataSetChanged();

					// Set Available Ingredients
					ViewModel.RefreshAvailableIngredients();
				};
			}

			// Lookup view for data population
			var ingredientRow = convertView.FindViewById<TextView>(Resource.Id.listview_textview);
			var removeButton = convertView.FindViewById<Button>(Resource.Id.listview_removebutton);

			// Populate the data into the template view using the data object
			if (ingredient.IngredientId == Constants.AddIngredientId)
			{
				ingredientRow.Text = ingredient.Name;
			}
			else
			{
				ingredientRow.Text = ingredient.Amount + " oz " + ingredient.Name;
			}

			// Set visibility of Remove Button
			if (position == Count - 1)
			{
				removeButton.Visibility = ViewStates.Invisible;
			}
			else
			{
				removeButton.Visibility = ViewStates.Visible;
			}

			// Return the completed view to render on screen
			return convertView;
		}
	}
}
