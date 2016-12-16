using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using CoreGraphics;
using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.Core.ViewModel;

namespace BarBot.iOS.View.Menu
{
    public class DrinkMenuViewController : UICollectionViewController
    {
		//MenuViewModel ViewModel;
		AppDelegate Delegate;
		WebSocketHandler Socket;
		string BarBotId;
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

			Delegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			Socket = Delegate.Socket;
			BarBotId = Constants.BarBotId;

			GetRecipes();

			// if new user
			//ShowAlert();
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

		public void ShowAlert()
		{
			// Create Alert
			var nameInputAlertController = UIAlertController.Create("Enter your name", null, UIAlertControllerStyle.Alert);

			//Add Text Input
            nameInputAlertController.AddTextField(textField =>
			{
			});

			//  Add Action
			nameInputAlertController.AddAction(UIAlertAction.Create("Submit", UIAlertActionStyle.Default, null));

			// Present Alert
			PresentViewController(nameInputAlertController, true, null);
		}

		public async void GetRecipes()
		{
			bool success = await Socket.OpenConnection(Constants.EndpointURL + "?id=" + Constants.BarBotId);

			if (success)
			{
				var data = new Dictionary<string, object>();
				data.Add("barbot_id", BarBotId);

				var message = new Message(Constants.Command, Constants.GetRecipesForBarbot, data);

				Socket.GetRecipesEvent += Socket_GetRecipesEvent;

				Socket.sendMessage(message);
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