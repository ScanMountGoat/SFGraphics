using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Encapsulates the state for an OpenGL texture object. To support texture types other than 
    /// <see cref="Texture2D"/> and <see cref="TextureCubeMap"/>, inherit from this class
    /// and add the necessary additional methods. 
    /// </summary>
    public abstract class Texture : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.Texture; } }

        /// <summary>
        /// The <see cref="OpenTK.Graphics.OpenGL.TextureTarget"/> for this texture.
        /// </summary>
        public TextureTarget TextureTarget { get; }

        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            get { return minFilter; }
            set
            {
                minFilter = value;
                SetTexParameter(TextureParameterName.TextureMinFilter, (int)value);
            }
        }
        private TextureMinFilter minFilter;

        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureMagFilter MagFilter
        {
            get { return magFilter; }
            set
            {
                magFilter = value;
                SetTexParameter(TextureParameterName.TextureMagFilter, (int)value);
            }
        }
        private TextureMagFilter magFilter;

        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapS
        {
            get { return textureWrapS; }
            set
            {
                textureWrapS = value;
                SetTexParameter(TextureParameterName.TextureWrapS, (int)value);
            }
        }
        private TextureWrapMode textureWrapS;

        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapT
        {
            get { return textureWrapT; }
            set
            {
                textureWrapT = value;
                SetTexParameter(TextureParameterName.TextureWrapT, (int)value);
            }
        }
        private TextureWrapMode textureWrapT;

        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapR
        {
            get { return textureWrapR; }
            set
            {
                textureWrapR = value;
                SetTexParameter(TextureParameterName.TextureWrapR, (int)value);
            }
        }
        private TextureWrapMode textureWrapR;

        /// <summary>
        /// Creates an empty texture of the specified target and internal format.
        /// </summary>
        /// <param name="textureTarget">The target to which <see cref="GLObject.Id"/> is bound.</param>
        public Texture(TextureTarget textureTarget) : base(GL.GenTexture())
        {
            TextureTarget = textureTarget;

            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;
            MinFilter = TextureMinFilter.LinearMipmapLinear;
            MagFilter = TextureMagFilter.Linear;
        }

        /// <summary>
        /// Binds the Id to <see cref="TextureTarget"/>.
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(TextureTarget, Id);
        }

        private void SetTexParameter(TextureParameterName param, int value)
        {
            Bind();
            GL.TexParameter(TextureTarget, param, value);
        }
    }
}
