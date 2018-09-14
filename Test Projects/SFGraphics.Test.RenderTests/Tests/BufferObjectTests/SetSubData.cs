using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.BufferObjects;
using OpenTK.Graphics.OpenGL;

namespace BufferObjectTests
{
    [TestClass]
    public class SetSubData
    {
        private float[] originalBufferData = new float[] { 1.5f, 2.5f, 3.5f };
        private float[] dataToWrite = new float[] { -1 };

        private BufferObject bufferObject;

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            bufferObject = new BufferObject(BufferTarget.ArrayBuffer);
            bufferObject.SetData(originalBufferData, BufferUsageHint.StaticDraw);
        }

        [TestMethod]
        public void ValidWrite()
        {
            // Write a -1 and index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            bufferObject.SetSubData(dataToWrite, offset);

            float[] newBufferData = new float[] { 1.5f, -1, 3.5f };
            CollectionAssert.AreEqual(newBufferData, bufferObject.GetData<float>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeOffset()
        {
            bufferObject.SetSubData(dataToWrite, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExceedsBufferSize()
        {
            // Try to write into an element past the end of the buffer.
            int offset = sizeof(float) * (originalBufferData.Length + 1);
            bufferObject.SetSubData(dataToWrite, offset);
        }
    }
}
