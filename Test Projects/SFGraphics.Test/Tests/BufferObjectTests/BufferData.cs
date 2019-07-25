using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace SFGraphics.Test.BufferObjectTests
{
    [TestClass]
    public class BufferData : BufferTest
    {
        private struct Empty
        {
        }

        [TestMethod]
        public void GetFloats()
        {
            float[] readData = buffer.GetData<float>();
            CollectionAssert.AreEqual(originalData, readData);
        }

        [TestMethod]
        public void GetDataEmptyStruct()
        {
            // Empty structs use 1 byte.
            Empty[] readData = buffer.GetData<Empty>();
            Assert.AreEqual(sizeof(float) * originalData.Length, readData.Length);
        }

        [TestMethod]
        public void GetVector3FromFloats()
        {
            // The 3 floats should make a single Vector3 struct.
            Vector3[] expectedData = { new Vector3(1.5f, 2.5f, 3.5f) };
            Vector3[] readData = buffer.GetData<Vector3>();
            CollectionAssert.AreEqual(expectedData, readData);
        }

        [TestMethod]
        public void DataSizeNotDivisibleByType()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() => buffer.GetData<Vector4>());
            Assert.AreEqual("T", e.ParamName);
            Assert.AreEqual($"The buffer's size is not divisible by the requested type's size.{Environment.NewLine}Parameter name: T", e.Message);
        }
    }
}
