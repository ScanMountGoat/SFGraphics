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
        /// The <see cref="TextureTarget"/> used for all GL functions.
        /// </summary>
        protected TextureTarget textureTarget = TextureTarget.Texture2D;

        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            get { return minFilter; }
            set
            {
                Bind();
                minFilter = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)value);
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
                Bind();
                magFilter = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)value);
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
                Bind();
                textureWrapS = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)value);
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
                Bind();
                textureWrapT = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)value);
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
                Bind();
                textureWrapR = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (int)value);
            }
        }
        private TextureWrapMode textureWrapR;

        /// <summary>
        /// Creates an empty texture of the specified target and internal format.
        /// </summary>
        /// <param name="target">The target to which <see cref="GLObject.Id"/> is bound.</param>
        public Texture(TextureTarget target) : base(GL.GenTexture())
        {
            textureTarget = target;

            // Use properties because the GL texture needs to be updated.
            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;
            MinFilter = TextureMinFilter.LinearMipmapLinear;
            MagFilter = TextureMagFilter.Linear;
        }

        /// <summary>
        /// Binds the Id to <see cref="textureTarget"/>.
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(textureTarget, Id);
        }
    }
}
