using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

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
        public MainPage()
        {
            InitializeComponent();
            PageSieve = new Sieve();
            DisplayStepList = new ObservableCollection<long>();
            DisplayPrimes = new ObservableCollection<long>();
        }

        private void ResetList(object sender, EventArgs e)
        {
            PageSieve = new Sieve((long)TxtMin.Value.GetValueOrDefault(2), (long)TxtMax.Value.GetValueOrDefault(2));
            DisplayStepList = new ObservableCollection<long>(PageSieve.StepList);
            DisplayPrimes = new ObservableCollection<long>(PageSieve.Primes);
            StepBox.ItemsSource = PageSieve.StepList;
            PrimesBox.ItemsSource = DisplayPrimes;
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            DisplayStepList.Clear();
            DisplayPrimes.Clear();
            PageSieve.Solve();
            DisplayPrimes = new ObservableCollection<long>(PageSieve.Primes);
            PrimesBox.ItemsSource = DisplayPrimes;
        }

        private void BtnExportPrimes_OnClick(object sender, RoutedEventArgs e)
        {
            ExportCollection(DisplayPrimes);
        }

        private void BtnExportStep_OnClick(object sender, RoutedEventArgs e)
        {
            ExportCollection(DisplayStepList);
        }

        private async void ExportCollection(ObservableCollection<long> exportList)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Comma Separated Values", new List<string> { ".csv" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, string.Join(",", exportList));
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            PageSieve.Step();
            DisplayStepList = new ObservableCollection<long>(PageSieve.StepList);
            DisplayPrimes = new ObservableCollection<long>(PageSieve.Primes);
            StepBox.ItemsSource = DisplayStepList;
            PrimesBox.ItemsSource = DisplayPrimes;
        }

        public class Sieve
        {
            public long Minimum { get; set; }
            public long Maximum { get; set; }
            public HashSet<long> StepList { get; set; }
            public HashSet<long> Primes { get; set; }

            public Sieve()
            {

                Minimum = 2;
                Maximum = 2;
                StepList = new HashSet<long> { 2 };
                Primes = new HashSet<long>();
            }

            public Sieve(long max)
            {
                Minimum = 2;
                Maximum = max;
                StepList = new HashSet<long>();
                Primes = new HashSet<long>();
                SolveForBase();
            }

            public Sieve(long min, long max)
            {
                Minimum = min;
                Maximum = max;
                StepList = new HashSet<long>();
                Primes = new HashSet<long>();
                SolveForBase();
            }

            public void Step()
            {
                if (!StepList.Any())
                {
                    return;
                }
                var localMinimum = StepList.First();
                Primes.Add(localMinimum);
                StepList.Remove(localMinimum);
                for (long y = localMinimum * 2; y <= Maximum; y = y + localMinimum)
                {

                    if (StepList.Contains(y))
                    {
                        StepList.Remove(y);
                    }

                }
            }

            private void SolveForBase()
            {
                for (long x = 2; x <= Maximum; x++)
                {
                    StepList.Add(x);
                }
                for (long x = 2; x < Minimum; x++)
                {
                    StepList.Remove(x);
                    for (long y = x * 2; y <= Maximum; y = y + x)
                    {

                        if (StepList.Contains(y))
                        {
                            StepList.Remove(y);
                        }
                    }
                }
            }

            public void Solve()
            {
                Primes.Clear();
                HashSet<long> composite = new HashSet<long>();
                for (long x = 2; x <= Maximum; x++)
                {
                    for (long y = x * 2; y <= Maximum; y = y + x)
                    {

                        if (!composite.Contains(y))
                        {
                            composite.Add(y);
                        }

                    }

                }

                for (long z = Minimum; z <= Maximum; z++)
                {
                    if (!composite.Contains(z))
                    {
                        Primes.Add(z);
                    }
                }
            }
        }
    }
}
