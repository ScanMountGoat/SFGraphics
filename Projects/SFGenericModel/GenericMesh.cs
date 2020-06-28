using OpenTK.Graphics.OpenGL;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.Utils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

namespace SFGenericModel
{
    /// <summary>
    /// A class for drawing indexed, interleaved vertex data using a user defined 
    /// vertex struct <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The struct used to define vertex data</typeparam>
    public abstract class GenericMesh<T> : GenericMeshBase where T : struct
    {
        // Used for attribute offset calculation.
        private readonly int vertexSizeInBytes;

        // Vertex and index data.
        private readonly BufferObject vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
        private readonly BufferObject vertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);

        /// <summary>
        /// Contains information about the arguments used to set a vertex attribute.
        /// </summary>
        /// <param name="sender">The <see cref=""/> 
        /// instance that generated the error</param>
        /// <param name="e">The vertex attribute information</param>
        public delegate void InvalidAttribSetEventHandler(object sender, AttribSetEventArgs e);

        /// <summary>
        /// Occurs when specified vertex attribute information does not match the shader.
        /// </summary>
        public event InvalidAttribSetEventHandler OnInvalidAttribSet;

        /// <summary>
        /// Initialized by default using the member attributes of <typeparamref name="T"/>.
        /// </summary>
        protected static List<VertexAttribute> vertexAttributes = VertexAttributeUtils.GetAttributesFromType<T>();

        private GenericMesh(PrimitiveType primitiveType, DrawElementsType drawElementsType, System.Type vertexType, int vertexCount) : base(primitiveType, drawElementsType, vertexCount)
        {
            vertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(vertexType);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// An index is generated for each vertex in <paramref name="vertices"/>.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        protected GenericMesh(T[] vertices, PrimitiveType primitiveType) 
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertices.Length)
        {
            InitializeBufferData(vertices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        protected GenericMesh(T[] vertices, int[] vertexIndices, PrimitiveType primitiveType)
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertexIndices.Length)
        {
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        protected GenericMesh(T[] vertices, uint[] vertexIndices, PrimitiveType primitiveType)
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertexIndices.Length)
        {
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertexData">Contains the vertices, indices, and primitive type</param>
        protected GenericMesh(IndexedVertexData<T> vertexData) 
            : this(vertexData.Vertices, vertexData.Indices, vertexData.PrimitiveType)
        {

        }

        /// <summary>
        /// Configures the vertex attributes for members of <typeparamref name="T"/> with <see cref="VertexAttribute"/> attributes.
        /// </summary>
        /// <param name="shader">The shader to query for attribute information</param>
        protected override void ConfigureVertexAttributes(Shader shader)
        {
            vertexBuffer.Bind();
            vertexIndexBuffer.Bind();

            shader.EnableVertexAttributes();
            SetVertexAttributes(shader, vertexAttributes);
        }

        private void SetVertexAttributes(Shader shader, IEnumerable<VertexAttribute> attributes)
        {
            // Calculating the offset requires the list order to match the struct member order.
            int offset = 0;
            foreach (var attribute in attributes)
            {
                if (!VertexAttributeUtils.SetVertexAttribute(shader, attribute, offset, vertexSizeInBytes))
                    OnInvalidAttribSet?.Invoke(this, new AttribSetEventArgs(attribute.Name, attribute.Type, attribute.ValueCount));
                offset += attribute.SizeInBytes; 
            }
        }

        private void InitializeBufferData<TIndex>(T[] vertices, TIndex[] vertexIndices) where TIndex : struct
        {
            // TODO: Forcing the parameters to be arrays will avoid copying the data an additional time.
            vertexBuffer.SetData(vertices, BufferUsageHint.StaticDraw);
            vertexIndexBuffer.SetData(vertexIndices, BufferUsageHint.StaticDraw);
        }

        private void InitializeBufferData(T[] vertices)
        {
            var indices = IndexUtils.GenerateIndices(vertices.Length);
            InitializeBufferData(vertices, indices);
        }
    }
}
