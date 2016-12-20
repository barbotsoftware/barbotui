using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using BarBot.Core;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

namespace BarBot.iOS.View.Detail
{
	public class DrinkDetailViewController : UIViewController
    {
		private DetailViewModel ViewModel => Application.Locator.Detail;

		UIImageView DrinkImageView;
		UIImageView HexagonImageView;
		UIButton OrderButton;

		AppDelegate Delegate;
		WebSocketHandler Socket;
        
		public DrinkDetailViewController()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = Color.BackgroundGray;
			ConfigureHexagon();
			ConfigureOrderButton("ORDER DRINK", 20, View.Bounds.Bottom - 80, View.Bounds.Width - 40);

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			Socket = Delegate.Socket;
			GetRecipeDetails();
		}

		async void Reload()
		{
			Title = ViewModel.Recipe.Name.ToUpper();
			// TODO: Pass image instead of getting from ws server again
			DrinkImageView.Image = await AsyncUtil.LoadImage(ViewModel.Recipe.Img);
		}

		void ConfigureHexagon()
		{
			nfloat factor = 174.0f / 200.0f;
			var point = new CGPoint(View.Frame.X + 10, View.Frame.Y + 75);
			var imgPoint = new CGPoint(point.X, point.Y - 10);
			var size = new CGSize(View.Bounds.Width / 2, factor * (View.Bounds.Width / 2));

			HexagonImageView = new Hexagon("Images/HexagonTile.png",
										   point,
										   size);
			HexagonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			DrinkImageView = new UIImageView();
			DrinkImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			DrinkImageView.Frame = new CGRect(imgPoint, size);

			View.AddSubview(HexagonImageView);
			View.AddSubview(DrinkImageView);
		}

		void ConfigureOrderButton(string title, nfloat x, nfloat y, nfloat width)
		{
			OrderButton = UIButton.FromType(UIButtonType.System);
			OrderButton.Frame = new CGRect(x, y, width, 60);
			OrderButton.SetTitle(title, UIControlState.Normal);
			OrderButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			OrderButton.Font = UIFont.FromName("Microsoft-Yi-Baiti", 23f);
			OrderButton.BackgroundColor = Color.BarBotBlue;
			OrderButton.Layer.CornerRadius = new nfloat(2.0);
			OrderButton.Layer.BorderWidth = new nfloat(0.9);
			OrderButton.Layer.BorderColor = Color.BarBotBlue.CGColor;
			var insets = new UIEdgeInsets(new nfloat(2.0), new nfloat(2.0), new nfloat(2.0),
								 new nfloat(2.0));
			OrderButton.TitleEdgeInsets = insets;

			View.AddSubview(OrderButton);
		}

		public void GetRecipeDetails()
		{
			if (Socket.IsOpen)
			{
				var data = new Dictionary<string, object>();
				data.Add("recipe_id", ViewModel.RecipeId);

				var message = new Message(Constants.Command, Constants.GetRecipeDetails, data);

				Socket.GetRecipeDetailsEvent += Socket_GetRecipeDetailsEvent;

				Socket.sendMessage(message);
			}
		}

		private async void Socket_GetRecipeDetailsEvent(object sender, WebSocketEvents.GetRecipeDetailsEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				ViewModel.Recipe = args.Recipe;
			}));
			Reload();
		}
    }
}