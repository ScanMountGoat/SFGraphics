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
    }
}
