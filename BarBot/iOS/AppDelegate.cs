using Foundation;
using UIKit;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BarBot.Core;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using BarBot.iOS.View.Menu;
using BarBot.iOS.View.Detail;
using System.Threading.Tasks;

namespace BarBot.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		public override UIWindow Window { get; set; }
		public WebSocketHandler Socket { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// create a new window instance based on the screen size
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			var initialViewController = new DrinkMenuViewController(new HexagonLayout());

			// Add the Navigation Controller and initialize it
			var navController = new UINavigationController(initialViewController);
			Window.RootViewController = navController;

			// make the window visible
			Window.MakeKeyAndVisible();

			// Initialize and register the Navigation Service
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			var nav = new NavigationService();
			nav.Initialize(navController);
			nav.Configure(ViewModelLocator.DrinkDetailKey, typeof(DrinkDetailViewController));

			SimpleIoc.Default.Register<INavigationService>(() => nav);

			// Initialize WebsocketHandler
			Socket = new IosWebsocketHandler();
			OpenWebSocket(Constants.EndpointURL, Constants.BarBotId);

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
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		public async void OpenWebSocket(string endpoint, string barbotId)
		{
			await Socket.OpenConnection(endpoint + "?id=" + barbotId);
		}
	}
}


