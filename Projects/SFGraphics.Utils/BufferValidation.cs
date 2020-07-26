namespace SFGraphics.Utils
{
    public static class BufferValidation
    {
        /// <summary>
        /// Validates accesses to regions of a portion of contiguous memory.
        /// </summary>
        /// <param name="offsetInBytes">The start position for the access in bytes</param>
        /// <param name="strideInBytes">The stride in bytes or the size of each element if tightly packed</param>
        /// <param name="elementCount">The number of elements to access</param>
        /// <param name="bufferCapacityInBytes">The buffer's total storage amount in bytes</param>
        /// <returns><c>true</c> if the access will not exceed the buffer's storage</returns>
        public static bool IsValidAccess(int offsetInBytes, int strideInBytes, int elementCount, int bufferCapacityInBytes)
        {
            return IsValidAccess(offsetInBytes, strideInBytes, strideInBytes, elementCount, bufferCapacityInBytes);
        }

        /// <summary>
        /// Validates accesses to regions of a portion of contiguous memory.
        /// <para></para><para></para>
        /// If <paramref name="strideInBytes"/> bytes are read for each element, use <see cref="IsValidAccess(int, int, int, int)"/>
        /// or set <paramref name="elementSizeInBytes"/> to <paramref name="strideInBytes"/>.
        /// </summary>
        /// <param name="offsetInBytes">The start position for the access in bytes</param>
        /// <param name="strideInBytes">The stride in bytes or the size of each element if tightly packed</param>
        /// <param name="elementSizeInBytes">The size of each element in bytes</param>
        /// <param name="elementCount">The number of elements to access</param>
        /// <param name="bufferCapacityInBytes">The buffer's total storage amount in bytes</param>
        /// <returns><c>true</c> if the access will not exceed the buffer's storage</returns>
        public static bool IsValidAccess(int offsetInBytes, int strideInBytes, int elementSizeInBytes, int elementCount, int bufferCapacityInBytes)
        {
            // Check for negatives to ensure there isn't any sign weirdness happening on the final return statement.
            if (offsetInBytes < 0 || strideInBytes < 0 || elementCount < 0 || bufferCapacityInBytes < 0 || elementSizeInBytes < 0)
                return false;

            // The bytes read shouldn't exceed the stride.
            if (elementSizeInBytes > strideInBytes)
                return false;

            // Accesses will ignore padding on the last element.
            var padding = strideInBytes - elementSizeInBytes;
            var bytesAccessed = (strideInBytes * elementCount) - padding;
            return (offsetInBytes + bytesAccessed) <= bufferCapacityInBytes;
        }
    }
}
