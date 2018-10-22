using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Framebuffers;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

namespace FramebufferTests
{
    [TestClass]
    public class Completion
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
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
            Assert.AreEqual("FramebufferComplete", framebuffer.GetStatus());
        }

        [TestMethod]
        public void OneColorAttachment()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 1);
            Assert.AreEqual("FramebufferComplete", framebuffer.GetStatus());
        }

        [TestMethod]
        public void OneMultisampledTextureColorAttachment()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, new Texture2DMultisample(8, 8, PixelInternalFormat.Rgba, 1));
            Assert.AreEqual("FramebufferComplete", framebuffer.GetStatus());
        }

        [TestMethod]
        public void MultipleColorAttachments()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 3);
            Assert.AreEqual("FramebufferComplete", framebuffer.GetStatus());
        }
    }
}
