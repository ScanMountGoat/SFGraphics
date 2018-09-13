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
            TestTools.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeColorAttachments()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, -1);
        }

        [TestMethod]
        public void UncompressedFormat()
        {
            // Shouldn't throw an exception.
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgb);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompressedFormat()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.CompressedRgbaS3tcDxt1Ext);
        }
    }
}
