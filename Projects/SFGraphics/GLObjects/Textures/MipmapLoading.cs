using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Provides methods for loading mipmaps for OpenGL textures. Bind the texture first.
    /// </summary>
    public static class MipmapLoading
    {
        private static readonly int minMipLevel = 0;

        /// <summary>
        /// Loads compressed 2D image data of the compressed format <paramref name="format"/> 
        /// for all the mip levels in <paramref name="mipmaps"/>.
        /// The texture must first be bound to the proper target before calling this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The list of mipmaps to load for <paramref name="target"/></param>
        /// <param name="format">The format for all mipmaps</param>
        public static void LoadCompressedMipMaps<T>(TextureTarget target, int width, int height, 
            List<T[]> mipmaps, InternalFormat format) where T : struct
        {
            // The number of mipmaps needs to be specified first.
            if (!TextureFormatTools.IsCubeMapTarget(target))
            {
                int maxMipLevel = Math.Max(mipmaps.Count - 1, minMipLevel);
                GL.TexParameter(target, TextureParameterName.TextureMaxLevel, maxMipLevel);
            }

            // Load mipmaps in the inclusive range [0, max level]
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = width / (int)Math.Pow(2, mipLevel);
                int mipHeight = height / (int)Math.Pow(2, mipLevel);
                LoadCompressedMipLevel(target, mipWidth, mipHeight, mipmaps[mipLevel], format, mipLevel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mipmaps"></param>
        /// <param name="format"></param>
        public static void LoadCompressedMipMaps(TextureTarget target, int width, int height,
            List<BufferObject> mipmaps, InternalFormat format)
        {
            // The number of mipmaps needs to be specified first.
            if (!TextureFormatTools.IsCubeMapTarget(target))
            {
                int maxMipLevel = Math.Max(mipmaps.Count - 1, minMipLevel);
                GL.TexParameter(target, TextureParameterName.TextureMaxLevel, maxMipLevel);
            }

            // Load mipmaps in the inclusive range [0, max level]
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = width / (int)Math.Pow(2, mipLevel);
                int mipHeight = height / (int)Math.Pow(2, mipLevel);
                int mipImageSize = TextureFormatTools.CalculateImageSize(mipWidth, mipHeight, format);

                // Load image data from buffer
                mipmaps[mipLevel].Bind();
                IntPtr bufferOffset = IntPtr.Zero;
                GL.CompressedTexImage2D(target, mipLevel, format, mipWidth, mipHeight, 0, 
                    mipImageSize, bufferOffset);
                GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
            }
        }

        /// <summary>
        /// Loads the base mip level from <paramref name="image"/> and generates mipmaps.
        /// Mipmaps are not generated for cube map targets.
        /// </summary>
        /// <param name="target">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="image"></param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget target, Bitmap image)
        {
            LoadMipLevelFromBitmap(target, 0, image);

            if (!TextureFormatTools.IsCubeMapTarget(target))
                GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="target">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel"></param>
        /// <param name="format">The format for all mipmaps</param>
        public static void LoadBaseLevelGenerateMipmaps<T>(TextureTarget target, int width, int height,
            T[] baseMipLevel, InternalFormat format) where T : struct
        {
            LoadCompressedMipLevel(target, width, height, baseMipLevel, format, 0);

            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="baseMipLevel"></param>
        /// <param name="internalFormat"></param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget target, int width, int height,
            BufferObject baseMipLevel, InternalFormat internalFormat)
        {
            // Calculate the proper imageSize.
            int baseImageSize = TextureFormatTools.CalculateImageSize(width, height, internalFormat);

            // Load the first level.
            baseMipLevel.Bind();
            IntPtr bufferOffset = IntPtr.Zero;
            GL.CompressedTexImage2D(target, 0, internalFormat, width, height, 0, 
                baseImageSize, bufferOffset);
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0); //unbind

            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="target">The target of the texture or cube face for loading mip maps. 
        /// Ex: Texture2D or TextureCubeMapPositiveX.</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel"></param>
        /// <param name="format">The uncompressed format information</param>
        public static void LoadBaseLevelGenerateMipmaps<T>(TextureTarget target, int width, int height, 
            T[] baseMipLevel, TextureFormatUncompressed format) where T : struct
        {
            // Load the first level.
            GL.TexImage2D(target, 0, format.pixelInternalFormat, width, height, 0,
                format.pixelFormat, format.pixelType, baseMipLevel);

            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="baseMipLevel"></param>
        /// <param name="format"></param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget target, int width, int height, 
            BufferObject baseMipLevel, TextureFormatUncompressed format)
        {
            // Load the first level.
            baseMipLevel.Bind();
            IntPtr bufferOffset = IntPtr.Zero;
            GL.TexImage2D(target, 0, format.pixelInternalFormat, width, height, 0, 
                format.pixelFormat, format.pixelType, bufferOffset);
            GL.BindBuffer(baseMipLevel.Target, 0); //unbind

            GL.GenerateMipmap((GenerateMipmapTarget)target);
        }

        /// <summary>
        /// Loads image data for all six faces of a cubemap. No mipmaps are generated, so use a min filter
        /// that does not use mipmaps.
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="length">The width and heigh of each cube map face in pixels</param>
        /// <param name="format"></param>
        /// <param name="posX"></param>
        /// <param name="negX"></param>
        /// <param name="posY"></param>
        /// <param name="negY"></param>
        /// <param name="posZ"></param>
        /// <param name="negZ"></param>
        public static void LoadFacesBaseLevel<T>(int length, TextureFormatUncompressed format,
            T[] posX, T[] negX, T[] posY, T[] negY, T[] posZ, T[] negZ) where T : struct
        {
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveX, length, length, posX, format);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeX, length, length, negX, format);

            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveY, length, length, posY, format);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeY, length, length, negY, format);

            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveZ, length, length, posZ, format);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeZ, length, length, negZ, format);
        }

        /// <summary>
        /// Loads image data and mipmaps for all six faces of a cube map.
        /// </summary>
        /// <typeparam name="T">The value type used for the image data. This inclues arithmetic types.</typeparam>
        /// <param name="length">The width and heigh of each cube map face in pixels</param>
        /// <param name="format"></param>
        /// <param name="mipsPosX"></param>
        /// <param name="mipsNegX"></param>
        /// <param name="mipsPosY"></param>
        /// <param name="mipsNegY"></param>
        /// <param name="mipsPosZ"></param>
        /// <param name="mipsNegZ"></param>
        public static void LoadFacesMipmaps<T>(int length, InternalFormat format,
            List<T[]> mipsPosX, List<T[]> mipsNegX, List<T[]> mipsPosY, 
            List<T[]> mipsNegY, List<T[]> mipsPosZ, List<T[]> mipsNegZ) where T : struct
        {
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveX, length, length, mipsPosX, format);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeX, length, length, mipsNegX, format);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveY, length, length, mipsPosY, format);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeY, length, length, mipsNegY, format);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveZ, length, length, mipsPosZ, format);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeZ, length, length, mipsNegZ, format);
        }

        private static void LoadMipLevelFromBitmap(TextureTarget target, int level, Bitmap image)
        {
            System.Drawing.Imaging.BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(target, level, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);
        }

        private static void LoadCompressedMipLevel<T>(TextureTarget target, int width, int height, T[] imageData,
            InternalFormat format, int mipLevel) where T : struct
        {
            int border = 0;
            int imageSize = TextureFormatTools.CalculateImageSize(width, height, format);
            GL.CompressedTexImage2D(target, mipLevel, format, width, height, border, imageSize, imageData);
        }
    }
}
