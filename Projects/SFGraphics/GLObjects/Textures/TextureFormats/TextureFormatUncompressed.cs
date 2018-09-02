using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures.TextureFormats
{
    /// <summary>
    /// Stores uncompressed texture format information.
    /// </summary>
    public struct TextureFormatUncompressed
    {
        /// <summary>
        /// The format used by OpenGL to store the data
        /// </summary>
        public readonly PixelInternalFormat pixelInternalFormat;

        /// <summary>
        /// The format of the color components for the input data.
        /// </summary>
        public readonly PixelFormat pixelFormat;

        /// <summary>
        /// The data type of each color component for the input data
        /// </summary>
        public readonly PixelType pixelType; 

        /// <summary>
        /// Initializes the format information for an uncompressed format.
        /// </summary>
        /// <param name="pixelInternalFormat"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="pixelType"></param>
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
