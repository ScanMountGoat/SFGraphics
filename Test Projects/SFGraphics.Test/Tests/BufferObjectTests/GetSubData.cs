using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Test.BufferObjectTests
{
    [TestClass]
    public class GetSubData : BufferTest
    {
        [TestMethod]
        public void ValidRead()
        {
            // Read at index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            float[] bufferData = buffer.GetSubData<float>(offset, 1);

            Assert.AreEqual(1, bufferData.Length);
            Assert.AreEqual(2.5f, bufferData[0]);
        }

        [TestMethod]
        public void NegativeOffset()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
                buffer.GetSubData<float>(-1, 1));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer " +
            "must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void GetBufferSubDataNegativeItemCount()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.GetSubData<float>(0, -1));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer " +
            "must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void GetBufferSubDataExceedsBufferSize()
        {
            // Try to read one element beyond the buffer's capacity.
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
                buffer.GetSubData<float>(0, originalData.Length + 1));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer " +
            "must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void DataSizeNotDivisibleByType()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() => buffer.GetSubData<Vector4>(0, 1));
            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer " +
            "must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void DataSizeDivisibleByTypeBecauseOfOffset()
        {
            // The buffer has three floats.
            CollectionAssert.AreEqual(new Vector2[] { new Vector2(2.5f, 3.5f) }, buffer.GetSubData<Vector2>(sizeof(float), 1));
        }
    }
}
