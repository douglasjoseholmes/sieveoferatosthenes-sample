using System.Collections.Generic;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SieveOfEratosthenesUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            testBlock.Text = string.Join("|", new Sieve(int.Parse(txtMin.Text), int.Parse(txtMax.Text)).Solve().ToArray());
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

            public Sieve(int min, int max)
            {
                Minimum = min;
                Maximum = max;
            }

            public List<string> Solve()
            {
                HashSet<int> composite = new HashSet<int>();
                var primes = new List<string>();
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

                for (int z = Minimum; z < Maximum; z++)
                {
                    if (!composite.Contains(z))
                    {
                        primes.Add(z.ToString());
                    }
                }
                return primes;
            }
        }
    }
}
