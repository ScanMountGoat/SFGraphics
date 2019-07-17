using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.VertexAttributes
{
    /// <summary>
    /// The number of vector components for a vertex attribute. Scalars should use <see cref="ValueCount.One"/>.
    /// </summary>
    public enum ValueCount
    {
        /// <summary>
        /// A scalar value
        /// </summary>
        One = 1,

        /// <summary>
        /// A two component vector value
        /// </summary>
        Two = 2,

        /// <summary>
        /// A three component vector value
        /// </summary>
        Three = 3,

        /// <summary>
        /// A four component vector value
        /// </summary>
        Four = 4
    }

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
        /// The attribute will also be used as the vertex UVs.
        /// </summary>
        TexCoord0
    }

    /// <summary>
    /// Stores information for a vertex attribute variable.
    /// </summary>
    public abstract class VertexAttribute : System.Attribute
    {
        /// <summary>
        /// The name of the attribute in the shader.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The number of components. Ex: 1 for <see cref="float"/> or
        /// 4 for Vector4.
        /// </summary>
        public ValueCount ValueCount { get; }

        /// <summary>
        /// The total size of the attribute's data in bytes.
        /// </summary>
        public int SizeInBytes { get; protected set; }

        /// <summary>
        /// The data type of the attribute value.
        /// </summary>
        public VertexAttribPointerType Type { get; }

        /// <summary>
        /// How the attribute should be used for generated shaders.
        /// </summary>
        public AttributeUsage AttributeUsage { get; }

        /// <summary>
        /// Normalize the vector before rendering.
        /// </summary>
        public bool NormalizeVector { get; }

        /// <summary>
        /// Remap values in range [-1, 1] to range [0, 1]. This occurs after normalization.
        /// </summary>
        public bool RemapToVisibleRange { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the attribute in the shader</param>
        /// <param name="valueCount">The number of components</param>
        /// <param name="type">The data type</param>
        /// <param name="attributeUsage">How the attribute will be rendered</param>
        /// <param name="normalizeVector">Normalize the vector before rendering</param>
        /// <param name="remapToVisibleRange">Remap the vector before rendering</param>
        protected VertexAttribute(string name, ValueCount valueCount, VertexAttribPointerType type, AttributeUsage attributeUsage, bool normalizeVector, bool remapToVisibleRange)
        {
            Name = name;
            ValueCount = valueCount;
            Type = type;
            AttributeUsage = attributeUsage;
            NormalizeVector = normalizeVector;
            RemapToVisibleRange = remapToVisibleRange;
        }

        /// <summary>
        /// Configures the vertex attribute for the currently bound array buffer.
        /// </summary>
        /// <param name="index">The index of the attribute variable in the shader</param>
        /// <param name="strideInBytes">The vertex size in bytes</param>
        /// <param name="offsetInBytes">The offset in bytes of the attribute in the vertex</param>
        public abstract void SetVertexAttribute(int index, int strideInBytes, int offsetInBytes);
    }
}
