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
        public string Name { get; }

        /// <summary>
        /// The data type of the uniform.
        /// </summary>
        public ActiveUniformType Type { get; }

        /// <summary>
        /// The value used to initialize the uniform variable.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// The number of components for <see cref="Value"/>.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        public UniformSetEventArgs(string name, ActiveUniformType type, object value, int size)
        {
            Name = name;
            Type = type;
            Value = value;
            Size = size;
        }
    }
}
