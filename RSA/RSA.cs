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

		public static Tuple<PublicKey, PrivateKey> GenerateKeys(int p, int q, int bits=32) {
			var n = p * q;
			var phi = (p-1) * (q-1);

			var e = BigPrimeGenerator.RandomPrime();
			while(Formulas.GCD(phi, e) != 1)
			{
				e = BigPrimeGenerator.RandomPrime();
			}

			var d = random.Next();
			while ( ( ( (long)e * d - 1) % phi) != 0)
			{
				d = random.Next();
			}

			return new ( 
				new PublicKey() { e = e, n = n}, 
				new PrivateKey() {d = d, n = n } 
			);

		}

		public static BigInteger Encrypt(byte[] message, PublicKey pubKey)
		{
			var messageRaw = new BigInteger(message);
			var encrypted = BigInteger.ModPow(messageRaw, pubKey.e, pubKey.n);
			
			return encrypted;
		}

        public static BigInteger EncryptByte(byte message, PublicKey pubKey)
        {
            var encrypted = BigInteger.ModPow(message, pubKey.e, pubKey.n);
            return encrypted;
        }

        public static byte[] Decrypt(BigInteger encryptedMessage, PrivateKey privKey)
		{
			var decryptedRaw = BigInteger.ModPow(encryptedMessage, privKey.d, privKey.n);
			var decryptedBytes = decryptedRaw.ToByteArray();
            return decryptedBytes;
		}

        public static byte DecryptByte(BigInteger encryptedMessage, PrivateKey privKey)
        {
            var decryptedRaw = BigInteger.ModPow(encryptedMessage, privKey.d, privKey.n);
            return (byte)decryptedRaw;
        }




    }
}
