using OpenTK.Graphics.OpenGL;
using System;

namespace SFGraphics.GLObjects.Shaders.ShaderEventArgs
{
    /// <summary>
    /// Contains the data used to set a shader uniform variable.
    /// </summary>
    public class UniformSetEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the uniform variable.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data type of the uniform.
        /// </summary>
        public ActiveUniformType Type { get; set; }

        /// <summary>
        /// The value used to initialize the uniform variable.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The number of components for <see cref="Value"/>.
        /// </summary>
        public int Size { get; set; }
    }
}
