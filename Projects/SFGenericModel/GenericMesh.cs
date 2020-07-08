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
        private static readonly int vertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

        private readonly BufferObject vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);

        /// <summary>
        /// Contains information about the arguments used to set a vertex attribute.
        /// </summary>
        /// <param name="sender">The object generating the event/> 
        /// instance that generated the error</param>
        /// <param name="e">The vertex attribute information</param>
        public delegate void InvalidAttribSetEventHandler(object sender, AttribSetEventArgs e);

        /// <summary>
        /// Occurs when specified vertex attribute information does not match the shader.
        /// </summary>
        public event InvalidAttribSetEventHandler InvalidAttribSet;

        /// <summary>
        /// Invoke the <see cref="InvalidAttribSet"/> event with the specified args.
        /// </summary>
        /// <param name="e">The vertex attribut information</param>
        protected virtual void OnInvalidAttribSet(AttribSetEventArgs e)
        {
            InvalidAttribSet?.Invoke(this, e);
        }

        /// <summary>
        /// Initialized by default using the member attributes of <typeparamref name="T"/>.
        /// </summary>
        protected static List<VertexAttribute> vertexAttributes = VertexAttributeUtils.GetAttributesFromType<T>();

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// An index is generated for each vertex in <paramref name="vertices"/>.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(T[] vertices, PrimitiveType primitiveType) : base(vertices.Length, primitiveType)
        {
            InitializeBufferData(vertices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// Indices in <paramref name="vertexIndices"/> are treated as unsigned integers.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(T[] vertices, int[] vertexIndices, PrimitiveType primitiveType)
            : base((uint[])(object)vertexIndices, primitiveType)
        {
            InitializeBufferData(vertices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="vertexIndices">The vertex index data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from the vertex data</param>
        public GenericMesh(T[] vertices, uint[] vertexIndices, PrimitiveType primitiveType)
            : base(vertexIndices, primitiveType)
        {
            InitializeBufferData(vertices);
        }

        /// <summary>
        /// Creates a new mesh and initializes the vertex buffer data.
        /// </summary>
        /// <param name="vertexData">Contains the vertices, indices, and primitive type</param>
        public GenericMesh(IndexedVertexData<T> vertexData) 
            : this(vertexData.Vertices, vertexData.Indices, vertexData.PrimitiveType)
        {

        }

        /// <summary>
        /// Configures the vertex attributes for members of <typeparamref name="T"/> with <see cref="VertexAttribute"/> attributes.
        /// </summary>
        /// <param name="shader">The shader to query for attribute information</param>
        protected override void ConfigureVertexAttributes(Shader shader)
        {
            shader.EnableVertexAttributes();
            SetVertexAttributes(shader, vertexAttributes);
        }

        private void SetVertexAttributes(Shader shader, IEnumerable<VertexAttribute> attributes)
        {
            vertexBuffer.Bind();

            // Calculating the offset requires the list order to match the struct member order.
            int offset = 0;
            foreach (var attribute in attributes)
            {
                if (!VertexAttributeUtils.SetVertexAttribute(shader, attribute, offset, vertexSizeInBytes))
                    OnInvalidAttribSet(new AttribSetEventArgs(attribute.Name, attribute.Type, attribute.ValueCount));
                offset += attribute.SizeInBytes; 
            }
        }

        private void InitializeBufferData(T[] vertices)
        {
            vertexBuffer.SetData(vertices, BufferUsageHint.StaticDraw);
        }
    }
}
