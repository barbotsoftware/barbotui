using CoreGraphics;
using UIKit;

using GalaSoft.MvvmLight.Helpers;

using BarBot.iOS.Style;

namespace BarBot.iOS.View
{
    public abstract class BaseView : UIView
    {
        protected BaseView(CGRect Frame) : base(Frame)
        {
            BackgroundColor = Color.BackgroundGray;
        }

        protected void ConfigureUILabel(UILabel Label, string title)
        {
            Label.Text = title;
        }

		protected void ConfigureUIButton(UIButton Button, string title, System.Windows.Input.ICommand command)
		{
            // Set Title
			Button.SetTitle(title, UIControlState.Normal);
			
            // Set Command
            Button.SetCommand("TouchUpInside", command);

            // Style Button
            SharedStyles.StyleUIButton(Button);
		}
    }
}
