using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// An immutable cotainer for storing format information for uncompressed texture image data.
    /// </summary>
    public struct TextureFormatUncompressed
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly PixelInternalFormat pixelInternalFormat;

        /// <summary>
        /// 
        /// </summary>
        public readonly PixelFormat pixelFormat;

        /// <summary>
        /// An immutable container for storing 
        /// </summary>
        public readonly PixelType pixelType; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelInternalFormat"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="pixelType"></param>
        /// <exception cref="ArgumentException"><paramref name="pixelInternalFormat"/> is compressed</exception>
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
