using System.Net.Sockets;
using System.Numerics;

namespace MathUtils
{
	public class Formulas
	{
		public static long GCD(long m, long n)
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

		public static BigInteger GCD(BigInteger m, BigInteger n)
		{
			BigInteger tmp = 0;
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

		public static bool[] Sieve(long max)
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

		public bool IsPrime(long number)
		{
			if(number % 2 == 0)
				return false;
			for(int i = 3; i < Math.Sqrt(number); i++)
			{
				if(number % i == 0) return false;
			}
			return true;
		}

		public int NoBits(BigInteger number)
		{
			int r = 0; // r will be lg(v)

			while (((number >>= 1) != 0)) // unroll for more speed...
			{
				r++;
			}
			return r;
		}
	}
}