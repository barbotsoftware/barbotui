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
		DetailViewModel ViewModel => App.Locator.Detail;

		public IngredientAdapter(Context context, List<Ingredient> ingredients) : base(context, 0, ingredients)
		{
		}

		public override int Count
		{
			get
			{
				return ViewModel.Ingredients.Count;
			}
		}

		public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

			var ingredient = ViewModel.Ingredients[position];
			Button removeButton = new Button(Context);

			// Check if an existing view is being reused, otherwise inflate the view
			if (convertView == null)
			{
				convertView = inflater.Inflate(Resource.Layout.ListViewRow, null);
				removeButton = convertView.FindViewById<Button>(Resource.Id.listview_removebutton);
				removeButton.Click += (sender, e) =>
				{
					ViewModel.Ingredients.RemoveAt(position);
					Remove(position);
					NotifyDataSetChanged();
				};
			}

			// Lookup view for data population
			var ingredientRow = convertView.FindViewById<TextView>(Resource.Id.listview_textview);

			// Populate the data into the template view using the data object
			if (ingredient.IngredientId == Constants.AddIngredientId)
			{
				ingredientRow.Text = ingredient.Name;
				removeButton.Visibility = ViewStates.Invisible;
			}
			else
			{
				ingredientRow.Text = ingredient.Quantity + " oz " + ingredient.Name;
				removeButton.Visibility = ViewStates.Visible;
			}

			// Return the completed view to render on screen
			return convertView;
		}
	}
}
