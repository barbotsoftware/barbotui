using UIKit;
using Foundation;

namespace BarBot.iOS.Util
{
	public static class KeyboardManager
	{
		public static void ConfigureKeyboard(UITextField field, UIAlertAction action, string placeholder, UIKeyboardType keyboardType)
		{
			field.Placeholder = placeholder;
			field.AutocorrectionType = UITextAutocorrectionType.No;
			field.AutocapitalizationType = UITextAutocapitalizationType.Words;
			field.EnablesReturnKeyAutomatically = true;
			field.KeyboardType = keyboardType;
			field.KeyboardAppearance = UIKeyboardAppearance.Dark;
			field.ReturnKeyType = UIReturnKeyType.Default;
			field.Delegate = new TextFieldDelegate();
			field.AddTarget((sender, e) => { TextChanged(action, field); }, UIControlEvent.EditingChanged);
		}

		// Enable Action when Text is not empty
		static void TextChanged(UIAlertAction actionToEnable, UITextField sender)
		{
			actionToEnable.Enabled = (sender.Text != "");
		}

		public class TextFieldDelegate : UITextFieldDelegate
		{
			public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
			{
				string resultText = textField.Text.Substring(0, (int)range.Location) + replacementString + textField.Text.Substring((int)(range.Location + range.Length));
				return resultText.Length <= 32;
			}
		}
	}
}
