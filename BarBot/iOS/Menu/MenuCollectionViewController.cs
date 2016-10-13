using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using BarBot.WebSocket;
using BarBot.Model;
using System.Threading.Tasks;

namespace BarBot.iOS.Menu
{
	public class MenuCollectionViewController : UICollectionViewController
	{
		static NSString recipeCellId = new NSString("RecipeCell");
		WebSocketHandler socket;
		List<Recipe> recipes;

		public MenuCollectionViewController(UICollectionViewLayout layout) : base(layout)
		{
			recipes = new List<Recipe>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			CollectionView.RegisterClassForCell(typeof(RecipeCell), recipeCellId);
			init();
		}

		public async void init()
		{
			socket = new WebSocketHandler();

			bool success = await socket.OpenConnection("ws://192.168.1.36:8000?id=barbot_805d2a");

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
				this.recipes = args.Recipes;
			}));
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return recipes.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			var recipeCell = (RecipeCell)collectionView.DequeueReusableCell(recipeCellId, indexPath);

			var recipe = recipes[indexPath.Row];

			recipeCell.Recipe = recipe;

			return recipeCell;
		}
	}
}
