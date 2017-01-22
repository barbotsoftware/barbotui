using System.Collections.Generic;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

using BarBot.Droid.Util;
using BarBot.Droid.View.Detail;
using BarBot.Droid.View.Menu;
using BarBot.Droid.WebSocket;

namespace BarBot.Droid
{
	public static class App
	{
		private static ViewModelLocator locator;
		private static WebSocketUtil webSocketUtil;
		private static RESTService restService;
		private static List<Ingredient> ingredientsInBarBot;
		private static User user;

		public static ViewModelLocator Locator
		{
			get
			{
				if (locator == null)
				{
					// MVVMLight's DispatcherHelper for cross-thread handling.
					DispatcherHelper.Initialize();

					// Initialize NavigationService
					var nav = new Util.NavigationServiceExtension();
					nav.Initialize();

					// Register NavigationService interfaces
					SimpleIoc.Default.Register<INavigationService>(() => nav);
					SimpleIoc.Default.Register<INavigationServiceExtension>(() => nav);

					// Configure PageKeys
					nav.Configure(
					  ViewModelLocator.DrinkMenuKey,
					  typeof(DrinkMenuActivity));

					nav.Configure(
					  ViewModelLocator.DrinkDetailKey,
					  typeof(DrinkDetailActivity));

					locator = new ViewModelLocator();
				}

				return locator;
			}
		}

		public static WebSocketUtil WebSocketUtil 
		{ 
			get
			{
				if (webSocketUtil == null)
				{
					webSocketUtil = new WebSocketUtil(new DroidWebSocketHandler());
					webSocketUtil.EndPoint = "ws://" + HostName + ":" + Constants.PortNumber;
				}

				return webSocketUtil;
			}
		}

		public static RESTService RESTService
		{
			get
			{
				if (restService == null)
				{
					restService = new RESTService(HostName);
				}

				return restService;
			}
		}

		public static List<Ingredient> IngredientsInBarBot
		{
			get
			{
				if (ingredientsInBarBot == null)
				{
					ingredientsInBarBot = new List<Ingredient>();
				}

				return ingredientsInBarBot;
			}

			set
			{
				ingredientsInBarBot = value;
			}
		}

		public static User User
		{
			get
			{
				if (user == null)
				{
					user = new User();
					//user.Uid = "user_3f2bb9";
				}

				return user;
			}
			set
			{
				user = value;
			}
		}

		public static string HostName 
		{ 
			get 
			{
				return Constants.IPAddress;
			} 
		}
	}
}
