using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.Tests.BufferObjectTests
{
    [TestClass]
    public class BufferSubDataTests
    {
        private float[] originalBufferData = new float[] { 1.5f, 2.5f, 3.5f };
        private float[] dataToWrite = new float[] { -1 };

        private BufferObject bufferObject;

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();

            bufferObject = new BufferObject(BufferTarget.ArrayBuffer);
            bufferObject.BufferData(originalBufferData, sizeof(float), BufferUsageHint.StaticDraw);
        }

        [TestMethod]
        public void BufferSubDataValidWrite()
        {
            // Write a -1 and index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            bufferObject.BufferSubData(dataToWrite, offset, sizeof(float));

            float[] newBufferData = new float[] { 1.5f, -1, 3.5f };
            CollectionAssert.AreEqual(newBufferData, bufferObject.GetBufferData<float>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BufferSubDataNegativeOffset()
        {
            bufferObject.BufferSubData(dataToWrite, -1, sizeof(float));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BufferSubDataNegativeItemSize()
        {
            bufferObject.BufferSubData(dataToWrite, 0, -1);
        }
    }
}
