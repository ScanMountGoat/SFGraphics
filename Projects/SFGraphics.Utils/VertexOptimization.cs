using System.Collections.Generic;

namespace SFGraphics.Utils
{
    /// <summary>
    /// Contains methods for simplifying duplicate vertex data.
    /// </summary>
    public static class VertexOptimization
    {
        /// <summary>
        /// Generates new vertices and indices with no repeated vertices.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertices">The original vertex list</param>
        /// <param name="newVertices">The optimized list of vertices</param>
        /// <param name="newIndices">The optimized list of indices</param>
        public static void OptimizeVertexData<T>(IList<T> vertices, out List<T> newVertices, out List<int> newIndices)
        {
            var indexByVertex = new Dictionary<T, int>();
            int maxIndex = 0;

            newVertices = new List<T>();

            // Only add an index and vertex for new vertices.
            foreach (var vertex in vertices)
            {
                if (!indexByVertex.ContainsKey(vertex))
                {
                    indexByVertex[vertex] = maxIndex;
                    newVertices.Add(vertex);
                    maxIndex++;
                }
            }

            // Use the optimized vertex list to generate new indices.
            newIndices = new List<int>();
            foreach (var vertex in vertices)
            {
                newIndices.Add(indexByVertex[vertex]);
            }
        }


        /// <summary>
        /// Generates new vertices and indices with no repeated vertices.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertices">The original vertex list</param>
        /// <param name="indices">The original vertex indices</param>
        /// <param name="newVertices">The optimized list of vertices</param>
        /// <param name="newIndices">The optimized list of indices</param>
        public static void OptimizeVertexData<T>(IList<T> vertices, IList<int> indices, out List<T> newVertices, out List<int> newIndices)
        {
            var indexByVertex = new Dictionary<T, int>();
            int maxIndex = 0;

            newVertices = new List<T>();

            // Only add an index and vertex for new vertices.
            foreach (var index in indices)
            {
                if (!indexByVertex.ContainsKey(vertices[index]))
                {
                    indexByVertex[vertices[index]] = maxIndex;
                    newVertices.Add(vertices[index]);
                    maxIndex++;
                }
            }

            // Use the optimized vertex list to generate new indices.
            newIndices = new List<int>();
            foreach (var index in indices)
            {
                newIndices.Add(indexByVertex[vertices[index]]);
            }
        }
    }
}
