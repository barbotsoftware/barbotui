using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Preferences;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;

using Calligraphy;

using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.Service.Login;
using BarBot.Core.ViewModel;

using BarBot.Droid.Service.Login;
using BarBot.Droid.View.Detail;
using BarBot.Droid.View.Menu;
using BarBot.Droid.WebSocket;
using BarBot.Core.Service.WebSocket;
using BarBot.Core.Service.Navigation;
using BarBot.Droid.View.Container;

namespace BarBot.Droid
{
	[Application(ManageSpaceActivity = typeof(DrinkMenuActivity))]
	public class App : Application
	{
		private static ViewModelLocator locator;
		private static WebSocketService webSocketService;
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

                    var nav = new Service.Navigation.NavigationService();
                    nav.Initialize();

                    // Register Services with IoC Container
                    SimpleIoc.Default.Register<INavigationService>(() => nav);
                    SimpleIoc.Default.Register<ILoginService>(() => LoginService);
                    SimpleIoc.Default.Register<IWebSocketService>(() => WebSocketService);

                    // Configure PageKeys
                    nav.Configure(ViewModelLocator.MenuPageKey,
					    typeof(DrinkMenuActivity));

					nav.Configure(ViewModelLocator.RecipeDetailPageKey,
					    typeof(DrinkDetailActivity));

                    nav.Configure(ViewModelLocator.ContainersPageKey,
                        typeof(ContainerActivity));

					locator = new ViewModelLocator();
				}

				return locator;
			}
		}

        public static WebSocketService WebSocketService
        {
            get
            {
                if (webSocketService == null)
                {
                    var host = "ws://" + HostName;
                    webSocketService = new WebSocketService(new DroidWebSocketHandler(),
                                                   Constants.BarBotId,
                                                   host);
                }

                return webSocketService;
            }
        }

		public static LoginService LoginService
		{
			get
			{
				if (loginService == null)
				{
                    var host = "http://" + HostName;
					loginService = new LoginService(host);
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
                    user = new User
                    {
                        Name = "panda",
                        Password = "password"
                    };
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
                hostName = Constants.IPAddress + ":" + Constants.PortNumber;
				return hostName;
			} 
		}

		// WEBSOCKET
		public static void ConnectWebSocket()
		{
			if (WebSocketService != null)
			{
                if (!WebSocketService.Socket.IsOpen)
				{
					// Open WebSocket
					WebSocketService.OpenWebSocket(user.Name, user.Password);
					while (!WebSocketService.IsOpen())
					{
						Task.Delay(10).Wait();
					}
				}
			}
		}

		public static void DisconnectWebSocket()
		{
            if (WebSocketService != null)
			{
                if (WebSocketService.IsOpen())
				{
                    WebSocketService.CloseWebSocket();
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
