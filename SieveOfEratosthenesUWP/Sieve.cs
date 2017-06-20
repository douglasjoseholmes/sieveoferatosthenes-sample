using System.Collections.Generic;
using System.Linq;

namespace SieveOfEratosthenesUWP
{


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
