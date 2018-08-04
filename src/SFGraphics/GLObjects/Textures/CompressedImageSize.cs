using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Calculates the number of bytes of image data for loading compressed texture data with OpenTK.
    /// </summary>
    public static class CompressedImageSize
    {
        /// <summary>
        /// Calculates the imageSize parameter for GL.CompressedTexImage.
        /// The imageSize should be recalculated for each mip level when reading mipmaps from existing image data.
        /// </summary>
        /// <param name="width">The width of the mip level in pixels</param>
        /// <param name="height">The height of the mip level in pixels</param>
        /// <param name="internalFormat">The <paramref name="internalFormat"/> should be a compressed format.</param>
        /// <returns></returns>
        public static int CalculateImageSize(int width, int height, InternalFormat internalFormat)
        {
            int blockSize = 16; // DXT3/DXT5
            if (internalFormat == InternalFormat.CompressedRgbaS3tcDxt1Ext)
                blockSize = 8;

            int imageSize = blockSize * (int)Math.Ceiling(width / 4.0) * (int)Math.Ceiling(height / 4.0);
            return imageSize;
        }
    }
}
