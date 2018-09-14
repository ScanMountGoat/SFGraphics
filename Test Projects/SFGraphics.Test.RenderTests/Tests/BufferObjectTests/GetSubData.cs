using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.BufferObjects;
using OpenTK.Graphics.OpenGL;

namespace BufferObjectTests
{
    [TestClass]
    public class GetSubData : Tests.ContextTest
    {
        private float[] originalBufferData = new float[] { 1.5f, 2.5f, 3.5f };

        private BufferObject bufferObject;

        [TestInitialize()]
        public override void Initialize()
        {
            base.Initialize();
            bufferObject = new BufferObject(BufferTarget.ArrayBuffer);
            bufferObject.SetData(originalBufferData, BufferUsageHint.StaticDraw);
        }

        [TestMethod]
        public void GetBufferSubDataValidRead()
        {
            // Read at index 1.
            int index = 1;
            int offset = sizeof(float) * index;
            float[] bufferData = bufferObject.GetSubData<float>(offset, 1);

            Assert.AreEqual(1, bufferData.Length);
            Assert.AreEqual(2.5f, bufferData[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetBufferSubDataNegativeOffset()
        {
            float[] bufferData = bufferObject.GetSubData<float>(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetBufferSubDataNegativeItemCount()
        {
            float[] bufferData = bufferObject.GetSubData<float>(0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetBufferSubDataExceedsBufferSize()
        {
            // Try to read one element beyond the buffer's capacity.
            float[] bufferData = bufferObject.GetSubData<float>(0, originalBufferData.Length + 1);
        }
    }
}
