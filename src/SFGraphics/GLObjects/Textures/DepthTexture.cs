using System;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelInternalFormat"></param>
        public DepthTexture(int width, int height, PixelInternalFormat pixelInternalFormat) : base(TextureTarget.Texture2D, pixelInternalFormat)
        {
            Width = width;
            Height = height;

            // TODO: Throw argument exception if the format isn't a depth map format.
            
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
