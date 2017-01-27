using Android.Content;
using Android.Views;
using Android.Widget;

using BarBot.Core;
using BarBot.Core.ViewModel;

using Square.Picasso;

namespace BarBot.Droid.View.Menu
{
	public class GridAdapter : BaseAdapter
	{
		private MenuViewModel ViewModel => App.Locator.Menu;

		public GridAdapter()
		{
		}

		Context context;

		public GridAdapter(Context c)
		{
			context = c;
		}

		public override int Count
		{
			get { return ViewModel.Recipes.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

			Android.Views.View hexagonView;

			// get layout from hexagon.xml
			hexagonView = inflater.Inflate(Resource.Layout.Hexagon, null);

			var hexagonImageView = (ImageView)hexagonView.FindViewById(Resource.Id.hexagon_tile);
			var drinkImageView = (ImageView)hexagonView.FindViewById(Resource.Id.hexagon_drink_image);

			// resize drink imageview
			drinkImageView.LayoutParameters = hexagonImageView.LayoutParameters;
			drinkImageView.SetY(drinkImageView.GetY() - 15);

			// get Recipe
			var recipe = ViewModel.Recipes[position];

			// load drink image
			if (recipe.RecipeId == Constants.CustomRecipeId)
			{
				Picasso.With(context).Load(Resource.Drawable.custom_recipe).Fit().CenterInside().Into(drinkImageView);
			}
			else
			{
				var url = "http://" + App.HostName + "/" + recipe.Img;
				Picasso.With(context).Load(url).Fit().CenterInside().Into(drinkImageView);
			}
			// populate Recipe name
			var recipeNameTextView = (TextView)hexagonView.FindViewById(Resource.Id.hexagon_recipe_name);
			recipeNameTextView.Text = recipe.Name;

			return hexagonView;
		}
	}
}
