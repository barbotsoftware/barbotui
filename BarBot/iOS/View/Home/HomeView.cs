using CoreGraphics;
using Foundation;
using UIKit;

using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Home
{
    public class HomeView : BaseView
    {
		// Constants
        const int BUTTON_LEFT_OFFSET = 20;

		// ViewModel
        HomeViewModel Vm => Application.Locator.Home;

		// UI Elements
		public UILabel TitleLabel;
        public UIImageView LogoImageView;
        public UIButton LoginButton;
        public UIButton SignUpButton;

        [Export("initWithFrame:")]
        public HomeView(CGRect Frame) : base(Frame)
        {
            ConfigureTitleLabel();
            ConfigureLogoImageView();
            ConfigureLoginButton();
            ConfigureSignUpButton();
        }

        void ConfigureTitleLabel()
        {
            TitleLabel = new UILabel();

            ConfigureUILabel(TitleLabel, Vm.TitleText);

            AddSubview(TitleLabel);
        }

        void ConfigureLogoImageView()
        {
            
        }

        void ConfigureLoginButton()
        {
            // Instantiate UIButton
            LoginButton = UIButton.FromType(UIButtonType.System);

			// Set Frame
			LoginButton.Frame = new CGRect(BUTTON_LEFT_OFFSET,
										   Bounds.Bottom - 160,
										   Bounds.Width - (BUTTON_LEFT_OFFSET * 2),
										   60);
            // Configure Button
            ConfigureUIButton(LoginButton, Vm.LoginButtonText, Vm.GoToLoginCommand);

            // Add to View
            AddSubview(LoginButton);
        }

        void ConfigureSignUpButton()
        {
            // Instantiate UIButton
			SignUpButton = UIButton.FromType(UIButtonType.System);

            // Set Frame
			SignUpButton.Frame = new CGRect(BUTTON_LEFT_OFFSET,
										   Bounds.Bottom - 80,
										   Bounds.Width - (BUTTON_LEFT_OFFSET * 2),
										   60);
            // Configure Button
            ConfigureUIButton(SignUpButton, Vm.SignUpButtonText, Vm.GoToSignUpCommand);

            // Add to View
            AddSubview(SignUpButton);
        }
    }
}
