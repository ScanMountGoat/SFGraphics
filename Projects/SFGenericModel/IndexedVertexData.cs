using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace SFGenericModel
{
    /// <summary>
    /// Stores indexed vertex data to be used for drawing with OpenGL.
    /// </summary>
    /// <typeparam name="T">The struct used for each vertex</typeparam>
    public class IndexedVertexData<T> where T : struct
    {
        /// <summary>
        /// The size of <typeparamref name="T"/> in bytes.
        /// </summary>
        public int VertexSizeInBytes { get; }

        /// <summary>
        /// The vertex data.
        /// </summary>
        public List<T> Vertices { get; }

        /// <summary>
        /// The vertex indices. The number of vertices rendered is the number of indices.
        /// </summary>
        public List<int> Indices { get; }

        /// <summary>
        /// Determines how primitives will be constructed from the vertex data.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Creates a new vertex data container with generated indices.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from vertex data</param>
        public IndexedVertexData(List<T> vertices, PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
            Vertices = vertices;
            VertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            // TODO: Generate more optimized indices
            Indices = Utils.IndexUtils.GenerateIndices(vertices.Count);
        }

        /// <summary>
        /// Creates a new vertex data container.
        /// </summary>
        /// <param name="vertices">The vertex data</param>
        /// <param name="indices">The vertex indices</param>
        /// <param name="primitiveType">Determines how primitives will be constructed from vertex data</param>
        public IndexedVertexData(List<T> vertices, List<int> indices, PrimitiveType primitiveType)
        {
            Vertices = vertices;
            Indices = indices;
            PrimitiveType = primitiveType;
            VertexSizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
        }
    }
}
