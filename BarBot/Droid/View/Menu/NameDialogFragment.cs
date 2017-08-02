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
		Context context;
		EditText NameEditText;

		public static NameDialogFragment NewInstance(Bundle bundle, Context c)
		{
			var fragment = new NameDialogFragment();
			fragment.Arguments = bundle;
			fragment.context = c;
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

			NameEditText = dialog.FindViewById<EditText>(Resource.Id.user_name);
			NameEditText.EditorAction += HandleEditorAction;

			return dialog;
		}

		// Keyboard Done button submission
		private void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
		{
			e.Handled = false;
			if (e.ActionId == Android.Views.InputMethods.ImeAction.Done)
			{
				SubmitAction(sender, e);
				e.Handled = true;
			}
		}

		async void SubmitAction(object sender, EventArgs e)
		{
			if (NameEditText != null && NameEditText.Text.Length > 0)
			{
				var user = await App.RestService.RegisterUser(NameEditText.Text, "", "");

				if (user.UserId.Equals("name_taken"))
				{
					Toast toast = Toast.MakeText(context, "That name is taken", ToastLength.Long);
					toast.SetGravity(GravityFlags.Center, 0, 0);
					toast.Show();
				}
				else if (user.UserId.Equals("exception"))
				{
					Dismiss();
				}
				else
				{
					// Set to App
					App.User = user;

					App.ConnectWebSocket();
					Dismiss();
				}
			}
		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
		}
	}
}
