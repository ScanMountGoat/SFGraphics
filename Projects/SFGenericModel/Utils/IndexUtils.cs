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
        public static int[] GenerateIndices(int count)
        {
            var result = new int[count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = i;
            }
            return result;
        }
    }
}
