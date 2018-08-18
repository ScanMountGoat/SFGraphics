using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects
{
    public sealed partial class Framebuffer
    {
        /// <summary>
        /// Reads the framebuffer's contents into a Bitmap.
        /// </summary>
        /// <param name="saveAlpha">The alpha channel is preserved when true or set to 255 (white when false</param>
        /// <returns>A bitmap of the framebuffer's contents</returns>
        public Bitmap ReadImagePixels(bool saveAlpha = false)
        {
            int componentCount = 4; // RGBA
            int pixelSizeInBytes = sizeof(byte) * componentCount;
            int imageSizeInBytes = width * height * pixelSizeInBytes;

            Bind();

            byte[] pixels = GetBitmapPixels(saveAlpha, pixelSizeInBytes, imageSizeInBytes);

            Bitmap bmp = CreateBitmap(pixels);
            return bmp;
        }

        /// <summary>
        /// The origin (0,0) corresponds to the top left of the screen.
        /// The coordinates are based on the framebuffer's
        /// <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        /// <param name="x">The horizontal pixel coordinate</param>
        /// <param name="y">The vertical pixel coordinate</param>
        /// <returns>A color with the RGBA values of the selected pixel</returns>
        public Color SamplePixelColor(int x, int y)
        {
            Bind();

            byte[] rgba = new byte[4];
            GL.ReadPixels(x, y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, rgba);

            return Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }

        private byte[] GetBitmapPixels(bool saveAlpha, int pixelSizeInBytes, int imageSizeInBytes)
        {
            byte[] pixels = ReadPixels(imageSizeInBytes);
            pixels = CopyPixelsFlipAdjustAlpha(width, height, saveAlpha, pixelSizeInBytes, pixels);
            return pixels;
        }

        private Bitmap CreateBitmap(byte[] imageData)
        {
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);

            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private byte[] ReadPixels(int imageSizeInBytes)
        {
            // Read the pixels from the framebuffer. PNG uses the BGRA format. 
            byte[] pixels = new byte[imageSizeInBytes];
            GL.ReadPixels(0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            return pixels;
        }

        private static byte[] CopyPixelsFlipAdjustAlpha(int width, int height, bool saveAlpha, int pixelSizeInBytes, byte[] pixels)
        {
            // Flip data because glReadPixels reads it in from bottom row to top row
            byte[] fixedPixels = new byte[pixels.Length];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int sourceIndex = w + (h * width);
                    int sourceByteIndex = sourceIndex * pixelSizeInBytes;

                    int destinationIndex = ((height - h - 1) * width) + w;
                    int destinationByteIndex = destinationIndex * pixelSizeInBytes;

                    if (!saveAlpha)
                        pixels[sourceByteIndex + 3] = 255;

                    // Copy each pixel one at a time.
                    Array.Copy(pixels, sourceByteIndex, fixedPixels, destinationByteIndex, pixelSizeInBytes);
                }
            }

            return fixedPixels;
        }
    }
}
