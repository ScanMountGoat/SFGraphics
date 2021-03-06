﻿using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Textures.TextureFormats;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SFGraphics.GLObjects.Textures.Utils
{
    /// <summary>
    /// Provides methods for loading mipmaps for OpenGL textures. 
    /// </summary>
    public static class MipmapLoading
    {
        private static readonly int minMipLevel = 0;
        private static readonly int border = 0;
        private static readonly IntPtr bufferOffset = IntPtr.Zero;

        /// <summary>
        /// Loads compressed 2D image data of the compressed format <paramref name="format"/> 
        /// for all the mip levels in <paramref name="mipmaps"/>.
        /// The texture must first be bound to the proper target.
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The list of mipmaps to load for <paramref name="target"/></param>
        /// <param name="format">The input format and internal format information</param>
        public static void LoadCompressedMipMaps<T>(TextureTarget target, int width, int height, 
            IList<T[]> mipmaps, InternalFormat format) where T : struct
        {
            SetMaxMipLevel(target, mipmaps);

            // Load mipmaps in the inclusive range [0, max level]
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = CalculateMipDimension(width, mipLevel);
                int mipHeight = CalculateMipDimension(height, mipLevel);

                LoadCompressedMipLevel(target, mipWidth, mipHeight, mipmaps[mipLevel], format, mipLevel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The collection of mipmaps to load for <paramref name="target"/></param>
        /// <param name="format">The input format and internal format information</param>
        public static void LoadCompressedMipmaps(TextureTarget target, int width, int height,
            IList<BufferObject> mipmaps, InternalFormat format)
        {
            SetMaxMipLevel(target, mipmaps);

            // Load mipmaps in the inclusive range [0, max level]
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = CalculateMipDimension(width, mipLevel);
                int mipHeight = CalculateMipDimension(height, mipLevel);

                LoadCompressedMipLevel(target, mipWidth, mipHeight, mipmaps[mipLevel], format, mipLevel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The collection of mipmaps to load for <paramref name="target"/></param>
        /// <param name="format">The input format and internal format information</param>
        public static void LoadUncompressedMipmaps(TextureTarget target, int width, int height,
            IList<BufferObject> mipmaps, TextureFormatUncompressed format)
        {
            SetMaxMipLevel(target, mipmaps);

            // Load mipmaps in the inclusive range [0, max level]
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = CalculateMipDimension(width, mipLevel);
                int mipHeight = CalculateMipDimension(height, mipLevel);

                LoadUncompressedMipLevel(target, mipWidth, mipHeight, mipmaps[mipLevel], format, mipLevel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="mipmaps">The collection of mipmaps to load for <paramref name="target"/></param>
        /// <param name="format">The format information</param>
        public static void LoadUncompressedMipmaps<T>(TextureTarget target, int width, int height,
            IList<T[]> mipmaps, TextureFormatUncompressed format) where T : struct
        {
            SetMaxMipLevel(target, mipmaps);

            // Load mipmaps in the inclusive range [0, max level]
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                int mipWidth = CalculateMipDimension(width, mipLevel);
                int mipHeight = CalculateMipDimension(height, mipLevel);

                LoadUncompressedMipLevel(target, mipWidth, mipHeight, mipmaps[mipLevel], format, mipLevel);
            }
        }

        /// <summary>
        /// Loads the base mip level from <paramref name="image"/> and generates mipmaps.
        /// Mipmaps are not generated for cube map targets.
        /// </summary>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="image">The image data to load for the base level</param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget target, Bitmap image)
        {
            LoadUncompressedMipLevel(target, image, 0);
            GenerateMipmaps(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel"></param>
        /// <param name="format">The input format and internal format information</param>
        public static void LoadBaseLevelGenerateMipmaps<T>(TextureTarget target, int width, int height,
            T[] baseMipLevel, InternalFormat format) where T : struct
        {
            LoadCompressedMipLevel(target, width, height, baseMipLevel, format, 0);
            GenerateMipmaps(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel">The data to load for the base mip level</param>
        /// <param name="internalFormat">The image format</param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget target, int width, int height,
            BufferObject baseMipLevel, InternalFormat internalFormat)
        {
            LoadCompressedMipLevel(target, width, height, baseMipLevel, internalFormat, 0);
            GenerateMipmaps(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="target">The target of the texture or cube face for loading mipmaps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel"></param>
        /// <param name="format">The input format and internal format information</param>
        public static void LoadBaseLevelGenerateMipmaps<T>(TextureTarget target, int width, int height, 
            T[] baseMipLevel, TextureFormatUncompressed format) where T : struct
        {
            LoadUncompressedMipLevel(target, width, height, baseMipLevel, format, 0);
            GenerateMipmaps(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">The target of the texture or cube face for loading mip maps</param>
        /// <param name="width">The width of the texture or cube map face in pixels</param>
        /// <param name="height">The height of the texture or cube map face in pixels</param>
        /// <param name="baseMipLevel">The data to load for the base mip level</param>
        /// <param name="format">The input format and internal format information</param>
        public static void LoadBaseLevelGenerateMipmaps(TextureTarget target, int width, int height, 
            BufferObject baseMipLevel, TextureFormatUncompressed format)
        {
            LoadUncompressedMipLevel(target, width, height, baseMipLevel, format, 0);
            GenerateMipmaps(target);
        }

        /// <summary>
        /// Loads image data for all six faces of a cubemap. No mipmaps are generated.
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="length">The width and height of each cube map face in pixels</param>
        /// <param name="format">The image format</param>
        /// <param name="posX">Mip map data for the positive x target</param>
        /// <param name="negX">Mip map data for the negative x target</param>
        /// <param name="posY">Mip map data for the positive y target</param>
        /// <param name="negY">Mip map data for the negative y target</param>
        /// <param name="posZ">Mip map data for the positive z target</param>
        /// <param name="negZ">Mip map data for the negative z target</param>
        public static void LoadFacesBaseLevel<T>(int length, TextureFormatUncompressed format,
            T[] posX, T[] negX, T[] posY, T[] negY, T[] posZ, T[] negZ) where T : struct
        {
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveX, length, length, posX, format);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeX, length, length, negX, format);

            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveY, length, length, posY, format);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeY, length, length, negY, format);

            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapPositiveZ, length, length, posZ, format);
            LoadBaseLevelGenerateMipmaps(TextureTarget.TextureCubeMapNegativeZ, length, length, negZ, format);

            // Mipmaps can't be generated until the texture is cube map complete.
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }

        /// <summary>
        /// Loads image data and mipmaps for all six faces of a cube map.
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="length">The width and height of each cube map face in pixels</param>
        /// <param name="format">The image format</param>
        /// <param name="mipsPosX">Mip map data for the positive x target</param>
        /// <param name="mipsNegX">Mip map data for the negative x target</param>
        /// <param name="mipsPosY">Mip map data for the positive y target</param>
        /// <param name="mipsNegY">Mip map data for the negative y target</param>
        /// <param name="mipsPosZ">Mip map data for the positive z target</param>
        /// <param name="mipsNegZ">Mip map data for the negative z target</param>
        public static void LoadFacesMipmaps<T>(int length, TextureFormatUncompressed format,
            IList<T[]> mipsPosX, IList<T[]> mipsNegX, IList<T[]> mipsPosY,
            IList<T[]> mipsNegY, IList<T[]> mipsPosZ, IList<T[]> mipsNegZ) where T : struct
        {
            // Assume all the mipmap counts are the same.
            SetMaxMipLevel(TextureTarget.TextureCubeMap, mipsPosX);

            LoadUncompressedMipmaps(TextureTarget.TextureCubeMapPositiveX, length, length, mipsPosX, format);
            LoadUncompressedMipmaps(TextureTarget.TextureCubeMapNegativeX, length, length, mipsNegX, format);

            LoadUncompressedMipmaps(TextureTarget.TextureCubeMapPositiveY, length, length, mipsPosY, format);
            LoadUncompressedMipmaps(TextureTarget.TextureCubeMapNegativeY, length, length, mipsNegY, format);

            LoadUncompressedMipmaps(TextureTarget.TextureCubeMapPositiveZ, length, length, mipsPosZ, format);
            LoadUncompressedMipmaps(TextureTarget.TextureCubeMapNegativeZ, length, length, mipsNegZ, format);
        }

        /// <summary>
        /// Loads image data and mipmaps for all six faces of a cube map.
        /// </summary>
        /// <typeparam name="T">The value type of the image data</typeparam>
        /// <param name="length">The width and height of each cube map face in pixels</param>
        /// <param name="format">The image format</param>
        /// <param name="mipsPosX">Mip map data for the positive x target</param>
        /// <param name="mipsNegX">Mip map data for the negative x target</param>
        /// <param name="mipsPosY">Mip map data for the positive y target</param>
        /// <param name="mipsNegY">Mip map data for the negative y target</param>
        /// <param name="mipsPosZ">Mip map data for the positive z target</param>
        /// <param name="mipsNegZ">Mip map data for the negative z target</param>
        public static void LoadFacesMipmaps<T>(int length, InternalFormat format,
            IList<T[]> mipsPosX, IList<T[]> mipsNegX, IList<T[]> mipsPosY,
            IList<T[]> mipsNegY, IList<T[]> mipsPosZ, IList<T[]> mipsNegZ) where T : struct
        {
            // Assume all the mipmap counts are the same.
            SetMaxMipLevel(TextureTarget.TextureCubeMap, mipsPosX);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveX, length, length, mipsPosX, format);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeX, length, length, mipsNegX, format);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveY, length, length, mipsPosY, format);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeY, length, length, mipsNegY, format);

            LoadCompressedMipMaps(TextureTarget.TextureCubeMapPositiveZ, length, length, mipsPosZ, format);
            LoadCompressedMipMaps(TextureTarget.TextureCubeMapNegativeZ, length, length, mipsNegZ, format);
        }

        private static void GenerateMipmaps(TextureTarget target)
        {
            if (!TextureFormatTools.IsCubeMapTarget(target))
                GL.GenerateMipmap(GetMipmapTarget(target));
        }

        private static GenerateMipmapTarget GetMipmapTarget(TextureTarget target)
        {
            // TODO: Support other types.
            if (target == TextureTarget.Texture2D)
                return GenerateMipmapTarget.Texture2D;
            if (target.ToString().ToLower().Contains("cubemap"))
                return GenerateMipmapTarget.TextureCubeMap;

            throw new NotImplementedException($"{target} is not supported for mipmap generation.");
        }

        private static void SetMaxMipLevel<T>(TextureTarget target, IList<T> mipmaps)
        {
            // The mip count can't be set for individual cubemap face targets.
            if (!TextureFormatTools.IsCubeMapTarget(target))
            {
                int maxMipLevel = Math.Max(mipmaps.Count - 1, minMipLevel);
                GL.TexParameter(target, TextureParameterName.TextureMaxLevel, maxMipLevel);
            }
        }

        private static int CalculateMipDimension(int baseLevelDimension, int mipLevel)
        {
            return baseLevelDimension / (int)Math.Pow(2, mipLevel);
        }

        private static void LoadUncompressedMipLevel(TextureTarget target, Bitmap image, int level)
        {
            System.Drawing.Imaging.BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(target, level, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);
        }

        private static void LoadUncompressedMipLevel<T>(TextureTarget target, int width, int height,
            T[] baseMipLevel, TextureFormatUncompressed format, int mipLevel) where T : struct
        {
            GL.TexImage2D(target, mipLevel, format.pixelInternalFormat, width, height, border,
                format.pixelFormat, format.pixelType, baseMipLevel);
        }

        private static void LoadUncompressedMipLevel(TextureTarget target, int width, int height, 
            BufferObject imageBuffer, TextureFormatUncompressed format, int mipLevel)
        {
            imageBuffer.Bind(BufferTarget.PixelUnpackBuffer);

            GL.TexImage2D(target, mipLevel, format.pixelInternalFormat, width, height, border,
                format.pixelFormat, format.pixelType, bufferOffset);

            // Unbind to avoid affecting other texture operations.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
        }

        private static void LoadCompressedMipLevel<T>(TextureTarget target, int width, int height, 
            T[] imageData, InternalFormat format, int mipLevel) where T : struct
        {
            int imageSize = TextureFormatTools.CalculateImageSize(width, height, format);
            GL.CompressedTexImage2D(target, mipLevel, format, width, height, border, imageSize, imageData);
        }

        private static void LoadCompressedMipLevel(TextureTarget target, int width, int height, 
            BufferObject imageBuffer, InternalFormat format, int mipLevel)
        {
            imageBuffer.Bind(BufferTarget.PixelUnpackBuffer);

            int imageSize = TextureFormatTools.CalculateImageSize(width, height, format);
            GL.CompressedTexImage2D(target, mipLevel, format, width, height, border, imageSize, bufferOffset);

            // Unbind to avoid affecting other texture operations.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
        }
    }
}
