using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Steganography
{
	[System.Runtime.Versioning.SupportedOSPlatform("windows")]
	internal class Program
	{
		static void Main(string[] args)
		{
			
			var secretMessage = "Under 149 centimeters";
			var messageBytes = Encoding.ASCII.GetBytes(secretMessage);
			var encodedFileName = EncodeMessage("arisu.png", messageBytes);
			var decodedBytes = DecodeMessage(encodedFileName, 100);
			var decodedMessage = Encoding.ASCII.GetString(decodedBytes);

			Console.WriteLine("Secret message:" + secretMessage);
			Console.WriteLine("Decoded message:" + decodedMessage);
		}

		static byte[] DecodeMessage(string imageName, int? messageLength = null)
		{
			var image = new Bitmap(imageName);
			var message = new List<byte>();

			for (int x = 0; x < image.Width; x++)
			{
				for (int y = 0; y < image.Height; y += 2)
				{
					if(messageLength == null || message.Count < messageLength)
					{
						var pixel1 = image.GetPixel(x, y);
						var pixel2 = image.GetPixel(x, y + 1);

						var r1 = pixel1.R;
						var g1 = pixel1.G;
						var b1 = pixel1.B;
						var a1 = pixel1.A;

						var r2 = pixel2.R;
						var g2 = pixel2.G;
						var b2 = pixel2.B;
						var a2 = pixel2.A;

						var bit0 = r1 & 1;
						var bit1 = g1 & 1;
						var bit2 = b1 & 1;
						var bit3 = a1 & 1;
						var bit4 = r2 & 1;
						var bit5 = g2 & 1;
						var bit6 = b2 & 1;
						var bit7 = a2 & 1;

						var decoded = 0;

						decoded ^= ((-bit0 ^ decoded) & (1 << 0));
						decoded ^= ((-bit1 ^ decoded) & (1 << 1));
						decoded ^= ((-bit2 ^ decoded) & (1 << 2));
						decoded ^= ((-bit3 ^ decoded) & (1 << 3));
						decoded ^= ((-bit4 ^ decoded) & (1 << 4));
						decoded ^= ((-bit5 ^ decoded) & (1 << 5));
						decoded ^= ((-bit6 ^ decoded) & (1 << 6));
						decoded ^= ((-bit7 ^ decoded) & (1 << 7));

						var decodedByte = (byte)decoded;
						message.Add(decodedByte);
					}

				}

			}
			return message.ToArray();

		}

		static string EncodeMessage(string imageName, byte[] secretMessage)
		{
			var image = new Bitmap(imageName);
			var imageWithSecret = new Bitmap(image);
			
			var bitsEncoded = 0;

			for (int x = 0; x < image.Width; x++)
			{
				for (int y = 0; y < image.Height; y++)
				{
					var color = image.GetPixel(x, y);
					if (bitsEncoded / 8 < secretMessage.Length && bitsEncoded/8 < image.Width* image.Height*4)
					{

						var r = color.R;
						var g = color.G;
						var b = color.B;
						var a = color.A;

						var encodedByte = secretMessage[bitsEncoded / 8];
						var bitInByteIndex = bitsEncoded % 8;


						var bit1 = (encodedByte & (1 << bitInByteIndex)) >> bitInByteIndex;
						var bit2 = (encodedByte & (1 << bitInByteIndex + 1)) >> (bitInByteIndex+1);
						var bit3 = (encodedByte & (1 << bitInByteIndex + 2)) >> (bitInByteIndex+2);
						var bit4 = (encodedByte & (1 << bitInByteIndex + 3)) >> (bitInByteIndex+3);


						var rModified = (byte)(r ^ ((-bit1 ^ r) & 1));
						var gModified = (byte)(g ^ ((-bit2 ^ g) & 1));
						var bModified = (byte)(b ^ ((-bit3 ^ b) & 1));
						var aModified = (byte)(a ^ ((-bit4 ^ a) & 1));

						var newColor = Color.FromArgb(aModified, rModified, gModified, bModified);

						imageWithSecret.SetPixel(x, y, newColor);

						bitsEncoded += 4;
					}


				}
			}
			var fileName = "encoded " + imageName;
			imageWithSecret.Save(fileName, ImageFormat.Png);
			return fileName;
		}
	}
}