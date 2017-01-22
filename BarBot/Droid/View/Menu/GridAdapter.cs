using System.Threading.Tasks;

using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

using BarBot.Core.ViewModel;

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

				// get layout from mobile.xml
				gridView = inflater.Inflate(Resource.Layout.hexagon_item, null);

				// set image based on selected text
				ImageView drinkImageView = (ImageView)gridView.FindViewById(Resource.Id.hexagon_drink_image);

				drinkImageView.SetImageBitmap(App.RESTService.GetImageBitmapFromUrl(recipe.Img));
			}
			else 
			{
				gridView = convertView;
			}

			return gridView;
		}

		//async Task<Bitmap> GetDrinkImage(int position)
		//{
		//	var recipe = ViewModel.Recipes[position];
		//	byte[] contents;

		//	if (!ViewModel.ImageCache.ContainsKey(recipe.Name))
		//	{
		//		// Load new Image
		//		contents = await App.RESTService.Get(recipe.Img);
		//		ViewModel.ImageCache.Add(recipe.Name, contents);
		//	}
		//	else
		//	{
		//		// find in Image Cache
		//		contents = ViewModel.ImageCache[recipe.Name];
		//	}

		//	return BitmapFactory.DecodeByteArray(contents, 0, contents.Length);
		//}
	}
}
