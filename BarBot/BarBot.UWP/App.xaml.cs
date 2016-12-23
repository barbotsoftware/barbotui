﻿using System;
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

namespace BarBot.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        #region Global app properties

        public WebSocketHandler webSocket { get; set; }

        public BarbotContext barbotDB { get; set; }

        public BLEPublisher blePublisher { get; set; }

        public BarbotIOController barbotIOController { get; set; }

        public string barbotID { get; set; }

        public Constants.BarbotStatus Status { get; set; }

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
            string endpoint = Constants.EndpointURL;
            barbotID = Constants.BarBotId;

            // Get database configuration
            try
            {
                List<BarbotConfig> config = barbotDB.BarbotConfigs.ToList();

                if (config.Count > 0)
                {
                    endpoint = config.ElementAt(0).apiEndpoint;
                    barbotID = config.ElementAt(0).barbotId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to open websocket connection.");
            }

            // Initialize bluetooth publisher
            blePublisher = new BLEPublisher(barbotID);

            // Initialize IO Controller
            List<Container> containers = barbotDB.Containers.Include(x => x.pump.ioPort).Include(x => x.flowSensor.ioPort).ToList();
            IceHopper iceHopper = barbotDB.IceHoppers.Include(x => x.stepper1).Include(x => x.stepper2).Include(x => x.stepper3).Include(x => x.stepper4).ToList().ElementAt(0);
            GarnishDispenser garnishDispenser = barbotDB.GarnishDispensers.Include(x => x.stepper1).Include(x => x.stepper2).Include(x => x.stepper3).Include(x => x.stepper4).ToList().ElementAt(0);
            CupDispenser cupDispenser = barbotDB.CupDispensers.Include(x => x.stepper1).Include(x => x.stepper2).Include(x => x.stepper3).Include(x => x.stepper4).ToList().ElementAt(0);
            barbotIOController = new BarbotIOController(containers,
                iceHopper,
                garnishDispenser,
                cupDispenser);

            // Wait for IO controller to initialize
            while (!barbotIOController.Initialized)
            {
                Task.Delay(10);
            }

            // Initialize websocket connection
            webSocket = new WebSocketHandler();
            openWebSocket(endpoint);

            // Wait until the websocket connection is open
            while (!webSocket.IsOpen)
            {
                Task.Delay(10);
            }
        }

        public async void openWebSocket(string endpoint)
        {
            await webSocket.OpenConnection(String.Format("{0}?id=barbot_{1}", endpoint, barbotID));
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
