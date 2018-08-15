using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFGraphics.GLObjects.BufferObjects
{
    internal static class BufferObjectExceptionMessages
    {
        public static readonly string subDataTooLong = "The data written to the buffer must not exceed the buffer's capacity.";

        public static readonly string offsetAndItemSizeMustBeNonNegative = "The offset and item size must be non negative.";
    }
}
