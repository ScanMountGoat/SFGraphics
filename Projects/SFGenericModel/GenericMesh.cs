using OpenTK.Graphics.OpenGL;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.Utils;
using SFGenericModel.VertexAttributes;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.VertexArrays;
using System.Collections.Generic;
using System.Linq;

namespace SFGenericModel
{
    /// <summary>
    /// A class for drawing indexed, generic vertex data using a user defined 
    /// vertex struct <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The struct used to define vertex data</typeparam>
    public abstract class GenericMesh<T> : IDrawableMesh where T : struct
    {
        /// <summary>
        /// The number of vertices stored in the buffers used for drawing.
        /// </summary>
        public int VertexCount { get; }

        /// <summary>
        /// Determines how primitives will be constructed from the vertex data.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Specifies the data type of the index values.
        /// </summary>
        public DrawElementsType DrawElementsType { get; }

        /// <summary>
        /// Contains information about the arguments used to set a vertex attribute.
        /// </summary>
        /// <param name="sender">The <see cref="GenericMesh{T}"/> 
        /// instance that generated the error</param>
        /// <param name="e">The vertex attribute information</param>
        public delegate void InvalidAttribSetEventHandler(object sender, AttribSetEventArgs e);

        /// <summary>
        /// Occurs when specified vertex attribute information does not match the shader.
        /// </summary>
        public event InvalidAttribSetEventHandler OnInvalidAttribSet;

        // Vertex attributes only need to be reconfigured when the shader changes.
        private int previousShaderId = -1;

        // Used for attribute offset calculation.
        private readonly int vertexSizeInBytes;

        private readonly VertexArrayObject vertexArrayObject = new VertexArrayObject();

        // Vertex and index data.
        private readonly BufferObject vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
        private readonly BufferObject vertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);

        /// <summary>
        /// Initialized by default using the member attributes of <typeparamref name="T"/>.
        /// </summary>
        protected static List<VertexAttribute> vertexAttributes = VertexAttributeUtils.GetAttributesFromType<T>();

        private GenericMesh(PrimitiveType primitiveType, DrawElementsType drawElementsType, System.Type vertexType, int vertexCount)
        {
            PrimitiveType = primitiveType;
            DrawElementsType = drawElementsType;

            // This works as expected as long as only value types are used.
            vertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(vertexType);
            VertexCount = vertexCount;
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// An index is generated for each vertex in <paramref name="vertices"/>.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        protected GenericMesh(ICollection<T> vertices, PrimitiveType primitiveType) 
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertices.Count)
        {
            InitializeBufferData(vertices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        protected GenericMesh(IList<T> vertices, IList<int> vertexIndices, PrimitiveType primitiveType)
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertexIndices.Count)
        {
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        protected GenericMesh(IList<T> vertices, IList<uint> vertexIndices, PrimitiveType primitiveType)
            : this(primitiveType, DrawElementsType.UnsignedInt, typeof(T), vertexIndices.Count)
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
        /// Draws the geometry data.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        /// <param name="count">The number of vertices to draw</param>
        /// <param name="offset">The offset into the index buffer</param>
        public void Draw(Shader shader, int count, int offset)
        {
            if (!shader.LinkStatusIsOk)
                return;

            shader.UseProgram();

            // Only update when the shader changes.
            if (shader.Id != previousShaderId)
            {
                ConfigureVertexAttributes(shader);
                previousShaderId = shader.Id;
            }

            DrawGeometry(count, offset);
        }

        /// <summary>
        /// Draws the geometry data.
        /// </summary>
        /// <param name="shader">The shader used for drawing</param>
        public void Draw(Shader shader)
        {
            Draw(shader, VertexCount, 0);
        }

        private void ConfigureVertexAttributes(Shader shader)
        {
            // The binding order here is critical.
            vertexArrayObject.Bind();

            vertexBuffer.Bind();
            vertexIndexBuffer.Bind();

            shader.EnableVertexAttributes();
            SetVertexAttributes(shader, vertexAttributes);

            // TODO: Binding the default VAO isn't part of the core specification.
            vertexArrayObject.Unbind();

            // Unbind all the buffers.
            // This step may not be necessary.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void DrawGeometry(int count, int offset)
        {
            vertexArrayObject.Bind();
            GL.DrawElements(PrimitiveType, count, DrawElementsType, offset);

            // TODO: This isn't part of the OpenGL core specification.
            // Leave this enabled for compatibility with older applications.
            vertexArrayObject.Unbind();
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

        private void InitializeBufferData<TIndex>(IEnumerable<T> vertices, IEnumerable<TIndex> vertexIndices) where TIndex : struct
        {
            // TODO: Forcing the parameters to be arrays will avoid copying the data an additional time.
            vertexBuffer.SetData(vertices.ToArray(), BufferUsageHint.StaticDraw);
            vertexIndexBuffer.SetData(vertexIndices.ToArray(), BufferUsageHint.StaticDraw);
        }

        private void InitializeBufferData(ICollection<T> vertices)
        {
            var indices = IndexUtils.GenerateIndices(vertices.Count);
            InitializeBufferData(vertices, indices);
        }
    }
}
