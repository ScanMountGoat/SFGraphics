using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using Tests;

namespace SFGraphics.Test.BufferObjectTests
{
    [TestClass]
    public class SizeInBytes : ContextTest
    {
        [TestMethod]
        public void UninitializedBuffer()
        {
            var buffer = new BufferObject(BufferTarget.ArrayBuffer);
            Assert.AreEqual(0, buffer.SizeInBytes);
        }

        [TestMethod]
        public void SetEmptyData()
        {
            var buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(new float[] { }, BufferUsageHint.StaticDraw);
            Assert.AreEqual(0, buffer.SizeInBytes);
        }

        [TestMethod]
        public void SetNonEmptyData()
        {
            var buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(new float[] { 1, 2, 3 }, BufferUsageHint.StaticDraw);
            Assert.AreEqual(sizeof(float) * 3, buffer.SizeInBytes);
        }
    }
}
