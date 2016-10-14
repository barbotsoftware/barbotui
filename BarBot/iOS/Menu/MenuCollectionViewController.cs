using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using BarBot.WebSocket;
using BarBot.Model;
using System.Threading.Tasks;
using CoreGraphics;
using System.Drawing;

namespace BarBot.iOS.Menu
{
	public class MenuCollectionViewController : UICollectionViewController
	{
		WebSocketHandler socket;
		private MenuSource source;

		public MenuCollectionViewController(UICollectionViewLayout layout) : base(layout)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "BarBot";
			CollectionView.BackgroundColor = UIColor.White;

			source = new MenuSource();
			source.FontSize = 11f;
			source.ImageViewSize = new SizeF(70, 52.64f);

			CollectionView.RegisterClassForCell(typeof(RecipeCell), RecipeCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;

			connectWebSocket();
		}

		public async void connectWebSocket()
		{
			socket = new WebSocketHandler();

			bool success = await socket.OpenConnection("ws://localhost:8000?id=barbot_805d2a");

			if (success)
			{
				Dictionary<String, Object> data = new Dictionary<String, Object>();
				data.Add("barbot_id", "barbot_805d2a");

				Message message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

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
					CollectionView.ReloadData();
				}
			}));
		}
	}
}
