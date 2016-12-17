using CoreGraphics;
using UIKit;
namespace BarBot.iOS.View.Menu.Animation
{
	public class TransitioningDelegate : UIViewControllerTransitioningDelegate
	{
		CGRect openingFrame;

		public TransitioningDelegate()
		{
		}

		public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
		{
			var presentationAnimator = new PresentationAnimator();
			presentationAnimator.openingFrame = openingFrame;
			return presentationAnimator;
		}

		public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
		{
			var dismissAnimator = new DismissalAnimator();
			dismissAnimator.openingFrame = openingFrame;
			return dismissAnimator;
		}
	}
}
