using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// Type information for converting from OpenGL vertex attributes to C#.
    /// </summary>
    public static class AttribPointerUtils
    {
        /// <summary>
        /// The size in bytes of the C# equivalent for a specified OpenGL attribute pointer type.
        /// </summary>
        public static readonly Dictionary<VertexAttribPointerType, int> sizeInBytesByType = new Dictionary<VertexAttribPointerType, int>()
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
    }
}
