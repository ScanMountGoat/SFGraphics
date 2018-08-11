using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;


namespace SFGraphics.GLObjects
{
    public sealed partial class Framebuffer
    {
        /// <summary>
        /// Reads the framebuffer's contents into a Bitmap using GL.ReadPixels. 
        /// This is intended for screenshots, so it only works properly for framebuffers of type 
        /// PixelFormat.Rgba.
        /// </summary>
        /// <param name="saveAlpha">The alpha channel is saved when true or set to 255 (white) when false</param>
        /// <returns></returns>
        public Bitmap ReadImagePixels(bool saveAlpha = false)
        {
            // Calculate the number of bytes needed.
            int pixelByteLength = width * height * sizeof(float);
            byte[] pixels = new byte[pixelByteLength];

            // Read the pixels from the framebuffer. PNG uses the BGRA format. 
            // This probably won't work for HDR textures.
            Bind();
            GL.ReadPixels(0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            byte[] fixedPixels = CopyImagePixels(width, height, saveAlpha, pixelByteLength, pixels);

            // Format and save the data
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(fixedPixels, 0, bmpData.Scan0, fixedPixels.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// The origin (0,0) corresponds to the top left of the screen.
        /// The coordinates are based on the framebuffer's dimensions 
        /// and not the screen's dimensions.
        /// </summary>
        /// <param name="x">The horizontal pixel coordinate</param>
        /// <param name="y">The vertical pixel coordinate</param>
        /// <returns>A color with the RGBA values of the selected pixel</returns>
        public Color SamplePixelColor(int x, int y)
        {
            Bind();
            // Only RGBA is supported for now.
            byte[] rgba = new byte[4];
            GL.ReadPixels(x, y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, rgba);
            return Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }

        private static byte[] CopyImagePixels(int width, int height, bool saveAlpha, int pixelByteLength, byte[] pixels)
        {
            // Flip data because glReadPixels reads it in from bottom row to top row
            byte[] fixedPixels = new byte[pixelByteLength];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    // Remove alpha blending from the end image - we just want the post-render colors
                    if (!saveAlpha)
                        pixels[((w + h * width) * sizeof(float)) + 3] = 255;

                    // Copy a 4 byte pixel one at a time
                    Array.Copy(pixels, (w + h * width) * sizeof(float), fixedPixels, ((height - h - 1) * width + w) * sizeof(float), sizeof(float));
                }
            }

            return fixedPixels;
        }
    }
}
