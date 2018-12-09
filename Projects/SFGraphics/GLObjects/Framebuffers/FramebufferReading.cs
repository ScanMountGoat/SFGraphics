using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Framebuffers
{
    public sealed partial class Framebuffer
    {
        /// <summary>
        /// Reads the default framebuffer's contents into a Bitmap. The default framebuffer is bound prior to reading.
        /// </summary>
        /// <param name="width">The width of the default framebuffer in pixels.</param>
        /// <param name="height">The height of the default framebuffer in pixels.</param>
        /// <param name="saveAlpha">The alpha channel is preserved when true or set to 255 (white when false</param>
        /// <returns>A bitmap of the framebuffer's contents</returns>
        public static System.Drawing.Bitmap ReadDefaultFramebufferImagePixels(int width, int height, bool saveAlpha = false)
        {
            // RGBA unsigned byte
            int pixelSizeInBytes = sizeof(byte) * 4;
            int imageSizeInBytes = width * height * pixelSizeInBytes;

            // TODO: Does the draw buffer need to be set?
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            byte[] pixels = GetBitmapPixels(width, height, pixelSizeInBytes, saveAlpha);

            var bitmap = Utils.BitmapUtils.GetBitmap(width, height, pixels);

            // Adjust for differences in the origin point.
            bitmap.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }
        
        /// <summary>
        /// Reads the framebuffer's contents into a Bitmap.
        /// </summary>
        /// <param name="saveAlpha">The alpha channel is preserved when true or set to 255 (white when false</param>
        /// <returns>A bitmap of the framebuffer's contents</returns>
        public System.Drawing.Bitmap ReadImagePixels(bool saveAlpha = false)
        {
            // RGBA unsigned byte
            int pixelSizeInBytes = sizeof(byte) * 4;

            Bind();
            byte[] pixels = GetBitmapPixels(Width, Height, pixelSizeInBytes, saveAlpha);

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

            // Convert RGBA to ARGB.
            return System.Drawing.Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }

        private static byte[] GetBitmapPixels(int width, int height, int pixelSizeInBytes, bool saveAlpha)
        {
            int imageSizeInBytes = width * height * pixelSizeInBytes;

            // Read the pixels from whatever framebuffer is currently bound.
            byte[] pixels = ReadPixels(width, height, imageSizeInBytes);

            if (!saveAlpha)
                SetAlphaToWhite(width, height, pixelSizeInBytes, pixels);
            return pixels;
        }

        private static byte[] ReadPixels(int width, int height, int imageSizeInBytes)
        {
            byte[] pixels = new byte[imageSizeInBytes];

            // Read the pixels from the framebuffer. PNG uses the BGRA format. 
            GL.ReadPixels(0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
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
