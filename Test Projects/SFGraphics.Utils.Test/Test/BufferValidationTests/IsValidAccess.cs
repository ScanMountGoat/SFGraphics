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
    }
}
