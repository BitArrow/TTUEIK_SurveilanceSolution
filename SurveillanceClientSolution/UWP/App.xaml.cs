using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Restup.Webserver.Attributes;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.Services;
using UWP.Services.Interfaces;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            RegisterServices();
            var pwVault = new PasswordVaultService();
            pwVault.LoadExistingTokens();

            RegisterRestup();
        }

        public static string WebServer { get { return "https://localhost:44370"; } }
        public static string WebApiEndpoint { get { return $"{WebServer}/api/"; } }

        #region B2C params
        public static string BearerToken { get; set; }
        public static DateTime BearerExpires { get; set; }
        public static string RefreshToken { get; set; }
        public static DateTime RefreshTokenExpires { get; set; }

        public static string SignInPolicy = "<Policy>"; // add your own signin policy
        public static string ClientId = "<ClientId>"; // your app id
        public static string ApplicationRedirect = "urn:ietf:wg:oauth:2.0:oob"; // leave this
        public static string ResponseType = "code";  // leave this
        public static string Tenant = "<Tenant>"; // your aadb2c tenant
        public static string ResponseModeQuery = "query"; // leave this
        public static string StateData = "random_verifiable_string"; // your own string
        public static string OfflineScope = $"{ClientId} offline_access"; // leave this
        public static string BaseHost = "https://login.microsoftonline.com"; // leave this
        public static string TokenPath = $"/{Tenant}/oauth2/v2.0/token?p={SignInPolicy}"; // leave this
        public static string AuthorizePath = $"/{Tenant}/oauth2/v2.0/authorize?p={SignInPolicy}"; // leave this
        #endregion


        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IB2CService, B2CService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IPasswordVaultService, PasswordVaultService>();
            services.AddScoped<IHomeService, HomeService>();
            Container = services.BuildServiceProvider();
        }

        public static IServiceProvider Container { get; private set; }

        public static HttpServer HttpServer { get; set; }

        public async static void RegisterRestup()
        {
            try
            {
                var restRouteHandler = new RestRouteHandler();
                restRouteHandler.RegisterController<ParameterController>();

                var configuration = new HttpServerConfiguration()
                  .ListenOnPort(8800)
                  .RegisterRoute("api", restRouteHandler)
                  .EnableCors();

                HttpServer = new HttpServer(configuration);
                //await HttpServer.StartServerAsync();
            }
            catch (Exception ex)
            {

            }

            // now make sure the app won't stop after this (eg use a BackgroundTaskDeferral)
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
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

                // If back button is pressed
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

                // Event handler that is always activated when rootframe is navitaged
                rootFrame.Navigated += RootFrame_Navigated;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                Frame frame = Window.Current.Content as Frame;
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                    e.Handled = true;
                }
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
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

    public class DataReceived
    {
        public DateTime Time { get; set; }
        public string Code { get; set; }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class ParameterController
    {
        // http://192.168.0.11:8800/api/somestring
        [UriFormat("/{guid}")]
        public IGetResponse GetWithSimpleParameters(string guid)
        {
            Debug.Write(guid);
            return new GetResponse(
              GetResponse.ResponseStatus.OK,
              new DataReceived() { Time = DateTime.Now, Code = guid });
        }
    }
}
