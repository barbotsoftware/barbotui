using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BarBot.Core.WebSocket;
using BarBot.UWP.Database;
using BarBot.UWP.Bluetooth;
using BarBot.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BarBot.UWP.IO;
using System.Diagnostics;
using BarBot.UWP.Websocket;

namespace BarBot.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        #region Global App Properties

        public WebSocketUtil webSocketUtil { get; set; }

        public BarbotContext barbotDB { get; set; }

        public BLEPublisher blePublisher { get; set; }

        public BarbotIOController barbotIOController { get; set; }

        public string barbotID { get; set; }

        public string webserverUrl { get; set; }

        public Constants.BarbotStatus Status { get; set; }

        public List<Core.Model.DrinkOrder> DrinkOrders { get; set; }

        #endregion

        #region Global App Events

        public event DrinkOrderAddedHandler DrinkOrderAdded = delegate { };

        public delegate void DrinkOrderAddedHandler(object sender, DrinkOrderAddedEventArgs args);

        public class DrinkOrderAddedEventArgs : EventArgs
        {
            private Core.Model.DrinkOrder drinkOrder;

            public DrinkOrderAddedEventArgs(Core.Model.DrinkOrder drinkOrder)
            {
                this.drinkOrder = drinkOrder;
            }

            public Core.Model.DrinkOrder DrinkOrder
            {
                get { return drinkOrder; }
            }
        }

        #endregion

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;
            this.Suspending += OnSuspending;

            init();

            // Wait for initialization to finish
            while(!Status.Equals(Constants.BarbotStatus.READY))
            {
                Task.Delay(10);
            }
        }

        public void init()
        {
            Status = Constants.BarbotStatus.STARTING;

            // Initialize database connection
            barbotDB = new BarbotContext();

            // Migrate pending migrations
            barbotDB.Database.Migrate();

            // Default config values to fall back on
            string endpoint = "ws://" + Constants.IPAddress + ":" + Constants.PortNumber;
            barbotID = Constants.BarBotId;

            // Get database configuration
            try
            {
                List<BarbotConfig> config = barbotDB.BarbotConfigs.ToList();

                if (config.Count > 0)
                {
                    barbotID = config.ElementAt(0).barbotId;
                    webserverUrl = config.ElementAt(0).apiEndpoint;
                    endpoint = "ws://" + webserverUrl + ":" + Constants.PortNumber;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Failed to retrieve barbot configuration settings: {0}.", e.Message));
            }

            // Initialize bluetooth publisher
            blePublisher = new BLEPublisher(barbotID);

            // Initialize IO Controller
            try
            {
                barbotIOController = new BarbotIOController(barbotDB.getContainers(),
                    barbotDB.getIceHopper(),
                    barbotDB.getGarnishDispenser(),
                    barbotDB.getCupDispenser());

                // Wait for IO controller to initialize
                while (!barbotIOController.Initialized)
                {
                    Task.Delay(10);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Failed to initialize barbot controller: {0}", e.Message));
            }

            // Initialize websocket connection
            webSocketUtil = new WebSocketUtil(new UWPWebsocketHandler());
            webSocketUtil.EndPoint = endpoint;

            // Opening websocket, with barbID, false bool for isMobile
            webSocketUtil.OpenWebSocket("barbot_" + barbotID, false);

            // Wait until the websocket connection is open
            while (!webSocketUtil.Socket.IsOpen)
            {
                Task.Delay(10);
            }

            DrinkOrders = new List<Core.Model.DrinkOrder>();
            webSocketUtil.Socket.DrinkOrderedEvent += WebSocket_DrinkOrderedEvent;

            Status = Constants.BarbotStatus.READY;
        }

        private void WebSocket_DrinkOrderedEvent(object sender, WebSocketEvents.DrinkOrderedEventArgs args)
        {
            // Create a new DrinkOrder database model from the incoming websocket model
            DrinkOrders.Add(args.DrinkOrder);

            // Fire event
            DrinkOrderAdded(this, new DrinkOrderAddedEventArgs(args.DrinkOrder));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
