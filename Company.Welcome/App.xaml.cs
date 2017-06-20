﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Company.Welcome.Commons;
using Company.Welcome.Core;
using Company.Welcome.Core.Bootstrap;
using Company.Welcome.Core.Navigation;
using Company.Welcome.Views;
using Company.Welcome.Views.Home;

namespace Company.Welcome
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
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif
            var mainPage = StartTheApp();
            Frame rootFrame = mainPage.MainFrame;

            Window.Current.Content = mainPage;
            rootFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            Window.Current.Activate();
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                delegate (object sender, BackRequestedEventArgs args)
                {
                    var navigationService = ServiceLocator.Current.GetInstance<INavigationService<ApplicationPages>>();
                    if (navigationService.CanGoBack())
                    {
                        navigationService.GoBack();
                    }
                    args.Handled = true;
                };

            rootFrame.Navigate(typeof(HomePage), e.Arguments);
        }

        private static MainPage StartTheApp()
        {
            var container = new UnityContainer();
            var unityServiceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
            WelcomeApp.Start(container);
            var mainPage = new MainPage();
            var rootFrame = mainPage.MainFrame;
            BootstrapNavigationService.Configure(container, rootFrame);
            mainPage.LoadViewModel<ApplicationPages>();
            return mainPage;
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
