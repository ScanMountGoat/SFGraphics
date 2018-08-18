using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures.TextureFormats
{
    /// <summary>
    /// An immutable container for uncompressed texture format information.
    /// </summary>
    public struct TextureFormatUncompressed
    {
        /// <summary>
        /// The color components of the image data
        /// </summary>
        public readonly PixelInternalFormat pixelInternalFormat;

        /// <summary>
        /// The format of the image data
        /// </summary>
        public readonly PixelFormat pixelFormat;

        /// <summary>
        /// The data type of the image data
        /// </summary>
        public readonly PixelType pixelType; 

        /// <summary>
        /// Initializes the format information for an uncompressed format.
        /// </summary>
        /// <param name="pixelInternalFormat">The color components of the image data</param>
        /// <param name="pixelFormat">The format of the image data</param>
        /// <param name="pixelType">The data type of the image data</param>
        /// <exception cref="ArgumentException"><paramref name="pixelInternalFormat"/> is a compressed format.</exception>
        public TextureFormatUncompressed(PixelInternalFormat pixelInternalFormat, PixelFormat pixelFormat, PixelType pixelType)
        {
            if (TextureFormatTools.IsCompressed(pixelInternalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedUncompressed);

            this.pixelInternalFormat = pixelInternalFormat;
            this.pixelFormat = pixelFormat;
            this.pixelType = pixelType;
        }
    }
}
