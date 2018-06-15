using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    public class Texture2D : Texture
    {
        /// <summary>
        /// Initialize an empty Texture2D of the specified dimensions.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Texture2D(int width, int height) : base(TextureTarget.Texture2D, width, height)
        {

        }

        /// <summary>
        /// Initialize an RGBA texture from the specified bitmap.
        /// </summary>
        /// <param name="image"></param>
        public Texture2D(Bitmap image) : base(TextureTarget.Texture2D, image.Width, image.Height)
        {
            // Load the image data.
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        /// <summary>
        /// Initializes a texture of the specified format and loads all specified mipmaps.
        /// </summary>
        /// <param name="width">The width of the base mip level</param>
        /// <param name="height">The height of the base mip level</param>
        /// <param name="mipmaps">A list of byte arrays for each mip level</param>
        /// <param name="pixelInternalFormat"></param>
        public Texture2D(int width, int height, List<byte[]> mipmaps, PixelInternalFormat pixelInternalFormat) : base(TextureTarget.Texture2D, width, height)
        {
            Bind();
            
            // Load the first level.
            GL.CompressedTexImage2D<byte>(TextureTarget.Texture2D, 0, pixelInternalFormat, width, height, 0, mipmaps[0].Length, mipmaps[0]);

            // The number of mip maps needs to be specified first.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, mipmaps.Count);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Initialize the data for each level.
            for (int mipLevel = 0; mipLevel < mipmaps.Count; mipLevel++)
            {
                GL.CompressedTexImage2D<byte>(TextureTarget.Texture2D, mipLevel, pixelInternalFormat,
                    width / (int)Math.Pow(2, mipLevel), height / (int)Math.Pow(2, mipLevel), 0, mipmaps[mipLevel].Length, mipmaps[mipLevel]);
            }
        }

        /// <summary>
        /// Initializes a texture of the specified format. The additional mip levels are generated.
        /// </summary>
        /// <param name="width">The width of the base mip level</param>
        /// <param name="height">The height of the base mip level</param>
        /// <param name="baseMipLevel">The data for the base mip level.</param>
        /// <param name="pixelInternalFormat"></param>
        public Texture2D(int width, int height, byte[] baseMipLevel, PixelInternalFormat pixelInternalFormat) : base(TextureTarget.Texture2D, width, height)
        {
            Bind();

            // Load the first level and generate the rest.
            GL.CompressedTexImage2D<byte>(TextureTarget.Texture2D, 0, pixelInternalFormat, width, height, 0, baseMipLevel.Length, baseMipLevel);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}
