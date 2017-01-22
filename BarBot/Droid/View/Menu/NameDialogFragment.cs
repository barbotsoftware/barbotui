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

namespace BarBot.Droid.View.Menu
{
	public class NameDialogFragment : DialogFragment
	{
		public static NameDialogFragment NewInstance(Bundle bundle)
		{
			var fragment = new NameDialogFragment();
			fragment.Arguments = bundle;
			return fragment;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
			builder.SetTitle(Resource.String.title_name_alert);

			LayoutInflater inflater = Activity.LayoutInflater;
			builder.SetView(inflater.Inflate(Resource.Layout.NameDialogFragment, null));

			builder.SetPositiveButton(Resource.String.submit_name_alert, SubmitAction);

			// Create Dialog
			var dialog = builder.Create();
			dialog.Window.SetSoftInputMode(SoftInput.StateVisible);

			return dialog;
		}

		async void SubmitAction(object sender, DialogClickEventArgs e)
		{
			var editText = (EditText)Dialog.FindViewById(Resource.Id.user_name);
			if (editText != null)
			{
				var user = await App.RESTService.SaveUserNameAsync(editText.Text);

				if (user.Uid.Equals("name_taken"))
				{
					// name is taken
				}
				else
				{
					// Save value
					//Delegate.UserDefaults.SetString(user.Uid, "UserId");

					// Set to App
					App.User = user;

					// Sync changes to database
					//Delegate.UserDefaults.Synchronize();

					App.ConnectWebSocket();
					Dismiss();
				}
			}
		}
	}
}
