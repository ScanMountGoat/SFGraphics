using SFGenericModel.VertexAttributes;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Determines how a <see cref="VertexAttribute"/> should be rendered for generated shaders.
    /// </summary>
    public class VertexRenderingAttribute : System.Attribute
    {
        /// <summary>
        /// The name used for rendering.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public ValueCount ValueCount { get; }

        /// <summary>
        /// 
        /// </summary>
        public VertexAttribPointerType Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInteger { get; }

        /// <summary>
        /// Normalize the vector before rendering.
        /// </summary>
        public bool NormalizeVector { get; }

        /// <summary>
        /// Remap values in range [-1, 1] to range [0, 1]. This occurs after normalization.
        /// </summary>
        public bool RemapToVisibleRange { get; }

        /// <summary>
        /// Creates a rendering attribute for a floating point vertex attribute.
        /// </summary>
        /// <param name="name">The name used for rendering</param>
        /// <param name="valueCount"></param>
        /// <param name="type"></param>
        /// <param name="normalizeVector">Normalize vector type values</param>
        /// <param name="remapToVisibleRange">Remap values in range [-1, 1] to range [0, 1]. This occurs after normalization.</param>
        public VertexRenderingAttribute(string name, ValueCount valueCount, VertexAttribPointerType type, bool normalizeVector, bool remapToVisibleRange)
        {
            Name = name;
            ValueCount = valueCount;
            Type = type;
            IsInteger = false;
            NormalizeVector = normalizeVector;
            RemapToVisibleRange = remapToVisibleRange;
        }

        /// <summary>
        /// Creates a rendering attribute for an integer vertex attribute.
        /// </summary>
        /// <param name="name">The name used for rendering</param>
        /// <param name="valueCount"></param>
        /// <param name="type"></param>
        /// <param name="normalizeVector">Normalize vector type values</param>
        /// <param name="remapToVisibleRange">Remap values in range [-1, 1] to range [0, 1]. This occurs after normalization.</param>
        public VertexRenderingAttribute(string name, ValueCount valueCount, VertexAttribIntegerType type, bool normalizeVector, bool remapToVisibleRange)
        {
            Name = name;
            ValueCount = valueCount;
            Type = (VertexAttribPointerType)type;
            IsInteger = true;
            NormalizeVector = normalizeVector;
            RemapToVisibleRange = remapToVisibleRange;
        }
    }
}
