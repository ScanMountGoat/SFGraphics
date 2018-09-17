using OpenTK.Graphics.OpenGL;
using SFGenericModel.VertexAttributes;
using System;

namespace SFGenericModel.MeshEventArgs
{
    /// <summary>
    /// Contains the data used to set a shader uniform variable.
    /// </summary>
    public class AttribSetEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the attribute variable.
        /// </summary>
        public string Name { get; }

        public VertexAttribPointerType Type { get; }

        /// <summary>
        /// The number of components. Ex: 1 for <see cref="float"/> or
        /// 4 for Vector4.
        /// </summary>
        public ValueCount ValueCount { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="valueCount"></param>
        public AttribSetEventArgs(string name, VertexAttribPointerType type, ValueCount valueCount)
        {
            Name = name;
            Type = type;
            ValueCount = valueCount;
        }
    }
}
