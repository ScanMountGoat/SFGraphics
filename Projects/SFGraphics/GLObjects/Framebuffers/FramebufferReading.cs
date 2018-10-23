using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Framebuffers
{
    public sealed partial class Framebuffer
    {
        /// <summary>
        /// Reads the framebuffer's contents into a Bitmap.
        /// </summary>
        /// <param name="saveAlpha">The alpha channel is preserved when true or set to 255 (white when false</param>
        /// <returns>A bitmap of the framebuffer's contents</returns>
        public System.Drawing.Bitmap ReadImagePixels(bool saveAlpha = false)
        {
            // RGBA unsigned byte
            int pixelSizeInBytes = sizeof(byte) * 4;
            int imageSizeInBytes = Width * Height * pixelSizeInBytes;

            byte[] pixels = GetBitmapPixels(saveAlpha, pixelSizeInBytes, imageSizeInBytes);

            var bitmap = Utils.BitmapUtils.GetBitmap(Width, Height, pixels);
            // Adjust for differences in the origin point.
            bitmap.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }

        /// <summary>
        /// The origin (0,0) corresponds to the top left of the screen.
        /// The coordinates are based on the framebuffer's
        /// <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        /// <param name="x">The horizontal pixel coordinate</param>
        /// <param name="y">The vertical pixel coordinate</param>
        /// <returns>A color with the RGBA values of the selected pixel</returns>
        public System.Drawing.Color SamplePixelColor(int x, int y)
        {
            Bind();

            byte[] rgba = new byte[4];
            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, rgba);

            return System.Drawing.Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }

        private byte[] GetBitmapPixels(bool saveAlpha, int pixelSizeInBytes, int imageSizeInBytes)
        {
            byte[] pixels = ReadPixels(imageSizeInBytes);
            if (!saveAlpha)
                SetAlphaToWhite(Width, Height, pixelSizeInBytes, pixels);
            return pixels;
        }

        private byte[] ReadPixels(int imageSizeInBytes)
        {
            Bind();
            byte[] pixels = new byte[imageSizeInBytes];

            // Read the pixels from the framebuffer. PNG uses the BGRA format. 
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            return pixels;
        }

        private static void SetAlphaToWhite(int width, int height, int pixelSizeInBytes, byte[] pixels)
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int pixelIndex = w + (h * width);
                    pixels[pixelIndex * pixelSizeInBytes + 3] = 255;
                }
            }
        }
    }
}
