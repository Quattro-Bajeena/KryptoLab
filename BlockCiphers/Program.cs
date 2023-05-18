using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle;
using Org.BouncyCastle.Utilities;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BlockCiphers
{
	internal class Program
	{
		public static void Main()
		{

			CheckCBC();


			//Console.WriteLine("Time Measuring");
			//Console.WriteLine("------ECB------");
			//MeasureCiptherTime(CipherMode.ECB);
			//Console.WriteLine("\n------CBC------");
			//MeasureCiptherTime(CipherMode.CBC);
			//Console.WriteLine("\n------CFB------");
			//MeasureCiptherTime(CipherMode.CFB);



			//Console.WriteLine("\n\nError propagation");
			//Console.WriteLine("------ECB------");
			//ErrorPropagation(CipherMode.ECB);
			//Console.WriteLine("\n------CBC------");
			//ErrorPropagation(CipherMode.CBC);
			//Console.WriteLine("\n------CFB------");
			//ErrorPropagation(CipherMode.CFB);

		}

		static void CheckCBC()
		{
			var original = "text I want to cipther and do other things";
			using var myAes = Aes.Create();

			CBC(original, myAes.Key, myAes.IV);
		}

		static void CBC(string plainText, byte[] Key, byte[] IV)
		{
			using var myAes = Aes.Create();
			myAes.Key = Key;
			myAes.IV = IV;
			myAes.Mode = CipherMode.ECB;

			
			var plainTextBytes = Encoding.ASCII.GetBytes(plainText);
			var blockSizeBytes = (int)(myAes.BlockSize / 8);


			var originalBytes = new byte[ (plainTextBytes.Length/ blockSizeBytes + 1) * blockSizeBytes];
			plainTextBytes.CopyTo(originalBytes, 0);

			var output = new List<byte>();

			var originalBlocks = new byte[(plainTextBytes.Length / blockSizeBytes + 1)][];
			for(var i = 0; i < originalBlocks.Length; i++)
			{
				originalBlocks[i] = new byte[blockSizeBytes];
				Array.Copy(originalBytes, i * blockSizeBytes, originalBlocks[i], 0, blockSizeBytes);
			}


			var xored = exclusiveOR(originalBlocks[0], IV);
			var outputBlock = EncryptStringToBytes_Aes(xored, Key, IV, CipherMode.ECB);

			// Encryping 16 bytes returns 32byte I don't know why?



		}

		public static byte[] exclusiveOR(byte[] arr1, byte[] arr2)
		{
			if (arr1.Length != arr2.Length)
				throw new ArgumentException("arr1 and arr2 are not the same length");

			byte[] result = new byte[arr1.Length];

			for (int i = 0; i < arr1.Length; ++i)
				result[i] = (byte)(arr1[i] ^ arr2[i]);

			return result;
		}

		static void MeasureCiptherTime(CipherMode mode)
		{

			var filename = "text to encrypt.txt";
			var original = File.ReadAllText(filename);

			// Create a new instance of the Aes
			// class.  This generates a new key and initialization
			// vector (IV).
			using var myAes = Aes.Create();

			var sw = new Stopwatch();
			sw.Start();
			// Encrypt the string to an array of bytes.
			byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV, mode);

			var encryptTime = sw.Elapsed;

			sw.Restart();
			// Decrypt the bytes to a string.
			string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV, mode);

			var decryptTime = sw.Elapsed;

			//Display the original data and the decrypted data.
			Console.WriteLine("original == roundtrip:   {0}\n", original == roundtrip);
			Console.WriteLine("Encryption time {0} ms", encryptTime.TotalMilliseconds);
			Console.WriteLine("Decryption time {0} ms", decryptTime.TotalMilliseconds);
			Console.WriteLine("Total time {0} ms", (encryptTime + decryptTime).TotalMilliseconds);
		}

		static void ErrorPropagation(CipherMode mode)
		{
			var original = new string('a', 16) + new string('b', 16);
			using var myAes = Aes.Create();

			// Encrypt the string to an array of bytes.
			byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV, mode);

			encrypted[0] ^= 1;

			string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV, mode);

			//Display the original data and the decrypted data.
			Console.WriteLine("Original:   {0}", original);
			Console.WriteLine("Round Trip: {0}", roundtrip);
		}

		static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV, CipherMode mode)
		{
			// Check arguments.
			if (plainText == null || plainText.Length <= 0)
				throw new ArgumentNullException("plainText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");

			byte[] encrypted = null;

			// Create an Aes object
			// with the specified key and IV.
			using (var aesAlg = Aes.Create())
			{
				aesAlg.Key = Key;
				aesAlg.IV = IV;
				aesAlg.Mode = mode;

				// Create an encryptor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for encryption.
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
					}
					encrypted = msEncrypt.ToArray();
				}	
			}

			// Return the encrypted bytes from the memory stream.
			return encrypted;
		}

		static byte[] EncryptStringToBytes_Aes(byte[] plainText, byte[] Key, byte[] IV, CipherMode mode)
		{
			//Check arguments.
			if (plainText == null || plainText.Length <= 0)
				throw new ArgumentNullException("plainText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");

			byte[] encrypted = null;

			// Create an Aes object
			// with the specified key and IV.
			using (var aesAlg = Aes.Create())
			{
				aesAlg.Key = Key;
				aesAlg.IV = IV;
				aesAlg.Mode = mode;

				// Create an encryptor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for encryption.
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						csEncrypt.Write(plainText);
					}
					encrypted = msEncrypt.ToArray();
				}
			}

			// Return the encrypted bytes from the memory stream.
			return encrypted;
		}

		static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV, CipherMode mode)
		{
			// Check arguments.
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			// Create an Aes object
			// with the specified key and IV.
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Key;
				aesAlg.IV = IV;
				aesAlg.Mode = mode;

				// Create a decryptor to perform the stream transform.
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for decryption.
				using (MemoryStream msDecrypt = new MemoryStream(cipherText))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{

							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							plaintext = srDecrypt.ReadToEnd();
						}
					}
				}
			}

			return plaintext;
		}
	}

	
}