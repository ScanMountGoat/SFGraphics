using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BufferObjectTests
{
    [TestClass]
    public class GetSubData : BufferTest
    {
        [TestMethod]
        public void GetBufferSubDataValidRead()
        {
            // Read at index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            float[] bufferData = buffer.GetSubData<float>(offset, 1);

            Assert.AreEqual(1, bufferData.Length);
            Assert.AreEqual(2.5f, bufferData[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetBufferSubDataNegativeOffset()
        {
            float[] bufferData = buffer.GetSubData<float>(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetBufferSubDataNegativeItemCount()
        {
            float[] bufferData = buffer.GetSubData<float>(0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetBufferSubDataExceedsBufferSize()
        {
            // Try to read one element beyond the buffer's capacity.
            float[] bufferData = buffer.GetSubData<float>(0, originalData.Length + 1);
        }
    }
}
