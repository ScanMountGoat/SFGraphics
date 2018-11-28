using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Framebuffers;
using OpenTK.Graphics.OpenGL;

namespace FramebufferTests
{
    [TestClass]
    public class ConstructorExceptions
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NegativeColorAttachments()
        {
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, -1));
        }

        [TestMethod]
        public void UncompressedFormat()
        {
            // Shouldn't throw an exception.
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 8, 4, PixelInternalFormat.Rgb);
            Assert.AreEqual(8, framebuffer.Width);
            Assert.AreEqual(4, framebuffer.Height);
        }

        [TestMethod]
        public void CompressedFormat()
        {
            var e = Assert.ThrowsException<ArgumentException>(() =>
                new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.CompressedRgbaS3tcDxt1Ext));
        }
    }
}
