using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Framebuffers;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Encapsulates an OpenGL texture object. 
    /// </summary>
    public abstract class Texture : GLObject, IFramebufferAttachment
    {
        internal override GLObjectType ObjectType => GLObjectType.Texture;

        /// <summary>
        /// The width of the base mip level in pixels.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// The height of the base mip level in pixels.
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// The <see cref="OpenTK.Graphics.OpenGL.TextureTarget"/> for this texture.
        /// </summary>
        public TextureTarget TextureTarget { get; }

        /// <summary>
        /// The algorithm used when scaling the texture below its actual size.
        /// Defaults to <see cref="TextureMinFilter.Linear"/>.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            get => minFilter;
            set
            {
                minFilter = value;
                SetTexParameter(TextureParameterName.TextureMinFilter, (int)value);
            }
        }
        private TextureMinFilter minFilter;

        /// <summary>
        /// The algorithm used when scaling the texture above its actual size.
        /// Defaults to <see cref="TextureMagFilter.Linear"/>.
        /// </summary>
        public TextureMagFilter MagFilter
        {
            get => magFilter;
            set
            {
                magFilter = value;
                SetTexParameter(TextureParameterName.TextureMagFilter, (int)value);
            }
        }
        private TextureMagFilter magFilter;

        /// <summary>
        /// The wrap mode for the first component of texture coordinates.
        /// Defaults to <see cref="TextureWrapMode.ClampToEdge"/>.
        /// </summary>
        public TextureWrapMode TextureWrapS
        {
            get => textureWrapS;
            set
            {
                textureWrapS = value;
                SetTexParameter(TextureParameterName.TextureWrapS, (int)value);
            }
        }
        private TextureWrapMode textureWrapS;

        /// <summary>
        /// The wrap mode for the second component of texture coordinates.
        /// Defaults to <see cref="TextureWrapMode.ClampToEdge"/>.
        /// </summary>
        public TextureWrapMode TextureWrapT
        {
            get => textureWrapT;
            set
            {
                textureWrapT = value;
                SetTexParameter(TextureParameterName.TextureWrapT, (int)value);
            }
        }
        private TextureWrapMode textureWrapT;

        /// <summary>
        /// The wrap mode for the third component of texture coordinates.
        /// Defaults to <see cref="TextureWrapMode.ClampToEdge"/>.
        /// </summary>
        public TextureWrapMode TextureWrapR
        {
            get => textureWrapR;
            set
            {
                textureWrapR = value;
                SetTexParameter(TextureParameterName.TextureWrapR, (int)value);
            }
        }
        private TextureWrapMode textureWrapR;

        /// <summary>
        /// Creates an empty texture of the specified target.
        /// </summary>
        /// <param name="textureTarget">The target to which <see cref="GLObject.Id"/> is bound.</param>
        protected Texture(TextureTarget textureTarget) : base(GL.GenTexture())
        {
            TextureTarget = textureTarget;

            // Set some reasonable defaults to avoid common issues with mipmaps or uvs.
            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;
            MagFilter = TextureMagFilter.Linear;
            MinFilter = TextureMinFilter.Linear;
        }

        /// <summary>
        /// Binds the Id to <see cref="TextureTarget"/>.
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(TextureTarget, Id);
        }

        /// <summary>
        /// Binds the texture to <paramref name="attachment"/> for
        /// <paramref name="target"/>.
        /// </summary>
        /// <param name="attachment">The attachment point</param>
        /// <param name="target">The target framebuffer for attachment</param>
        public void Attach(FramebufferAttachment attachment, Framebuffer target)
        {
            target.Bind();
            GL.FramebufferTexture(target.Target, attachment, Id, 0);
        }

        /// <summary>
        /// Gets the image data for <paramref name="mipLevel"/>
        /// converted to an RGBA unsigned byte format.
        /// </summary>
        /// <param name="mipLevel">The mip level to read</param>
        /// <returns>The image data for <paramref name="mipLevel"/></returns>
        public byte[] GetImageDataRgba(int mipLevel)
        {
            int channels = 4;
            byte[] data = new byte[Width * Height * sizeof(byte) * channels];

            GL.GetTexImage(TextureTarget, mipLevel, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            return data;
        }

        /// <summary>
        /// Gets the image data for <paramref name="mipLevel"/> 
        /// converted to a BGRA unsigned byte format used for Bitmaps (PNG).
        /// </summary>
        /// <param name="mipLevel">The mip level to read</param>
        /// <returns>The image data for <paramref name="mipLevel"/></returns>
        public byte[] GetImageDataBgra(int mipLevel = 0)
        {
            int channels = 4;
            byte[] data = new byte[Width * Height * sizeof(byte) * channels];

            GL.GetTexImage(TextureTarget, mipLevel, PixelFormat.Bgra, PixelType.UnsignedByte, data);
            return data;
        }

        private void SetTexParameter(TextureParameterName param, int value)
        {
            Bind();
            GL.TexParameter(TextureTarget, param, value);
        }
    }
}
