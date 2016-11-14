using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using BarBot.Model;
using BarBot.WebSocket;

namespace BarBot.iOS.Menu
{
	public class MenuCollectionViewController : UICollectionViewController
	{
		WebSocketHandler socket;
		MenuSource source;
		UIBarButtonItem customButton;

		public MenuCollectionViewController(UICollectionViewLayout layout) : base(layout)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "DRINK MENU";
			CollectionView.BackgroundColor = UIColor.Black;

			customButton = new UIBarButtonItem(
				UIImage.FromBundle("navigation_icon.png"),
				UIBarButtonItemStyle.Plain,
				(s, e) =>
				{
					System.Diagnostics.Debug.WriteLine("button tapped");
				}
			);
			NavigationItem.LeftBarButtonItem = customButton;

			source = new MenuSource(this);

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
