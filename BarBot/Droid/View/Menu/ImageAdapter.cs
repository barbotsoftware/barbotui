using Android.Content;
using Android.Views;
using Android.Widget;

namespace BarBot.Droid.View.Menu
{
	public class ImageAdapter : BaseAdapter
	{
		Context context;

		public ImageAdapter(Context c)
		{
			context = c;
		}

		public override int Count
		{
			get { return thumbIds.Length; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
		{
			ImageView imageView;

			if (convertView == null)
			{  // if it's not recycled, initialize some attributes
				imageView = new ImageView(context);
				//imageView.LayoutParameters = new GridView.LayoutParams(150, 150);
				imageView.SetMaxHeight(200);
				imageView.SetMaxWidth(200);
				imageView.SetAdjustViewBounds(true);
				imageView.SetScaleType(ImageView.ScaleType.FitEnd);
			}
			else {
				imageView = (ImageView)convertView;
			}

			imageView.SetImageResource(thumbIds[position]);
			return imageView;
		}

		// references to our images
		int[] thumbIds = {
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile,
			Resource.Drawable.HexagonTile, Resource.Drawable.HexagonTile
		};
	}
}
