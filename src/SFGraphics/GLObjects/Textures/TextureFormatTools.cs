using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Calculates the number of bytes of image data for loading compressed texture data with OpenTK.
    /// </summary>
    public static class TextureFormatTools
    {
        /// <summary>
        /// Calculates the imageSize parameter for GL.CompressedTexImage. 
        /// Supports the variants of InternalFormat for DXT1, DXT3, DXT5.
        /// The imageSize should be recalculated for each mip level when reading mipmaps from existing image data.
        /// </summary>
        /// <param name="width">The width of the mip level in pixels</param>
        /// <param name="height">The height of the mip level in pixels</param>
        /// <param name="internalFormat">The <paramref name="internalFormat"/> should be a compressed format.</param>
        /// <returns></returns>
        public static int CalculateImageSize(int width, int height, InternalFormat internalFormat)
        {
            int blockSize = 16; // DXT3/DXT5

            // Ignore the parts of the enum we don't care about.
            if (internalFormat.ToString().ToLower().Contains("dxt1"))
                blockSize = 8;

            int imageSize = blockSize * (int)Math.Ceiling(width / 4.0) * (int)Math.Ceiling(height / 4.0);
            return imageSize;
        }

        /// <summary>
        /// Determines whether a format is compressed.
        /// Compressed formats should use GL.CompressedTexImage instead of GL.TexImage.
        /// </summary>
        /// <param name="internalFormat">The image format for the texture data</param>
        /// <returns>True if the format is compressed</returns>
        public static bool IsCompressed(InternalFormat internalFormat)
        {
            // All the enum value names should follow this convention.
            return internalFormat.ToString().ToLower().Contains("compressed");
        }
    }
}
