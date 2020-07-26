using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFGraphics.Utils.Test.BufferValidationTests
{
    [TestClass]
    public class IsValidAccess
    {
        [TestMethod]
        public void ValidWriteEmptyBuffer()
        {
            Assert.IsTrue(BufferValidation.IsValidAccess(0, 0, 0, 0));
        }

        [TestMethod]
        public void NegativeOffset()
        {
            Assert.IsFalse(BufferValidation.IsValidAccess(-4, 4, 4, 32));
        }

        [TestMethod]
        public void ReadWholeBuffer()
        {
            Assert.IsTrue(BufferValidation.IsValidAccess(0, 4, 4, 16));
        }

        [TestMethod]
        public void ReadHalfBuffer()
        {
            Assert.IsTrue(BufferValidation.IsValidAccess(8, 4, 2, 16));
        }


        [TestMethod]
        public void ReadOnePastEnd()
        {
            Assert.IsFalse(BufferValidation.IsValidAccess(1, 4, 4, 16));
        }

        [TestMethod]
        public void NegativeStride()
        {
            Assert.IsFalse(BufferValidation.IsValidAccess(0, -4, 4, 16));
        }

        [TestMethod]
        public void NegativeLength()
        {
            // This may work for a naive implementation.
            Assert.IsFalse(BufferValidation.IsValidAccess(0, -4, 4, -16));
        }

        [TestMethod]
        public void NegativeElementCount()
        {
            // This may work for a naive implementation.
            Assert.IsFalse(BufferValidation.IsValidAccess(0, 4, -4, -16));
        }

        [TestMethod]
        public void NegativeElementCountNegativeStride()
        {
            // This may work for a naive implementation.
            Assert.IsFalse(BufferValidation.IsValidAccess(0, -4, -4, 16));
        }

        [TestMethod]
        public void ElementSizeGreaterThanStride()
        {
            // The element size is valid but the stride is not.
            Assert.IsFalse(BufferValidation.IsValidAccess(0, 3, 4, 4, 16));
        }

        [TestMethod]
        public void ElementSizeLessThanStride()
        {
            // The difference between stride and element size is just padding and won't actually be read for the last element.
            // Read: element + pad 1 + element + pad 1 + element + pad 1 + element + *skip padding* = 19 bytes
            Assert.IsTrue(BufferValidation.IsValidAccess(0, 5, 4, 4, 19));
        }

        [TestMethod]
        public void NegativeElementSize()
        {
            Assert.IsFalse(BufferValidation.IsValidAccess(0, 4, -4, 4, 16));
        }
    }
}
