using UIKit;
using CoreGraphics;

namespace BarBot.iOS.View
{
	public class Hexagon : UIImageView
	{
		public Hexagon(string img, CGPoint point, CGSize size)
		{
			Image = UIImage.FromFile(img);
			Frame = new CGRect(point, size);
		}

		public override bool PointInside(CGPoint point, UIEvent uievent)
		{
			var p = UIBezierPath.FromOval(Frame);
			return p.ContainsPoint(point);
		}
	}
}
