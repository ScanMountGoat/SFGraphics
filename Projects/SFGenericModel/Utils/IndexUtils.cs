using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFGenericModel.Utils
{
    /// <summary>
    /// Contains methods for generating and manipulating vertex indices.
    /// </summary>
    public static class IndexUtils
    {
        /// <summary>
        /// Generates a unique index for each vertex in <paramref name="vertices"/>
        /// starting with <c>0</c>.
        /// </summary>
        /// <typeparam name="T">The vertex struct</typeparam>
        /// <param name="vertices">The vertices used to generate indices</param>
        /// <returns></returns>
        public static List<int> GenerateIndices<T>(List<T> vertices) where T : struct
        {
            List<int> vertexIndices = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertexIndices.Add(i);
            }

            return vertexIndices;
        }
    }
}
