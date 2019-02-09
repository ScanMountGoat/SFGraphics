using SFGenericModel.VertexAttributes;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.ShaderGenerators
{
    /// <summary>
    /// Determines the additional usage for the generated shader.
    /// </summary>
    public enum AttributeUsage
    {
        /// <summary>
        /// The attribute will only be used for the render modes.
        /// </summary>
        Default,

        /// <summary>
        /// The attribute will also be used as the vertex positions.
        /// </summary>
        Position,

        /// <summary>
        /// The attribute will also be used as the vertex normals.
        /// </summary>
        Normal,

        /// <summary>
        /// The attribute will also be used as the vertex tangents.
        /// </summary>
        Tangent,

        /// <summary>
        /// The attribute will also be used as the vertex bitangents.
        /// </summary>
        Bitangent,

        /// <summary>
        /// The attribute will also be used as the vertex UVs.
        /// </summary>
        TexCoord0
    }

    /// <summary>
    /// Determines how a <see cref="VertexAttribute"/> should be rendered for generated shaders.
    /// </summary>
    public class VertexRenderingAttribute : VertexAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public AttributeUsage AttributeUsage { get; }

        /// <summary>
        /// TODO: fix this comment
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
        /// <param name="valueCount">The number of vector components for the vertex attribute</param>
        /// <param name="type">The data type of the attribute variable</param>
        /// <param name="usage"></param>
        /// <param name="normalizeVector">Normalize vector type values</param>
        /// <param name="remapToVisibleRange">Remap values in range [-1, 1] to range [0, 1]. This occurs after normalization.</param>
        public VertexRenderingAttribute(string name, ValueCount valueCount, 
            VertexAttribPointerType type, AttributeUsage usage, bool normalizeVector, bool remapToVisibleRange) 
            : base(name, valueCount, type)
        {
            IsInteger = false;
            NormalizeVector = normalizeVector;
            AttributeUsage = usage;
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
        public VertexRenderingAttribute(string name, ValueCount valueCount, VertexAttribIntegerType type, 
            bool normalizeVector, bool remapToVisibleRange) : base(name, valueCount, (VertexAttribPointerType)type)
        {
            // TODO: Attribute usage for integers?
            IsInteger = true;
            NormalizeVector = normalizeVector;
            RemapToVisibleRange = remapToVisibleRange;
        }

        public override void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes)
        {
            // TODO: Move this functionality to an interface.
            throw new System.NotImplementedException();
        }
    }
}
