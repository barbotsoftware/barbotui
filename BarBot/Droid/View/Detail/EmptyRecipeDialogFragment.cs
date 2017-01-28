using Android.App;
using Android.OS;
using Android.Views;

using BarBot.Core;

namespace BarBot.Droid.View.Detail
{
	public class EmptyRecipeDialogFragment : DialogFragment
	{
		public static EmptyRecipeDialogFragment NewInstance(Bundle bundle)
		{
			var fragment = new EmptyRecipeDialogFragment();
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
			builder.SetTitle(Resource.String.title_empty_alert);
			var message = GetString(Resource.String.message_empty_alert);
			builder.SetMessage(message);
			builder.SetPositiveButton(Resource.String.ok, (sender, e) =>
			{
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
