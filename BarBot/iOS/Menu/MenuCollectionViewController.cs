﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using BarBot.Model;
using BarBot.WebSocket;

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
			//source.ImageViewSize = new CGRect(CollectionView.Bounds.X,
			//                                  CollectionView.Bounds.Y,
			//                                  (CollectionView.Frame.Width / 2.0f) - 4.0f,
			//                                  CollectionView.Frame.Height - 10.0f);

			CollectionView.RegisterClassForCell(typeof(RecipeCell), RecipeCell.CellID);
			CollectionView.ShowsHorizontalScrollIndicator = false;
			CollectionView.Source = source;

			connectWebSocket();
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
					CollectionView.ReloadData();
				}
			}));
		}
	}
}
