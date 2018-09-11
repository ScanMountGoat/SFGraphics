using System;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures.TextureFormats;
using SFGraphics.GLObjects.Textures.Utils;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A simple texture for storing depth information. 
    /// The texture can be attached to a <see cref="Framebuffer"/> object for shadow mapping and other effects.
    /// </summary>
    public class DepthTexture : Texture
    {
        /// <summary>
        /// The width of the texture in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the texture in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Creates an empty depth texture of the specified dimensions and format. 
        /// This texture does not use mipmaps.
        /// The border color is set to white, so attempts to sample outside the texture's border will return white.
        /// </summary>
        /// <param name="width">The width of the texture in pixels</param>
        /// <param name="height">The height of the texture in pixels</param>
        /// <param name="pixelInternalFormat">The internal format of the image data. This should be a valid depth format.</param>
        public DepthTexture(int width, int height, PixelInternalFormat pixelInternalFormat) : base(TextureTarget.Texture2D)
        {
            // Only certain formats are valid for a depth attachment.
            if (!TextureFormatTools.IsDepthFormat(pixelInternalFormat))
                throw new ArgumentException(TextureExceptionMessages.invalidDepthTexFormat);

            Width = width;
            Height = height;
            
            // Set texture settings.
            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, Width, Height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            MagFilter = TextureMagFilter.Nearest;
            MinFilter = TextureMinFilter.Nearest;

            // Use white for values outside the depth map's border.
            TextureWrapS = TextureWrapMode.ClampToBorder;
            TextureWrapT = TextureWrapMode.ClampToBorder;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });
        }
    }
}
