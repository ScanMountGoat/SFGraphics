using System.Collections.Generic;
using System.Linq;

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
            return Enumerable.Range(0, count).ToList();
        }
    }
}
