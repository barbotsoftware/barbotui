using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;

namespace BarBot.iOS.View.Menu
{
    public class DrinkMenuViewController : UICollectionViewController
    {
		WebSocketHandler socket;
		MenuSource source;

        public DrinkMenuViewController(UICollectionViewLayout layout) : base(layout)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "DRINK MENU";
			NavBarStyle(NavigationController.NavigationBar);
			NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, null);

			source = new MenuSource(this);

			CollectionView.RegisterClassForCell(typeof(RecipeCollectionViewCell), RecipeCollectionViewCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;

			connectWebSocket();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void NavBarStyle(UINavigationBar NavBar)
		{
			NavBar.TintColor = UIColor.White;
			NavBar.BarTintColor = Color.BackgroundGray;
			NavBar.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White,
				Font = UIFont.FromName("Microsoft-Yi-Baiti", 26f)
			};
			var NavBorder = new UIView(new CGRect(0,
												  NavBar.Frame.Size.Height - 1,
												  NavBar.Frame.Size.Width,
												  4));
			NavBorder.BackgroundColor = Color.BarBotBlue;
			NavBorder.Opaque = true;
			NavBar.AddSubview(NavBorder);
		}

		public async void connectWebSocket()
		{
			socket = new WebSocketHandler();

			bool success = await socket.OpenConnection(Constants.EndpointURL + "?id=" + Constants.BarBotId);

			if (success)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", Constants.BarBotId);

				var message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

				socket.GetRecipesEvent += Socket_GetRecipesEvent;

				socket.sendMessage(message);
			}
		}

		private async void Socket_GetRecipesEvent(object sender, WebSocketEvents.GetRecipesEventArgs args)
		{
			await Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				foreach (Recipe r in args.Recipes)
				{
					source.Rows.Add(r);
				}
				CollectionView.ReloadData();
			}));
		}
    }
}