using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
using BarBot.Core.Service.Login;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;

using BarBot.Droid.Service.Login;
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
		private static LoginService loginService;
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
					var nav = new NavigationService();

                    // Register Services with IoC Container
                    SimpleIoc.Default.Register<INavigationService>(() => nav);
                    SimpleIoc.Default.Register<IDialogService, DialogService>();
                    SimpleIoc.Default.Register<ILoginService, LoginService>();

                    // Configure PageKeys
                    nav.Configure(
					  ViewModelLocator.MenuPageKey,
					  typeof(DrinkMenuActivity));

					nav.Configure(
					  ViewModelLocator.RecipeDetailPageKey,
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

		public static LoginService LoginService
		{
			get
			{
				if (loginService == null)
				{
					loginService = new LoginService(Constants.IPAddress);
				}

				return loginService;
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
				//if (Preferences.GetString("HostName", null) != null)
				//{
				//	hostName = Preferences.GetString("HostName", "");
				//}
				//else
				//{
				hostName = Constants.IPAddress;
				//}

				return hostName;
			} 
		}

		// WEBSOCKET
		public static void ConnectWebSocket()
		{
			if (WebSocketUtil != null)
			{
				if (!WebSocketUtil.Socket.IsOpen)
				{
					// Open WebSocket
					WebSocketUtil.OpenWebSocket(user.UserId, true);
					while (!WebSocketUtil.Socket.IsOpen)
					{
						Task.Delay(10).Wait();
					}
				}
			}
		}

		public static void DisconnectWebSocket()
		{
			if (webSocketUtil != null)
			{
				if (WebSocketUtil.Socket.IsOpen)
				{
					WebSocketUtil.CloseWebSocket();
				}
			}
		}

		// Saves UserId
		public static void SaveSharedPreferences()
		{
			// Save values
			var editor = Preferences.Edit();
			editor.Clear();
			editor.PutString("UserId", user.UserId);
			//editor.PutString("HostName", hostName);

			// Sync changes to Shared Prefs
			editor.Commit();
		}

		public static void LoadSharedPreferences()
		{
			User.UserId = Preferences.GetString("UserId", "");
			//hostName = Preferences.GetString("HostName", "");
		}
	}
}
