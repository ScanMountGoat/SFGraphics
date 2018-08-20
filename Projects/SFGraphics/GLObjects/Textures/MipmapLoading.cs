using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Provides methods for loading mipmaps for OpenGL textures from arrays of image data.
    /// The arrays can be of any value type. As long as the correct image format information is used,
    /// OpenGL will still interpret the data correctly.
    /// Make sure to bind the texture before calling these methods.
    /// </summary>
    public static class MipmapLoading
    {
        private static readonly int minMipLevel = 0;

        /// <summary>
        /// Loads compressed 2D image data of the compressed format <paramref name="internalFormat"/> 
        /// for all the mip levels in <paramref name="mipmaps"/>.
        /// The texture must first be bound to the proper target before calling this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textureTarget">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The list of mipmaps to load for <paramref name="textureTarget"/></param>
        /// <param name="internalFormat">The format for all mipmaps</param>
        public static void LoadCompressedMipMaps<T>(TextureTarget textureTarget, int width, int height, 
            List<T[]> mipmaps, InternalFormat internalFormat) where T : struct
        {
            // The number of mipmaps needs to be specified first.
            if (!textureTarget.ToString().ToLower().Contains("cubemap"))
            {
                int maxMipLevel = Math.Max(mipmaps.Count - 1, minMipLevel);
                GL.TexParameter(textureTarget, TextureParameterName.TextureMaxLevel, maxMipLevel);
            }

            // Load mipmaps in the inclusive range [0, max level]
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
        /// <param name="textureTarget">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
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
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="textureTarget">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel"></param>
        /// <param name="mipCount">The total number of mipmaps. Negative values are converted to <c>0</c></param>
        /// <param name="internalFormat">The format for all mipmaps</param>
        public static void LoadBaseLevelGenerateMipmaps<T>(TextureTarget textureTarget, int width, int height, 
            T[] baseMipLevel, int mipCount, InternalFormat internalFormat) where T : struct
        {
            // The number of mipmaps needs to be specified first.
            int maxMipLevel = Math.Max(mipCount - 1, minMipLevel);
            GL.TexParameter(textureTarget, TextureParameterName.TextureMaxLevel, maxMipLevel);

            // Calculate the proper imageSize.
            int baseImageSize = TextureFormatTools.CalculateImageSize(width, height, internalFormat);

            // Load the first level.
            GL.CompressedTexImage2D(textureTarget, 0, internalFormat, width, height, 0, baseImageSize, baseMipLevel);
            GL.GenerateMipmap((GenerateMipmapTarget)textureTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="textureTarget">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel"></param>
        /// <param name="mipCount">The total number of mipmaps. Negative values are converted to <c>0</c></param>
        /// <param name="textureFormat">The uncompressed format information</param>
        public static void LoadBaseLevelGenerateMipmaps<T>(TextureTarget textureTarget, int width, int height, T[] baseMipLevel, int mipCount, 
            TextureFormatUncompressed textureFormat) where T : struct
        {
            // The number of mipmaps needs to be specified first.
            int maxMipLevel = Math.Max(mipCount - 1, minMipLevel);
            GL.TexParameter(textureTarget, TextureParameterName.TextureMaxLevel, maxMipLevel);

            // Load the first level.
            GL.TexImage2D(textureTarget, 0, textureFormat.pixelInternalFormat, width, height, 0,
                textureFormat.pixelFormat, textureFormat.pixelType, baseMipLevel);

            // The number of mip maps needs to be specified first.
            GL.TexParameter(textureTarget, TextureParameterName.TextureMaxLevel, mipCount);
            GL.GenerateMipmap((GenerateMipmapTarget)textureTarget);
        }

        /// <summary>
        /// Loads image data for all six faces of a cubemap. No mipmaps are generated, so use a min filter
        /// that does not use mipmaps.
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="length">The width and heigh of each cube map face in pixels</param>
        /// <param name="textureFormat"></param>
        /// <param name="facePosX"></param>
        /// <param name="faceNegX"></param>
        /// <param name="facePosY"></param>
        /// <param name="faceNegY"></param>
        /// <param name="facePosZ"></param>
        /// <param name="faceNegZ"></param>
        public static void LoadFacesBaseLevel<T>(int length, TextureFormatUncompressed textureFormat,
            T[] facePosX, T[] faceNegX, T[] facePosY, T[] faceNegY, T[] facePosZ, T[] faceNegZ) where T : struct
        {
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveX, length, length, facePosX, 0, textureFormat);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeX, length, length, faceNegX, 0, textureFormat);

            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveY, length, length, facePosY, 0, textureFormat);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeY, length, length, faceNegY, 0, textureFormat);

            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveZ, length, length, facePosZ, 0, textureFormat);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeZ, length, length, faceNegZ, 0, textureFormat);
        }

        /// <summary>
        /// Loads image data and mipmaps for all six faces of a cube map.
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="length">The width and heigh of each cube map face in pixels</param>
        /// <param name="internalFormat"></param>
        /// <param name="mipsPosX"></param>
        /// <param name="mipsNegX"></param>
        /// <param name="mipsPosY"></param>
        /// <param name="mipsNegY"></param>
        /// <param name="mipsPosZ"></param>
        /// <param name="mipsNegZ"></param>
        public static void LoadFacesMipmaps<T>(int length, InternalFormat internalFormat,
            List<T[]> mipsPosX, List<T[]> mipsNegX, List<T[]> mipsPosY, 
            List<T[]> mipsNegY, List<T[]> mipsPosZ, List<T[]> mipsNegZ) where T : struct
        {
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveX, length, length, mipsPosX, internalFormat);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeX, length, length, mipsNegX, internalFormat);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveY, length, length, mipsPosY, internalFormat);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeY, length, length, mipsNegY, internalFormat);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveZ, length, length, mipsPosZ, internalFormat);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeZ, length, length, mipsNegZ, internalFormat);
        }
    }
}
