using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using RSA;

namespace TestBBS
{
	[TestClass]
	public class RSATest
	{
		private static Random random = new();

		PublicKey publicKey;
		PrivateKey privateKey;

		const int p = 1423;
		const int q = 8641;
		const int keyLengthBits = 32;
		const int messageLength = 50;

		public RSATest() 
		{
			(PublicKey publicKey, PrivateKey privateKey) = RSA.RSA.GenerateKeys(p, q, keyLengthBits);
			this.publicKey = publicKey;
			this.privateKey = privateKey;

		}

		static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		[TestMethod]
		public void EncryptDecrypt()
		{
			var message = RandomString(messageLength);
			message = "ab";

			var encrypted = RSA.RSA.Encrypt(message, publicKey);
			var decrypted = RSA.RSA.Decrypt(encrypted, privateKey);
			Assert.AreEqual(message, decrypted);
		}
	}
}
