﻿using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributes
{           
    /// <summary>
    /// A floating point vertex attribute. Integer types are converted directly to floats.
    /// </summary>
    public sealed class VertexFloatAttribute : VertexAttribute
    {
        /// <summary>
        /// When <c>true</c>, integer values are converted to floats. Ex: <c>float = int / INT_MAX</c>
        /// </summary>
        public bool Normalized { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components for the value</param>
        /// <param name="type">The data type of the value</param>
        /// <exception cref="System.NotImplementedException"><paramref name="type"/> is not an implemented attribute type.</exception>
        public VertexFloatAttribute(string name, ValueCount valueCount, VertexAttribPointerType type, bool normalized) : base(name, valueCount, type)
        {
            Normalized = normalized;
            SizeInBytes = (int)valueCount * AttribPointerUtils.GetSizeInBytes(type);
        }

        /// <summary>
        /// Configures the vertex attribute for the currently bound array buffer.
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset in bytes of the attribute in the vertex</param>
        public override void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes)
        {
            GL.VertexAttribPointer(index, (int)ValueCount, Type, Normalized, strideInBytes, offsetInBytes);
        }
    }
}
