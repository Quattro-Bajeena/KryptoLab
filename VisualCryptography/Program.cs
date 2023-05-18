using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VisualCryptography
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class Program
    {
        static void Main(string[] args)
        {
            GenerateShares();
        }

        static void GenerateShares()
        {
            var original = new Bitmap("original.png");

            var share1 = new Bitmap(original.Width * 2, original.Height);
            var share2 = new Bitmap(original.Width * 2, original.Height);
			var rand = new Random();



			for (int x = 0; x < original.Width; x++)
			{
				for (int y = 0; y < original.Height; y++)
				{
					var pixel = original.GetPixel(x, y);
					if(pixel.ToArgb() == Color.White.ToArgb())
					{
						if(rand.Next(2) == 0)
						{
							share1.SetPixel(2 * x, y, Color.Black);
							share1.SetPixel(2 * x + 1, y, Color.White);

							share2.SetPixel(2 * x, y, Color.Black);
							share2.SetPixel(2 * x + 1, y, Color.White);
						}
						else
						{
							share1.SetPixel(2 * x, y, Color.White);
							share1.SetPixel(2 * x + 1, y, Color.Black);

							share2.SetPixel(2 * x, y, Color.White);
							share2.SetPixel(2 * x + 1, y, Color.Black);
						}
					} 
					else if (pixel.ToArgb() == Color.Black.ToArgb())
					{

						if (rand.Next(2) == 0)
						{
							share1.SetPixel(2 * x, y, Color.Black);
							share1.SetPixel(2 * x + 1, y, Color.White);

							share2.SetPixel(2 * x, y, Color.White);
							share2.SetPixel(2 * x + 1, y, Color.Black);
						}
						else
						{
							share1.SetPixel(2 * x, y, Color.White);
							share1.SetPixel(2 * x + 1, y, Color.Black);

							share2.SetPixel(2 * x, y, Color.Black);
							share2.SetPixel(2 * x + 1, y, Color.White);
						}
					}

				}
			}


			// save image to file or stream
			share1.Save("share1.png", ImageFormat.Png);
            share2.Save("share2.png", ImageFormat.Png);
        }

		static void GenerateSharesFast()
        {

			var original = new Bitmap("original.png");

			var share1 = new Bitmap(original.Width * 2, original.Height);
			var share2 = new Bitmap(original.Width * 2, original.Height);

			// Lock the bitmap's bits.  
			Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
			BitmapData bmpData = original.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			// Get the address of the first line.
			IntPtr ptr = bmpData.Scan0;

			// Declare an array to hold the bytes of the bitmap.
			int bytes = bmpData.Stride * original.Height;
			byte[] rgbValues = new byte[bytes];
			byte[] r = new byte[bytes / 3];
			byte[] g = new byte[bytes / 3];
			byte[] b = new byte[bytes / 3];

			// Copy the RGB values into the array.
			Marshal.Copy(ptr, rgbValues, 0, bytes);

			int count = 0;
			int stride = bmpData.Stride;

			for (int column = 0; column < bmpData.Height; column++)
			{
				for (int row = 0; row < bmpData.Width; row++)
				{
					b[count] = rgbValues[(column * stride) + (row * 3)];
					g[count] = rgbValues[(column * stride) + (row * 3) + 1];
					r[count] = rgbValues[(column * stride) + (row * 3) + 2];



					count++;

				}
			}

			// save image to file or stream
			share1.Save("edited.png", ImageFormat.Png);
			share2.Save("edited.png", ImageFormat.Png);
		}

	}
}