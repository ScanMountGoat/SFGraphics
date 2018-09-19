using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using System.Collections.Generic;

namespace SFGenericModel
{
    /// <summary>
    /// Creates buffers for indexed vertex data using an interleaved buffer of 
    /// vertex attributes.
    /// </summary>
    /// <typeparam name="T">The struct used for each vertex</typeparam>
    public class IndexedVertexData<T> where T : struct
    {
        /// <summary>
        /// The size of <typeparamref name="T"/> in bytes.
        /// </summary>
        public int VertexSizeInBytes { get; }

        /// <summary>
        /// The number of vertices, which is equal to the number of vertex indices.
        /// </summary>
        public int VertexCount { get; }

        /// <summary>
        /// An interleaved buffer of vertex attributes.
        /// </summary>
        public BufferObject VertexBuffer { get; }

        /// <summary>
        /// The buffer of vertex indices.
        /// </summary>
        public BufferObject VertexIndexBuffer { get; }

        /// <summary>
        /// Determines how primitives will be constructed from the vertex data.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="primitiveType"></param>
        public IndexedVertexData(List<T> vertices, PrimitiveType primitiveType)
        {
            VertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
            VertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);
            PrimitiveType = primitiveType;

            VertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            VertexCount = vertices.Count;

            // Generate a unique index for each vertex.
            // TODO: Generate more optimized indices
            List<int> vertexIndices = GenerateIndices(vertices);
            InitializeBufferData(vertices, vertexIndices);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertexIndices"></param>
        /// <param name="primitiveType"></param>
        public IndexedVertexData(List<T> vertices, List<int> vertexIndices, PrimitiveType primitiveType)
        {
            VertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
            VertexIndexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);
            PrimitiveType = primitiveType;

            VertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            VertexCount = vertexIndices.Count;

            InitializeBufferData(vertices, vertexIndices);
        }

        private void InitializeBufferData(List<T> vertices, List<int> vertexIndices)
        {
            VertexBuffer.SetData(vertices.ToArray(), BufferUsageHint.StaticDraw);
            VertexIndexBuffer.SetData(vertexIndices.ToArray(), BufferUsageHint.StaticDraw);
        }

        private static List<int> GenerateIndices(List<T> vertices)
        {
            // TODO: Generate more optimized indices by looking for duplicate vertices.
            List<int> vertexIndices = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertexIndices.Add(i);
            }

            return vertexIndices;
        }
    }
}
