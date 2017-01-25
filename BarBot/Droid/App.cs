using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Preferences;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;

using Calligraphy;

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
	[Application(ManageSpaceActivity = typeof(DrinkMenuActivity))]
	public class App : Application
	{
		private static ViewModelLocator locator;
		private static WebSocketUtil webSocketUtil;
		private static RESTService restService;
		private static List<Ingredient> ingredientsInBarBot;
		private static User user;
		private static string hostName;

		public App(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
		}

		public override void OnCreate()
		{
			base.OnCreate();
			CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
					.SetFontAttrId(Resource.Attribute.fontPath)
					.Build()
			);
		}

		public static ISharedPreferences Preferences
		{
			get
			{
				return PreferenceManager.GetDefaultSharedPreferences(Context);
			}
		}

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
					restService = new RESTService(Constants.IPAddress);
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
					// Check for stored UserID
					if (Preferences.GetString("UserId", null) != null)
					{
						user.Uid = Preferences.GetString("UserId", "");
					}
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
				if (Preferences.GetString("HostName", null) != null)
				{
					hostName = Preferences.GetString("HostName", "");
				}
				else
				{
					hostName = Constants.IPAddress;
					Preferences.Edit().PutString("HostName", hostName);
					Preferences.Edit().Commit();
				}

				return hostName;
			} 
		}

		// WEBSOCKET
		public static void ConnectWebSocket()
		{
			if (webSocketUtil != null)
			{
				// Close WebSocket if Reconnecting
				if (WebSocketUtil.Socket.IsOpen)
				{
					Locator.Menu.Recipes.Clear();
					WebSocketUtil.CloseWebSocket();
				}

				// Open WebSocket
				WebSocketUtil.OpenWebSocket(App.User.Uid, true);
			}
		}
	}
}
