using OpenTK.Graphics.OpenGL;
using System;

namespace SFGraphics.GLObjects.Shaders.ShaderEventArgs
{
    /// <summary>
    /// Contains the data used to set a shader sampler uniform variable.
    /// </summary>
    public class TextureSetEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the uniform variable.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data type of the texture uniform.
        /// </summary>
        public ActiveUniformType Type { get; set; }

        /// <summary>
        /// The texture used to initialize the uniform variable.
        /// </summary>
        public Textures.Texture Value { get; set; }

        /// <summary>
        /// The texture unit to which <see cref="Value"/> was bound.
        /// </summary>
        public int TextureUnit { get; set; }
    }
}
