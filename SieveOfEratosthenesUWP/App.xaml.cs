using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SieveOfEratosthenesUWP
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

        protected override void OnActivated(IActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // TODO: Initialize root frame just like in OnLaunched

            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;
                var args = toastActivationArgs.Argument.Split(':');
                switch (args[0])
                {
                    case "copy":
                        var outPrimes = new DataPackage();
                        outPrimes.SetText(args[1].Replace(",", "\r\n"));
                        Clipboard.SetContent(outPrimes);
                        break;


                    // Open the conversation
                    case "save":
                        SaveFile(args);
                        break;
                }
                if (rootFrame.BackStack.Count == 0)
                    rootFrame.BackStack.Add(new PageStackEntry(typeof(MainPage), null, null));
            }

            // TODO: Handle other types of activation
            Window.Current.Activate();
        }

        public async Task SaveFile(string[] args)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };

            savePicker.FileTypeChoices.Add("Plain Text File", new List<string> { ".txt" });

            savePicker.SuggestedFileName = string.Format("Primes - {0} to {1}", args[1], args[2]);
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);

                await Windows.Storage.FileIO.WriteTextAsync(file, args[3].Replace(",", "\r\n"));

                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
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
