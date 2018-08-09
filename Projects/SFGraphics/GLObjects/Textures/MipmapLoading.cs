using System;
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
    }
}
