using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// Type information for converting from OpenGL vertex attributes to C#.
    /// </summary>
    public static class AttribPointerUtils
    {
        private static readonly Dictionary<VertexAttribPointerType, int> sizeInBytesByType = new Dictionary<VertexAttribPointerType, int>()
        {
            { VertexAttribPointerType.Byte,          sizeof(byte) },
            { VertexAttribPointerType.UnsignedByte,  sizeof(byte) },
            { VertexAttribPointerType.Short,         sizeof(short) },
            { VertexAttribPointerType.UnsignedShort, sizeof(ushort) },
            { VertexAttribPointerType.Int,           sizeof(int) },
            { VertexAttribPointerType.UnsignedInt,   sizeof(uint) },
            { VertexAttribPointerType.Float,         sizeof(float) },
            { VertexAttribPointerType.Double,        sizeof(double) },
        };

        /// <summary>
        /// Returns the size in bytes of the C# equivalent for a specified OpenGL attribute pointer type.
        /// </summary>
        /// <returns>The size of <paramref name="type"/> in bytes</returns>
        /// <exception cref="System.NotImplementedException">The size of <paramref name="type"/> is not implemented</exception>
        public static int GetSizeInBytes(VertexAttribPointerType type)
        {
            if (sizeInBytesByType.ContainsKey(type))
                return sizeInBytesByType[type];
            else
                throw new System.NotImplementedException($"{type.ToString()} is not a supported type.");
        }

        /// <summary>
        /// Returns the size in bytes of the C# equivalent for a specified OpenGL attribute pointer type.
        /// </summary>
        /// <returns>The size of <paramref name="type"/> in bytes</returns>
        /// <exception cref="System.NotImplementedException">The size of <paramref name="type"/> is not implemented</exception>
        public static int GetSizeInBytes(VertexAttribIntegerType type)
        {
            return GetSizeInBytes((VertexAttribPointerType)type);
        }
    }
}
