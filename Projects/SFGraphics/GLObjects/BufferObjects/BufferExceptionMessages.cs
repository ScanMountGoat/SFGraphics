namespace SFGraphics.GLObjects.BufferObjects
{
    internal static class BufferExceptionMessages
    {
        public static readonly string outOfRange = "The data read from or written to a buffer must not exceed the buffer's storage.";

        public static readonly string bufferNotDivisibleByRequestedType = "The buffer data is not divisible by the requested type's size.";
    }
}
