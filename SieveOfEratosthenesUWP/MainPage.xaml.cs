﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Notifications;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SieveOfEratosthenesUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public Sieve PageSieve;
        public ObservableCollection<long> DisplayStepList { get; set; }
        public ObservableCollection<long> DisplayPrimes { get; set; }
        private long MinInput => (long)TxtMin.Value.GetValueOrDefault(2);
        private long MaxInput => (long)TxtMax.Value.GetValueOrDefault(MinInput);
        private ThreadPoolTimer _autoTimer;

        public MainPage()
        {
            InitializeComponent();
            PageSieve = new Sieve();
            DisplayStepList = new ObservableCollection<long>();
            DisplayPrimes = new ObservableCollection<long>();
        }

        private void UpdateValues(object sender, EventArgs e)
        {
            ResetList();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetList();
        }

        private async void ResetList()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                EndAuto();
                PageSieve = new Sieve(MinInput, MaxInput);
                TxtMax.Minimum = MinInput;
                DisplayStepList = new ObservableCollection<long>(PageSieve.StepList);
                DisplayPrimes = new ObservableCollection<long>(PageSieve.Primes);
                StepBox.ItemsSource = DisplayStepList;
                PrimesBox.ClearValue(BackgroundProperty);
                PrimesBox.ItemsSource = DisplayPrimes;
            });
        }


        private async void btnStep_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PageSieve.Step();
                DisplayStepList = new ObservableCollection<long>(PageSieve.StepList);
                DisplayPrimes = new ObservableCollection<long>(PageSieve.Primes);
                StepBox.ItemsSource = DisplayStepList;
                PrimesBox.ItemsSource = DisplayPrimes;
                if (!DisplayStepList.Any() && DisplayPrimes.Any())
                {
                    PrimesBox.Background = new SolidColorBrush(Colors.LightGreen);
                }
                PrimesBox.ScrollIntoView(DisplayPrimes.Last());
                UpdateTile();
            });
        }

        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            if (!DisplayStepList.Any())
            {
                return;
            }
            StartAuto();
        }

        private async void StartAuto()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                BtnAuto.IsEnabled = false;
                _autoTimer = ThreadPoolTimer.CreatePeriodicTimer(autoTimer_Tick, TimeSpan.FromSeconds(1));
            });
        }

        private async void EndAuto()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                BtnAuto.IsEnabled = true;
                _autoTimer?.Cancel();
            });
        }

        private void autoTimer_Tick(ThreadPoolTimer sender)
        {
            if (!DisplayStepList.Any())
            {
                EndAuto();
                SendToasterNotification();
            }
            btnStep_Click(null, null);
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            EndAuto();
            DisplayStepList.Clear();
            DisplayPrimes.Clear();
            PageSieve.Solve();
            DisplayPrimes = new ObservableCollection<long>(PageSieve.Primes);
            PrimesBox.ItemsSource = DisplayPrimes;
            PrimesBox.Background = new SolidColorBrush(Colors.LightGreen);
            UpdateTile();
        }

        private void SendToasterNotification()
        {
            var toastContent = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = string.Format("{0} to {1}", MinInput, MaxInput),
                                HintMaxLines = 1
                            },

                            new AdaptiveText
                            {
                                Text = "# Of Primes: " + DisplayPrimes.Count
                            }

                        }
                    }
                }
            };
            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void UpdateTile()
        {
            var generalNotification = new TileBindingContentAdaptive
            {
                Children =
                {
                    new AdaptiveText
                    {
                        Text = "Min: " + MinInput,
                        HintStyle = AdaptiveTextStyle.Caption
                    },

                    new AdaptiveText
                    {
                        Text = "Max: " + MaxInput,
                        HintStyle = AdaptiveTextStyle.Caption
                    },

                    new AdaptiveText
                    {
                        Text = DisplayStepList.Any() ? "Unsolved" : "Primes",
                        HintStyle = AdaptiveTextStyle.Caption
                    },

                    new AdaptiveText
                    {
                        Text = DisplayStepList.Any()
                            ? DisplayStepList.Count.ToString()
                            : DisplayPrimes.Count.ToString(),
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                }
            };

            TileContent content = new TileContent
            {
                Visual = new TileVisual
                {
                    TileSmall = new TileBinding
                    {
                        Content = generalNotification
                    },
                    TileMedium = new TileBinding
                    {
                        Content = generalNotification
                    },
                    TileWide = new TileBinding
                    {
                        Content = generalNotification
                    },
                    TileLarge = new TileBinding
                    {
                        Content = generalNotification
                    }
                }
            };
            var notification = new TileNotification(content.GetXml())
            {
                ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(1),
            };
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            if (SecondaryTile.Exists("MySecondaryTile"))
            {
                // Get its updater
                var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile("MySecondaryTile");

                // And send the notification
                updater.Update(notification);
            }
        }


        private void BtnCopyStep_OnClick(object sender, RoutedEventArgs e)
        {
            var outStep = new DataPackage();
            outStep.SetText(string.Join("\r\n", DisplayStepList));
            Clipboard.SetContent(outStep);
        }

        private void BtnCopyPrimes_OnClick(object sender, RoutedEventArgs e)
        {
            var outPrimes = new DataPackage();
            outPrimes.SetText(string.Join("\r\n", DisplayPrimes));
            Clipboard.SetContent(outPrimes);
        }

        private void BtnExportStep_OnClick(object sender, RoutedEventArgs e)
        {
            ExportCollection(DisplayStepList);
        }

        private void BtnExportPrimes_OnClick(object sender, RoutedEventArgs e)
        {
            ExportCollection(DisplayPrimes);
        }

        private async void ExportCollection(ObservableCollection<long> exportList)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };

            savePicker.FileTypeChoices.Add("Plain Text File", new List<string> { ".txt" });

            savePicker.SuggestedFileName = string.Format("Primes - {0} to {1}", MinInput, MaxInput);
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);

                await Windows.Storage.FileIO.WriteTextAsync(file, string.Join("\r\n", exportList));

                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
    }
}
