using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A TextureTarget.Texture3D texture designed for 3D lookup tables.
    /// </summary>
    public class Texture3D : Texture
    {
        /// <summary>
        /// The depth of the base mip level in pixels.
        /// </summary>
        public int Depth { get; protected set; }

        /// <summary>
        /// Creates an empty 3D texture. 
        /// The texture is incomplete until the dimensions and format are set.
        /// </summary>
        public Texture3D() : base(TextureTarget.Texture3D)
        {

        }

        /// <summary>
        /// Loads uncompressed texture data of the specified format for the first mip level.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="imageData"></param>
        /// <param name="format"></param>
        public void LoadImageData<T>(int width, int height, int depth, T[] imageData, TextureFormats.TextureFormatUncompressed format) where T : struct
        {
            Width = width;
            Height = height;
            Depth = depth;

            Bind();
            GL.TexImage3D(TextureTarget, 0, format.pixelInternalFormat, width, height, depth, 0, format.pixelFormat, format.pixelType, imageData);
        }
    }
}
