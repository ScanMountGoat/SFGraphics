using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFGraphics.GLObjects.BufferObjects
{
    internal static class BufferExceptionMessages
    {
        public static readonly string subDataTooLong = "The data read to or written from a buffer " +
            "must not exceed the buffer's capacity.";

        public static readonly string offsetAndItemSizeMustBeNonNegative = "The offset and item size must be non negative.";

        public static readonly string bufferNotDivisibleByRequestedType = "The buffer's size is not divisible by the requested type's size.";
    }
}
