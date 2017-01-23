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

using BarBot.Core;

namespace BarBot.Droid.View.Detail
{
	public class VolumeDialogFragment : DialogFragment
	{
		public static VolumeDialogFragment NewInstance(Bundle bundle)
		{
			var fragment = new VolumeDialogFragment();
			fragment.Arguments = bundle;
			return fragment;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(Activity, Resource.Style.BarBotTheme_AlertDialog));
			builder.SetTitle(Resource.String.title_volume_alert);
			var message = GetString(Resource.String.message_volume_alert) + " " + Constants.MaxVolume + " oz";
			builder.SetMessage(message);
			builder.SetPositiveButton(Resource.String.ok_volume_alert, (sender, e) =>
			{
				Dismiss();
			});

			return builder.Create();
		}
	}
}
