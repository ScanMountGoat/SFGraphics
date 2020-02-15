using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.RenderBuffers;
using System;

namespace SFGraphics.Test.RenderbufferTests
{
    [TestClass]
    public class ConstructorExceptions : GraphicsContextTest
    {
        [TestMethod]
        public void NegativeWidth()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Renderbuffer(-1, 8, RenderbufferStorage.Rgba8));

            Assert.IsTrue(e.Message.Contains("Dimensions must be non negative."));
            Assert.AreEqual("width", e.ParamName);
        }

        [TestMethod]
        public void NegativeHeight()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Renderbuffer(8, -1, RenderbufferStorage.Rgba8));

            Assert.IsTrue(e.Message.Contains("Dimensions must be non negative."));
            Assert.AreEqual("height", e.ParamName);
        }

        [TestMethod]
        public void NegativeWidthMultisample()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Renderbuffer(-1, 8, 4,RenderbufferStorage.Rgba8));

            Assert.IsTrue(e.Message.Contains("Dimensions must be non negative."));
            Assert.AreEqual("width", e.ParamName);
        }

        [TestMethod]
        public void NegativeHeightMultisample()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Renderbuffer(8, -1, 4, RenderbufferStorage.Rgba8));

            Assert.IsTrue(e.Message.Contains("Dimensions must be non negative."));
            Assert.AreEqual("height", e.ParamName);
        }
    }
}
