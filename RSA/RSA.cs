using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{

	public struct PublicKey
	{
		public int e { set; get; }
		public int n { set; get; }
	}

	public struct PrivateKey
	{
		public int d { set; get; }
		public int n { set; get; }
	}


	public static class RSA
	{
		static Random random = new Random();

		public static Tuple<PublicKey, PrivateKey> GenerateKeys(int p, int q, int bits) {
			var n = p * q;
			var phi = (p-1) * (q-1);

			var e = BigPrimeGenerator.RandomPrime();
			while(Formulas.GCD(phi, e) != 1)
			{
				e = BigPrimeGenerator.RandomPrime();
			}

			var d = random.Next();
			while ( ( (e * d - 1) % phi) != 0)
			{
				d = random.Next();
			}

			return new ( 
				new PublicKey() { e = e, n = n}, 
				new PrivateKey() {d = d, n = n } 
			);

		}

		public static byte[] Encrypt(string message, PublicKey pubKey)
		{
			var messageRaw = new BigInteger(Encoding.UTF8.GetBytes(message));
			var encrypted = BigInteger.ModPow(messageRaw, pubKey.e, pubKey.n);
			return encrypted.ToByteArray();
		}

		public static string Decrypt(byte[] encryptedMessage, PrivateKey privKey)
		{
			var encryptedMessageRaw = new BigInteger(encryptedMessage);
			var decryptedRaw = BigInteger.ModPow(encryptedMessageRaw, privKey.d, privKey.n);
			return Encoding.UTF8.GetString(decryptedRaw.ToByteArray());
		}




	}
}
