using CoreGraphics;
using UIKit;
namespace BarBot.iOS.View.Menu.Animation
{
	public class PresentationAnimator : UIViewControllerAnimatedTransitioning
	{
		public CGRect openingFrame { get; set; }

		public PresentationAnimator()
		{
		}

		public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
		{
			return 0.5;
		}

		public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
		{
			var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
			var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
			var containerView = transitionContext.ContainerView;

			var animationDuration = TransitionDuration(transitionContext);

			// add blurred background to the view
			var fromViewFrame = fromViewController.View.Frame;

			UIGraphics.BeginImageContext(fromViewFrame.Size);

			fromViewController.View.DrawViewHierarchy(fromViewFrame, true);

			var snapshotImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			var snapshotView = toViewController.View.ResizableSnapshotView(toViewController.View.Frame, true, UIEdgeInsets.Zero);

			snapshotView.Frame = openingFrame;
			containerView.AddSubview(snapshotView);

			toViewController.View.Alpha = 0.0f;

			containerView.AddSubview(toViewController.View);

			//UIView.Animate(animationDuration, 0.0f, 0.8f, null,
			//	() =>
			//    {
			//	   imageView.Center =
			//		   new CGPoint(UIScreen.MainScreen.Bounds.Right - imageView.Frame.Width / 2, imageView.Center.Y);
			//    },
			//	() =>
			//	{
			//		imageView.Center = pt;
			//	}
			//);
		}
	}
}

