using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFGraphics.Test.BufferObjectTests
{
    [TestClass]
    public class SetSubData : BufferTest
    {

        [TestMethod]
        public void ValidWrite()
        {
            // Write a -1 at index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            float[] dataToWrite = { -1 };
            buffer.SetSubData(dataToWrite, offset);

            float[] newBufferData = { 1.5f, -1, 3.5f };
            CollectionAssert.AreEqual(newBufferData, buffer.GetData<float>());
        }

        [TestMethod]
        public void ValidWriteSingleElement()
        {
            // Write a -1 at index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            buffer.SetSubData(-1f, offset);

            float[] newBufferData = { 1.5f, -1, 3.5f };
            CollectionAssert.AreEqual(newBufferData, buffer.GetData<float>());
        }

        [TestMethod]
        public void NegativeOffset()
        {
            float[] dataToWrite = { -1 };
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.SetSubData(dataToWrite, -1));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void NegativeOffsetSingleElement()
        {
            float dataToWrite = -1f;
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.SetSubData(dataToWrite, -1));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void ExceedsBufferSize()
        {
            // Try to write into an element past the end of the buffer.
            float[] dataToWrite = { -1 };
            int offset = sizeof(float) * (originalData.Length + 1);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.SetSubData(dataToWrite, offset));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer must not exceed the buffer's storage."));
        }

        [TestMethod]
        public void ExceedsBufferSizeSingleElement()
        {
            // Try to write into an element past the end of the buffer.
            float dataToWrite = -1f;
            int offset = sizeof(float) * (originalData.Length + 1);
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                buffer.SetSubData(dataToWrite, offset));

            Assert.IsTrue(e.Message.Contains("The data read from or written to a buffer must not exceed the buffer's storage."));
        }
    }
}
