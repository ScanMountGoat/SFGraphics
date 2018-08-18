using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Helpful tools for working with PixelInternalFormat and InternalFormat 
    /// with OpenTK's OpenGL texture functions.
    /// </summary>
    public static class TextureFormatTools
    {
        private static HashSet<InternalFormat> genericCompressedFormats = new HashSet<InternalFormat>()
        {
            InternalFormat.CompressedRed,
            InternalFormat.CompressedRg,
            InternalFormat.CompressedRgb,
            InternalFormat.CompressedRgba,
            InternalFormat.CompressedSrgb,
            InternalFormat.CompressedSrgbAlpha
        };

        /// <summary>
        /// Calculates the imageSize parameter for GL.CompressedTexImage. 
        /// The imageSize should be recalculated for each mip level when reading mipmaps from existing image data.
        /// </summary>
        /// <param name="width">The width of the mip level in pixels</param>
        /// <param name="height">The height of the mip level in pixels</param>
        /// <param name="format">The <paramref name="format"/> should be a compressed format.</param>
        /// <returns></returns>
        public static int CalculateImageSize(int width, int height, InternalFormat format)
        {
            int blockSize = CalculateBlockSize(format);

            int imageSize = blockSize * (int)Math.Ceiling(width / 4.0) * (int)Math.Ceiling(height / 4.0);
            return imageSize;
        }

        private static int CalculateBlockSize(InternalFormat format)
        {
            return CompressedBlockSize.blockSizeByFormat[format.ToString()];
        }

        /// <summary>
        /// Determines whether a format is compressed.
        /// Compressed formats should use GL.CompressedTexImage instead of GL.TexImage.
        /// </summary>
        /// <param name="format">The image format for the texture data</param>
        /// <returns>True if the format is compressed</returns>
        public static bool IsCompressed(InternalFormat format)
        {
            // All the enum value names should follow this convention.
            return format.ToString().ToLower().Contains("compressed");
        }

        /// <summary>
        /// Determines whether a format is compressed.
        /// Compressed formats should use GL.CompressedTexImage instead of GL.TexImage.
        /// </summary>
        /// <param name="format">The image format for the texture data</param>
        /// <returns>True if the format is compressed</returns>
        public static bool IsCompressed(PixelInternalFormat format)
        {
            // All the enum value names should follow this convention.
            return format.ToString().ToLower().Contains("compressed");
        }

        /// <summary>
        /// Determines if <paramref name="format"/> is a valid format for a 
        /// <see cref="DepthTexture"/>.
        /// </summary>
        /// <param name="format">The image format for the texture data</param>
        /// <returns>True if the format is a valid depth texture format</returns>
        public static bool IsDepthFormat(PixelInternalFormat format)
        {
            string formatName = format.ToString().ToLower();
            return formatName.Contains("depthcomponent");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsGenericCompressedFormat(InternalFormat format)
        {
            return genericCompressedFormats.Contains(format);    
        }
    }
}
