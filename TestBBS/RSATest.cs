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

		const int p = 31;
		const int q = 19;

		public RSATest() 
		{
			(PublicKey publicKey, PrivateKey privateKey) = RSA.RSA.GenerateKeys(p, q);
			this.publicKey = publicKey;
			this.privateKey = privateKey;

            //this.publicKey = new PublicKey { e = 7, n = 589};
            //this.privateKey = new PrivateKey { d = 463, n = 589};

        }

		[TestMethod]
		public void EncryptDecrypt()
		{
			var message = "This is random message";

			var message_bytes = ASCIIEncoding.ASCII.GetBytes(message);

			var encrypted = new BigInteger[message.Length];
            var decrypted = new byte[message.Length];

			//var encrypted_ = RSA.RSA.Encrypt(message_bytes, publicKey);
			//var decrypted_ = RSA.RSA.Decrypt(encrypted_, privateKey);

			for (int i = 0; i < encrypted.Length; i++)
			{
                encrypted[i] = RSA.RSA.EncryptByte(message_bytes[i], publicKey);

            }
            
            for (int i = 0; i < encrypted.Length; i++)
            {
                decrypted[i] = RSA.RSA.DecryptByte(encrypted[i], privateKey);
            }

			var message_decrypted = ASCIIEncoding.ASCII.GetString(decrypted);
			
            Assert.AreEqual(message, message_decrypted);
		}

        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
