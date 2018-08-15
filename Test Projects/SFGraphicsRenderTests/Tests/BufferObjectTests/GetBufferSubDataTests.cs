using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.Tests.BufferObjectTests
{
    [TestClass]
    public class GetBufferSubDataTests
    {
        private float[] originalBufferData = new float[] { 1.5f, 2.5f, 3.5f };

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
            // Read at index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            float[] bufferData = bufferObject.GetBufferSubData<float>(offset, 1, sizeof(float));

            Assert.AreEqual(1, bufferData.Length);
            Assert.AreEqual(2.5f, bufferData[0]);
        }
    }
}
