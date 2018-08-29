﻿using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A TextureTarget.Texture2D texture that supports mipmaps.
    /// </summary>
    public class Texture2D : Texture
    {
        /// <summary>
        /// The width of the base mip level in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the base mip level in pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Creates an empty 2D texture. 
        /// The texture is incomplete until the dimensions and format are set.
        /// </summary>
        public Texture2D() : base(TextureTarget.Texture2D)
        {

        }

        /// <summary>
        /// Loads RGBA texture data with mipmaps generated from the specified bitmap.
        /// Binds the texture.
        /// </summary>
        /// <param name="image">the image data for the base mip level</param>
        public void LoadImageData(Bitmap image)
        {
            Width = image.Width;
            Height = image.Height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, image);
        }

        /// <summary>
        /// Loads texture data of the specified format for the first mip level.
        /// Mipmaps are generated by OpenGL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="height">The height of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="baseMipLevel">The image data to load for the first mip level. The other levels are generated.</param>
        /// <param name="internalFormat">The image format of <paramref name="baseMipLevel"/></param>
        /// 
        /// <exception cref="ArgumentException"><paramref name="internalFormat"/> is not a compressed format.</exception>
        public void LoadImageData<T>(int width, int height, T[] baseMipLevel, InternalFormat internalFormat)
            where T : struct
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, internalFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="baseMipLevel"></param>
        /// <param name="internalFormat"></param>
        public void LoadImageData(int width, int height, BufferObject baseMipLevel, InternalFormat internalFormat)
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, internalFormat);
        }

        /// <summary>
        /// Loads texture data of the specified format for the first mip level.
        /// Mipmaps are generated by OpenGL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="height">The height of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="baseMipLevel">The image data to load for the first mip level. The other levels are generated.</param>
        /// <param name="textureFormat">The format information for the uncompressed format</param>
        public void LoadImageData<T>(int width, int height, T[] baseMipLevel, TextureFormatUncompressed textureFormat)
            where T : struct
        {
            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, textureFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="baseMipLevel"></param>
        /// <param name="format"></param>
        public void LoadImageData(int width, int height, BufferObject baseMipLevel, TextureFormatUncompressed format)
        {
            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, format);
        }

        /// <summary>
        /// Loads a mip level of compressed texture texture data
        /// for each array in <paramref name="mipmaps"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width of the base mip level</param>
        /// <param name="height">The height of the base mip level</param>
        /// <param name="mipmaps">A list of byte arrays for each mip level</param>
        /// <param name="internalFormat">The image format of <paramref name="mipmaps"/></param>
        /// <exception cref="ArgumentException"><paramref name="internalFormat"/> is not a compressed format.</exception>
        public void LoadImageData<T>(int width, int height, List<T[]> mipmaps, InternalFormat internalFormat)
            where T : struct
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            Width = width;
            Height = height;

            MipmapLoading.LoadCompressedMipMaps(TextureTarget.Texture2D, width, height, mipmaps, internalFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mipmaps"></param>
        /// <param name="internalFormat"></param>
        public void LoadImageData(int width, int height, List<BufferObject> mipmaps,
            InternalFormat internalFormat)
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            Width = width;
            Height = height;

            MipmapLoading.LoadCompressedMipMaps(TextureTarget.Texture2D, width, height, mipmaps, internalFormat);
        }
    }
}
