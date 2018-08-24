using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.BufferObjectTests
{
    [TestClass]
    public class BufferDataTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void GetFloatBufferData()
        {
            float[] data = new float[] { 1.5f, 2.5f, 3.5f };
            BufferObject buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(data, sizeof(float), BufferUsageHint.StaticDraw);

            float[] readData = buffer.GetData<float>();
            CollectionAssert.AreEqual(data, readData);
        }
    }
}
