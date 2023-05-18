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
            Console.WriteLine("Hello, World!");
        }

        static void GenerateShares()
        {
            var original = new Bitmap("original.png");

            var share1 = new Bitmap(200, 100);
            var share2 = new Bitmap(200, 100);

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
                    b[count] = (byte)(rgbValues[(column * stride) + (row * 3)]);
                    g[count] = (byte)(rgbValues[(column * stride) + (row * 3) + 1]);
                    r[count++] = (byte)(rgbValues[(column * stride) + (row * 3) + 2]);
                }
            }


            // save image to file or stream
            share1.Save("edited.phg", ImageFormat.Png);
            share2.Save("edited.phg", ImageFormat.Png);
        }
    }
}