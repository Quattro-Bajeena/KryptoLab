using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace MathUtils
{


	public static class BigPrimeGenerator
	{

		static RandomNumberGenerator rng = RandomNumberGenerator.Create();
		static Random random = new Random();


		public static BigInteger RandomPrimeNBit(int n)
		{
			var num = NBitRandomInt(n);
			while (SmallPrimeTest(num) == false || IsMillerRabinPassed((int)num) == false)
			{
				num = NBitRandomInt(n);
			}
			return num;
		}

		public static int RandomPrime()
		{
			var num = random.Next();
			while (IsMillerRabinPassed(num) == false)
			{
				num = random.Next();
			}
			return num;
		}

		static bool SmallPrimeTest(BigInteger number)
		{
			foreach(var prime in SmallPrimes.LotOfPrimes)
			{
				if (number % prime == 0)
					return false;
			}
			return true;
		}

		static bool SmallPrimeTest(int number)
		{
			foreach (var prime in SmallPrimes.BitOfPrimes)
			{
				if (number % prime == 0)
					return false;
			}
			return true;
		}

		public static BigInteger NBitRandomInt(int n)
		{
			byte[] bytes = new byte[n / 8];
			rng.GetBytes(bytes);
			bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
			return new BigInteger(bytes);
		}

		static int ExpMod(int number, int exp, int mod)
		{
			if (exp == 0) return 1;
			if (exp % 2 == 0)
			{
				return (int)Math.Pow(ExpMod(number, (exp / 2), mod), 2) % mod;
			}
			else
			{
				return (number * ExpMod(number, (exp - 1), mod)) % mod;
			}
		}

		static bool TrialComposite(int round_tester, int evenComponent,
								   int miller_rabin_candidate, int maxDivisionsByTwo)
		{
			if (ExpMod(round_tester, evenComponent, miller_rabin_candidate) == 1)
				return false;
			for (int i = 0; i < maxDivisionsByTwo; i++)
			{
				if (ExpMod(round_tester, (1 << i) * evenComponent,
						   miller_rabin_candidate) == miller_rabin_candidate - 1)
					return false;
			}
			return true;
		}

		static bool IsMillerRabinPassed(int miller_rabin_candidate)
		{
			// Run 20 iterations of Rabin Miller Primality test

			var maxDivisionsByTwo = 0;
			var evenComponent = miller_rabin_candidate - 1;

			while (evenComponent % 2 == 0)
			{
				evenComponent >>= 1;
				maxDivisionsByTwo += 1;
			}

			// Set number of trials here
			int numberOfRabinTrials = 200;
			for (int i = 0; i < (numberOfRabinTrials); i++)
			{
				var rand = new Random();
				var round_tester = random.Next(2, miller_rabin_candidate);

				if (TrialComposite(round_tester, evenComponent,
								   miller_rabin_candidate, maxDivisionsByTwo))
					return false;
			}
			return true;
		}

		static BigInteger RandomIntegerBelow(BigInteger N)
		{
			byte[] bytes = N.ToByteArray();
			BigInteger R;

			do
			{
				random.NextBytes(bytes);
				bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
				R = new BigInteger(bytes);
			} while (R >= N);

			return R;
		}

	}
}
