using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Samplers
{
    /// <summary>
    /// Allows for setting texture state per texture unit rather than per texture.
    /// The texture parameters of the bound sampler object are used instead of the texture's parameters.
    /// </summary>
    public sealed class SamplerObject : GLObject
    {
        /// <summary>
        /// Returns the type of OpenGL object. Used for memory management.
        /// </summary>
        public override GLObjectType ObjectType { get { return GLObjectType.SamplerObject; } }

        /// <summary>
        /// Updates the SamplerParameter when set.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            get { return minFilter; }
            set
            {
                minFilter = value;
                SetSamplerParameter(SamplerParameterName.TextureMinFilter, (int)value);
            }
        }
        private TextureMinFilter minFilter;

        /// <summary>
        /// Updates the SamplerParameter when set.
        /// </summary>
        public TextureMagFilter MagFilter
        {
            get { return magFilter; }
            set
            {
                magFilter = value;
                SetSamplerParameter(SamplerParameterName.TextureMagFilter, (int)value);
            }
        }
        private TextureMagFilter magFilter;

        /// <summary>
        /// Updates the SamplerParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapS
        {
            get { return textureWrapS; }
            set
            {
                textureWrapS = value;
                SetSamplerParameter(SamplerParameterName.TextureWrapS, (int)value);
            }
        }
        private TextureWrapMode textureWrapS;

        /// <summary>
        /// Updates the SamplerParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapT
        {
            get { return textureWrapT; }
            set
            {
                textureWrapT = value;
                SetSamplerParameter(SamplerParameterName.TextureWrapT, (int)value);
            }
        }
        private TextureWrapMode textureWrapT;

        /// <summary>
        /// Updates the SamplerParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapR
        {
            get { return textureWrapR; }
            set
            {
                textureWrapR = value;
                SetSamplerParameter(SamplerParameterName.TextureWrapR, (int)value);
            }
        }
        private TextureWrapMode textureWrapR;

        /// <summary>
        /// Creates an unitialized sampler object. 
        /// </summary>
        public SamplerObject() : base(GL.GenSampler())
        {

        }

        /// <summary>
        /// Binds the Id to <paramref name="textureUnit"/>.
        /// This sampler's state will override the state of the bound texture.
        /// </summary>
        /// <param name="textureUnit"></param>
        public void Bind(int textureUnit)
        {
            GL.BindSampler(textureUnit, Id);
        }

        private void SetSamplerParameter(SamplerParameterName parameter, int value)
        {
            GL.SamplerParameter(Id, parameter, value);
        }
    }
}
