using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            testBlock.Text = string.Join("|", new Sieve(1000).Solve().ToArray());
        }

        public class Sieve
        {
            public int Minimum { get; set; }
            public int Maximum { get; set; }
            
            public Sieve(int max)
            {
                Minimum = 0;
                Maximum = max;
            }

            public List<string> Solve()
            {
                HashSet<int> composite = new HashSet<int>();
                var primes = new List<string>();
                long count = 0;
                for (int x = 2; x < Maximum; x++)
                {
                    for (int y = x * 2; y < Maximum; y = y + x)
                    {

                        if (!composite.Contains(y))
                        {
                            composite.Add(y);
                        }

                    }

                }

                for (int z = 2; z < Maximum; z++)
                {
                    if (!composite.Contains(z))
                    {
                        primes.Add(z.ToString());
                        count = count + z;
                    }
                }

                Console.WriteLine("Sum is: " + count);
                return primes;
            }
        }
    }
}
