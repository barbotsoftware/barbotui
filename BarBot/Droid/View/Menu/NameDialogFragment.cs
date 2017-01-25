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

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(Activity, Resource.Style.BarBotTheme_AlertDialog));
			builder.SetTitle(Resource.String.title_name_alert);

			LayoutInflater inflater = Activity.LayoutInflater;
			builder.SetView(inflater.Inflate(Resource.Layout.NameDialogFragment, null));

			builder.SetPositiveButton(Resource.String.submit_name_alert, (EventHandler<DialogClickEventArgs>)null);

			// Create Dialog
			var dialog = builder.Create();
			dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
			dialog.Show();

			// Add Event handler to Submit Button
			var submitButton = dialog.GetButton((int)DialogButtonType.Positive);
			submitButton.Click += SubmitAction;

			return dialog;
		}

		async void SubmitAction(object sender, EventArgs e)
		{
			var editText = (EditText)Dialog.FindViewById(Resource.Id.user_name);
			if (editText != null && editText.Text.Length > 0)
			{
				var user = await App.RESTService.SaveUserNameAsync(editText.Text);

				if (user.Uid.Equals("name_taken"))
				{
					Toast.MakeText(Context, "That name is taken", ToastLength.Long).Show();
				}
				else if (user.Uid.Equals("exception"))
				{
					Dismiss();
				}
				else
				{
					// Save value
					App.Preferences.Edit().PutString("UserId", user.Uid);

					// Set to App
					App.User = user;

					// Sync changes to database
					App.Preferences.Edit().Commit();

					App.ConnectWebSocket();
					Dismiss();
				}
			}
		}
	}
}
