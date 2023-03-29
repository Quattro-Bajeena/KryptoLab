using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryptoLab1BBS
{
    public class BBS
    {
        long N = 0;
        long x = 0;
        Random rnd = new Random();
        const long MaxPrime = 100_000;
        const long MinPrime = 100;

        public BBS()
        {
            var sieve = Sieve(MaxPrime);
            var primes = GetFittingPrimes(sieve, MinPrime, MaxPrime);


            var p = primes[rnd.Next(primes.Count)];
            var q = primes[rnd.Next(primes.Count)];
            
            N = p * q;

            while (GCD(x, N) != 1)
            {
                x = rnd.Next();
            }
        }

        private List<long> GetFittingPrimes(bool[] sieve, long min, long max)
        {
            var primes = new List<long>();
            for(long i = min; i < max; i++)
            {
                if (sieve[i] == true && i%4 == 3)
                {
                    primes.Add(i);
                }
            }
            return primes;
        }

        private bool[] Sieve(long max)
        {
            // Create an array of boolean values indicating whether a number is prime.
            // Start by assuming all numbers are prime by setting them to true.
            bool[] primes = new bool[max + 1];
            for (long i = 0; i < primes.Length; i++)
            {
                primes[i] = true;
            }

            // Loop through a portion of the array (up to the square root of MAX). If
            // it's a prime, ensure all multiples of it are set to false, as they
            // clearly cannot be prime.
            for (long i = 2; i < Math.Sqrt(max) + 1; i++)
            {
                if (primes[i])
                {
                    for (long j = (long)Math.Pow(i, 2); j <= max; j += i)
                    {
                        primes[j] = false;
                    }
                }
            }
            return primes;
        }

        private long GCD(long m, long n)
        {
            var tmp = 0L;
            if (m < n)
            {
                tmp = m;
                m = n;
                n = tmp;
            }
            while (n != 0)
            {
                tmp = m % n;
                m = n;
                n = tmp;
            }
            return m;
        }


        public List<int> GenerateBits(int length)
        {
            var numbers = new List<long>(length);
            var bits = new List<int>(length);

            var x0 = ((long)Math.Pow(x, 2)) % N;
            numbers.Add(x0);
            bits.Add( (int)(x0 & 1) );


            for (int i = 0; i < length-1; i++)
            {
                var xi1 = ((long)Math.Pow(numbers[i], 2)) % N;
                numbers.Add(xi1);
                bits.Add( (int)(xi1 & 1) );
            }
            
            return bits;
        }
    }
}
