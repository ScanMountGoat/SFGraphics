using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SFGraphics.Utils
{
    public static class BitmapUtils
    {
        public static Bitmap GetBitmap(int width, int height, byte[] imageData)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
