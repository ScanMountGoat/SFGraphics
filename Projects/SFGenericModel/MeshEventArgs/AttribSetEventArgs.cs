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

        /// <summary>
        /// The data type of the attribute variable
        /// </summary>
        public VertexAttribPointerType Type { get; }

        /// <summary>
        /// The number of components. Ex: 1 for <see cref="float"/> or
        /// 4 for <see cref="OpenTK.Vector4"/>.
        /// </summary>
        public ValueCount ValueCount { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="type">The data type of the attribute</param>
        /// <param name="valueCount">The number of vector components or 1 for scalars</param>
        public AttribSetEventArgs(string name, VertexAttribPointerType type, ValueCount valueCount)
        {
            Name = name;
            Type = type;
            ValueCount = valueCount;
        }
    }
}
