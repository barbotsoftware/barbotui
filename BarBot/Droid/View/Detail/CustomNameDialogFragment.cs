using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Detail
{
	public class CustomNameDialogFragment : DialogFragment
	{
		DetailViewModel ViewModel => App.Locator.Detail;

		EditText NameEditText;

		public static CustomNameDialogFragment NewInstance(Bundle bundle)
		{
			var fragment = new CustomNameDialogFragment();
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
			builder.SetTitle(Resource.String.title_custom_alert);

			LayoutInflater inflater = Activity.LayoutInflater;
			builder.SetView(inflater.Inflate(Resource.Layout.CustomNameDialogFragment, null));

			builder.SetPositiveButton(Resource.String.positive_custom_alert, (EventHandler<DialogClickEventArgs>)null);
			builder.SetNegativeButton(Resource.String.negative_custom_alert, (sender, e) =>
			{
				ViewModel.ShowDrinkMenuCommand(false);
			});

			// Create Dialog
			var dialog = builder.Create();
			dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
			dialog.Show();

			// Add Event handler to Submit Button
			var submitButton = dialog.GetButton((int)DialogButtonType.Positive);
			submitButton.Click += SubmitAction;

			NameEditText = dialog.FindViewById<EditText>(Resource.Id.custom_drink_name);
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

		void SubmitAction(object sender, EventArgs e)
		{
			if (NameEditText != null && NameEditText.Text.Length > 0)
			{
				ViewModel.Recipe.Name = NameEditText.Text;
				(Activity as DrinkDetailActivity).Reload();
				Dismiss();
			}
		}
	}
}
