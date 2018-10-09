using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Contains methods for working with <see cref="Bitmap"/> images.
    /// </summary>
    public static class BitmapUtils
    {
        /// <summary>
        /// Creates a <see cref="Bitmap"/> from ABGR pixel data.
        /// </summary>
        /// <param name="width">The width in pixels of the image data</param>
        /// <param name="height">The height in pixels of the image data</param>
        /// <param name="imageData">ABGR image pixels</param>
        /// <returns>A new image with the given image data</returns>
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
