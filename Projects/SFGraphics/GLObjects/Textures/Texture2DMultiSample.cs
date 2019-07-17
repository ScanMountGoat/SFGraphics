using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A TextureTarget.Texture2DMultisample texture that does not support mipmaps.
    /// </summary>
    public class Texture2DMultisample : Texture
    {
        /// <summary>
        /// The number of texture samples.
        /// </summary>
        int Samples { get; }

        /// <summary>
        /// Creates a multisampled 2D texture of the given dimensions and format
        /// </summary>
        /// <param name="width">The width of the texture in pixels</param>
        /// <param name="height">The height of the texture in pixels</param>
        /// <param name="format">The format used to store the image data</param>
        /// <param name="samples">The number of texture samples</param>
        public Texture2DMultisample(int width, int height, PixelInternalFormat format, 
            int samples) : base(TextureTarget.Texture2DMultisample)
        {
            Samples = samples;
            if (Samples <= 0)
                throw new System.ArgumentOutOfRangeException(nameof(samples), "Sample count must be greater than 0");

            SetDimensionsAndFormat(width, height, format);
        }

        private void SetDimensionsAndFormat(int width, int height, PixelInternalFormat format)
        {
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Samples, format,
                width, height, true);
        }
    }
}
