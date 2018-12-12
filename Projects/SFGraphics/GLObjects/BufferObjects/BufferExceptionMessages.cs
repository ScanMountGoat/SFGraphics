namespace SFGraphics.GLObjects.BufferObjects
{
    internal static class BufferExceptionMessages
    {
        public static readonly string subDataTooLong = "The data read from or written to a buffer " +
            "must not exceed the buffer's capacity.";

        public static readonly string offsetMustBeNonNegative = "The offset must be non negative.";

        public static readonly string bufferNotDivisibleByRequestedType = "The buffer's size is not divisible by the requested type's size.";
    }
}
