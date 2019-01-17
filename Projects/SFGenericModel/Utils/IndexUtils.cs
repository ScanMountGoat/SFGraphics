using System.Collections.Generic;

namespace SFGenericModel.Utils
{
    /// <summary>
    /// Contains methods for generating and manipulating vertex indices.
    /// </summary>
    public static class IndexUtils
    {
        /// <summary>
        /// Generates consecutive indices starting with 0 
        /// up to but not including <paramref name="count"/>.
        /// </summary>
        /// <param name="count">The number of indices to generate</param>
        /// <returns>Generated consecutive indices</returns>
        public static List<int> GenerateIndices(int count)
        {
            List<int> vertexIndices = new List<int>();
            for (int i = 0; i < count; i++)
            {
                vertexIndices.Add(i);
            }

            return vertexIndices;
        }

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
        public static void OptimizedVertexData<T>(IList<T> vertices, IList<int> indices, out List<T> newVertices, out List<int> newIndices)
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
