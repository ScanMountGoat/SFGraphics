using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BufferObjectTests
{
    [TestClass]
    public class SetSubData : BufferTest
    {
        private float[] dataToWrite = new float[] { -1 };

        [TestMethod]
        public void ValidWrite()
        {
            // Write a -1 and index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            buffer.SetSubData(dataToWrite, offset);

            float[] newBufferData = new float[] { 1.5f, -1, 3.5f };
            CollectionAssert.AreEqual(newBufferData, buffer.GetData<float>());
        }

        [TestMethod]
        public void NegativeOffset()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.SetSubData(dataToWrite, -1));
        }

        [TestMethod]
        public void ExceedsBufferSize()
        {
            // Try to write into an element past the end of the buffer.
            int offset = sizeof(float) * (originalData.Length + 1);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.SetSubData(dataToWrite, offset));
        }
    }
}
