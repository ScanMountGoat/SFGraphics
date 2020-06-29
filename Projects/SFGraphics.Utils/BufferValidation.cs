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
            // Check for negatives to ensure there isn't any sign weirdness happening on the final return statement.
            if (offsetInBytes < 0 || strideInBytes < 0 || elementCount < 0 || bufferCapacityInBytes < 0)
                return false;

            return (offsetInBytes + (strideInBytes * elementCount)) <= bufferCapacityInBytes;
        }
    }
}
