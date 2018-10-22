using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Framebuffers;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace FramebufferTests
{
    [TestClass]
    public class Completion : Tests.ContextTest
    {
        [TestMethod]
        public void NoAttachments()
        {
            // This is missing a depth attachment.
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            Assert.AreEqual(FramebufferErrorCode.FramebufferIncompleteMissingAttachment, framebuffer.GetStatus());
        }

        [TestMethod]
        public void JustDepthTexture()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 0);
            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
        }

        [TestMethod]
        public void OneColor()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 1);
            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
        }

        [TestMethod]
        public void OneMultisampledColorTexture()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, new Texture2DMultisample(8, 8, PixelInternalFormat.Rgba, 1));
            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
        }

        [TestMethod]
        public void MultisampledColorTextureMultiSampledRboDepth()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, new Texture2DMultisample(8, 8, PixelInternalFormat.Rgba, 1));
            framebuffer.AddAttachment(FramebufferAttachment.DepthAttachment, new SFGraphics.GLObjects.RenderBuffers.Renderbuffer(8, 8, 1, RenderbufferStorage.DepthComponent));
            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
        }

        [TestMethod]
        public void MultipleColorTextures()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 3);
            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
        }
    }
}
