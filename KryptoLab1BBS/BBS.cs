using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathUtils;

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
            var sieve = Formulas.Sieve(MaxPrime);
            var primes = GetFittingPrimes(sieve, MinPrime, MaxPrime);


            var p = primes[rnd.Next(primes.Count)];
            var q = primes[rnd.Next(primes.Count)];
            
            N = p * q;

            while (Formulas.GCD(x, N) != 1)
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
