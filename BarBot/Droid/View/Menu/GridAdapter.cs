using Android.Content;
using Android.Views;
using Android.Widget;

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

			Android.Views.View gridView;

			if (convertView == null)
			{
				var recipe = ViewModel.Recipes[position];

				gridView = new Android.Views.View(context);

				// get layout from hexagon.xml
				gridView = inflater.Inflate(Resource.Layout.hexagon, null);

				var hexagonImageView = (ImageView)gridView.FindViewById(Resource.Id.hexagon_tile);
				var drinkImageView = (ImageView)gridView.FindViewById(Resource.Id.hexagon_drink_image);

				// resize drink imageview
				drinkImageView.LayoutParameters = hexagonImageView.LayoutParameters;

				// load drink image
				var url = "http://" + App.HostName + "/" + recipe.Img;
				Picasso.With(context).Load(url).Into(drinkImageView);

				// populate recipe name
				var recipeNameTextView = (TextView)gridView.FindViewById(Resource.Id.hexagon_recipe_name);
				recipeNameTextView.Text = recipe.Name;
			}
			else 
			{
				gridView = convertView;
			}

			return gridView;
		}
	}
}
