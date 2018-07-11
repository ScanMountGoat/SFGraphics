using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsRenderTests.Tests.FramebufferTests
{
    [TestClass]
    public class CompletionTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.CreateDummyContext();
        }

        [TestMethod]
        public void NoAttachments()
        {
            // This is missing a depth attachment.
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            Assert.AreEqual("FramebufferIncompleteMissingAttachment", framebuffer.GetStatus());
        }

        [TestMethod]
        public void JustDepthAttachments()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 0);
            Assert.AreEqual("FramebufferCompleteExt", framebuffer.GetStatus());
        }

        [TestMethod]
        public void OneColorAttachment()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 1);
            Assert.AreEqual("FramebufferCompleteExt", framebuffer.GetStatus());
        }

        [TestMethod]
        public void MultipleColorAttachments()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 3);
            Assert.AreEqual("FramebufferCompleteExt", framebuffer.GetStatus());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeColorAttachments()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, -1);
        }
    }
}
