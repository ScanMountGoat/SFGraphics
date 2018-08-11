using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Provides methods for loading mipmaps for OpenGL textures from byte arrays of image data.
    /// </summary>
    public static class MipmapLoading
    {
        /// <summary>
        /// Loads compressed 2D image data of the compressed format <paramref name="internalFormat"/> 
        /// for all the mip levels in <paramref name="mipmaps"/>.
        /// The texture must first be bound to the proper target before calling this method.
        /// </summary>
        /// <param name="textureTarget">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The list of mipmaps to load for <paramref name="textureTarget"/></param>
        /// <param name="internalFormat"></param>
        public static void LoadCompressedMipMaps(TextureTarget textureTarget, int width, int height, 
            List<byte[]> mipmaps, InternalFormat internalFormat)
        {
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = width / (int)Math.Pow(2, mipLevel);
                int mipHeight = height / (int)Math.Pow(2, mipLevel);
                int mipImageSize = TextureFormatTools.CalculateImageSize(mipWidth, mipHeight, internalFormat);
                GL.CompressedTexImage2D(textureTarget, mipLevel, internalFormat,
                    mipWidth, mipHeight, 0, mipImageSize, mipmaps[mipLevel]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureTarget"></param>
        /// <param name="image"></param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget textureTarget, Bitmap image)
        {
            // Load the image data.
            System.Drawing.Imaging.BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);

            // TODO: Set max level?
            // Generate mipmaps.
            GL.GenerateMipmap((GenerateMipmapTarget)textureTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureTarget"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="baseMipLevel"></param>
        /// <param name="mipCount"></param>
        /// <param name="internalFormat"></param>
        public static void LoadBaseLevelGenerateMipMaps(TextureTarget textureTarget, int width, int height, 
            byte[] baseMipLevel, int mipCount, InternalFormat internalFormat)
        {
            // Calculate the proper imageSize.
            int baseImageSize = TextureFormatTools.CalculateImageSize(width, height, internalFormat);

            // Load the first level.
            GL.CompressedTexImage2D(textureTarget, 0, internalFormat, width, height, 0, baseImageSize, baseMipLevel);

            // The number of mip maps needs to be specified first.
            GL.TexParameter(textureTarget, TextureParameterName.TextureMaxLevel, mipCount);
            GL.GenerateMipmap((GenerateMipmapTarget)textureTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureTarget"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="baseMipLevel"></param>
        /// <param name="mipCount"></param>
        /// <param name="pixelInternalFormat"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="pixelType"></param>
        public static void LoadBaseLevelGenerateMipMaps(TextureTarget textureTarget, int width, int height, byte[] baseMipLevel, int mipCount,
            PixelInternalFormat pixelInternalFormat, PixelFormat pixelFormat, PixelType pixelType)
        {
            // Load the first level.
            GL.TexImage2D(textureTarget, 0, pixelInternalFormat, width, height, 0,
                pixelFormat, pixelType, baseMipLevel);

            // The number of mip maps needs to be specified first.
            GL.TexParameter(textureTarget, TextureParameterName.TextureMaxLevel, mipCount);
            GL.GenerateMipmap((GenerateMipmapTarget)textureTarget);
        }
    }
}
