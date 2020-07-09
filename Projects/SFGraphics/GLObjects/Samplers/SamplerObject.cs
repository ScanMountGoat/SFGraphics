using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Samplers
{
    /// <summary>
    /// Allows for setting texture state per texture unit rather than per texture.
    /// The texture parameters of the bound sampler object are used instead of the texture's parameters.
    /// </summary>
    public sealed class SamplerObject : GLObject
    {
        internal override GLObjectType ObjectType => GLObjectType.SamplerObject;

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public TextureMinFilter MinFilter { set => SetSamplerParameter(SamplerParameterName.TextureMagFilter, (int)value); }

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public TextureMagFilter MagFilter { set => SetSamplerParameter(SamplerParameterName.TextureMagFilter, (int)value); }

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public TextureWrapMode TextureWrapS { set => SetSamplerParameter(SamplerParameterName.TextureWrapS, (int)value); }

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public TextureWrapMode TextureWrapT { set => SetSamplerParameter(SamplerParameterName.TextureWrapT, (int)value); }

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public TextureWrapMode TextureWrapR { set => SetSamplerParameter(SamplerParameterName.TextureWrapR, (int)value); }

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public float TextureMaxAnisotropy { set => SetSamplerParameter(SamplerParameterName.TextureMaxAnisotropyExt, value); }

        /// <summary>
        /// Updates the SamplerParameter.
        /// </summary>
        public float TextureLodBias { set => SetSamplerParameter(SamplerParameterName.TextureLodBias, value); }

        /// <summary>
        /// Creates an unitialized sampler.
        /// </summary>
        public SamplerObject() : base(GL.GenSampler())
        {

        }

        /// <summary>
        /// Binds the Id to <paramref name="textureUnit"/>.
        /// This sampler's state will override the state of the bound texture.
        /// </summary>
        /// <param name="textureUnit">The target texture unit for binding</param>
        public void Bind(int textureUnit)
        {
            GL.BindSampler(textureUnit, Id);
        }

        private void SetSamplerParameter(SamplerParameterName parameter, int value)
        {
            GL.SamplerParameter(Id, parameter, value);
        }

        private void SetSamplerParameter(SamplerParameterName parameter, float value)
        {
            GL.SamplerParameter(Id, parameter, value);
        }
    }
}
