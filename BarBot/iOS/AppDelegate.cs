using Foundation;
using UIKit;

using System.Collections.Generic;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Threading;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

using BarBot.iOS.Util;
using BarBot.iOS.View.Menu;
using BarBot.iOS.View.Detail;
using BarBot.iOS.WebSocket;

namespace BarBot.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		public override UIWindow Window { get; set; }
		public NSUserDefaults UserDefaults { get; set; }

		public WebSocketUtil WebSocketUtil { get; set; }
		public AsyncUtil AsyncUtil { get; set; }
		public List<Ingredient> IngredientsInBarBot { get; set; }
		public User User { get; set; }

		private string _hostName;
		public string HostName 
		{ 
			get { return _hostName; } 
			set 
			{ 
				_hostName = value;
				WebSocketUtil.EndPoint = "ws://" + value + ":" + Constants.PortNumber;
				AsyncUtil.HostName = "http://" + value;
			}
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			UserDefaults = NSUserDefaults.StandardUserDefaults;

			User = new User();
			// Check for stored UserID
			if (UserDefaults.StringForKey("UserId") != null)
			{
				User.Uid = UserDefaults.StringForKey("UserId");
			}

			// Initialize Ingredient List
			IngredientsInBarBot = new List<Ingredient>();

			// Initialize WebsocketHandler
			WebSocketUtil = new WebSocketUtil(new IosWebSocketHandler());

			// Initialize AsyncUtil
			AsyncUtil = new AsyncUtil();

			// Check for stored IP Address
			if (UserDefaults.StringForKey("HostName") != null)
			{
				HostName = UserDefaults.StringForKey("HostName");
			}
			else
			{
				HostName = Constants.HostName;
			}

			// create a new window instance based on the screen size
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			var initialViewController = new DrinkMenuViewController(new HexagonLayout());

			// Add the Navigation Controller and initialize it
			var navController = new UINavigationController(initialViewController);
			Window.RootViewController = navController;

			// make the window visible
			Window.MakeKeyAndVisible();

			// MVVM Light's DispatcherHelper for cross-thread handling.
			DispatcherHelper.Initialize(application);

			// Initialize and register the Navigation Service
			var nav = new Util.NavigationServiceExtension();
			SimpleIoc.Default.Register<INavigationService>(() => nav);
			SimpleIoc.Default.Register<INavigationServiceExtension>(() => nav);
			nav.Initialize(navController);
			nav.Configure(ViewModelLocator.DrinkMenuKey, typeof(DrinkMenuViewController));
			nav.Configure(ViewModelLocator.DrinkDetailKey, typeof(DrinkDetailViewController));

			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
			if (WebSocketUtil.Socket.IsOpen)
			{
				WebSocketUtil.CloseWebSocket();
			}
			UserDefaults.Synchronize();
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
			//if (!WebSocketUtil.Socket.IsOpen)
			//{
			//	WebSocketUtil.OpenWebSocket(User.Uid, true);
			//}
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
			if (WebSocketUtil.Socket.IsOpen)
			{
				WebSocketUtil.CloseWebSocket();
			}
			UserDefaults.Synchronize();
		}
	}
}
