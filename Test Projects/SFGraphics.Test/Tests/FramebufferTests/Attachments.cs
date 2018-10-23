using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Framebuffers;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;

namespace FramebufferTests
{
    [TestClass]
    public class Attachments : Tests.ContextTest
    {
        [TestMethod]
        public void NoAttachments()
        {
            // This is missing a depth attachment.
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);

            Assert.AreEqual(FramebufferErrorCode.FramebufferIncompleteMissingAttachment, framebuffer.GetStatus());
            Assert.AreEqual(0, framebuffer.Attachments.Count);
        }

        [TestMethod]
        public void JustDepthTexture()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 0);

            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
            Assert.AreEqual(1, framebuffer.Attachments.Count);
        }

        [TestMethod]
        public void OneColorTexture()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 1, 1, PixelInternalFormat.Rgba, 1);

            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
            Assert.AreEqual(2, framebuffer.Attachments.Count); // 1 + depth
        }

        [TestMethod]
        public void AddAttachmentChangesDimensions()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);

            var format = new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float);

            var texture1 = new Texture2D();
            texture1.LoadImageData(8, 4, format);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, texture1);

            Assert.AreEqual(8, framebuffer.Width);
            Assert.AreEqual(4, framebuffer.Height);

            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void DifferentSizedColorTextures()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            var format = new TextureFormatUncompressed(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float);

            var texture1 = new Texture2D();
            texture1.LoadImageData(8, 8, format);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, texture1);

            var texture2 = new Texture2D();
            texture2.LoadImageData(1, 1, format);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment1, texture2);
        }

        [TestMethod]
        public void OneMultisampledColorTexture()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, new Texture2DMultisample(8, 8, PixelInternalFormat.Rgba, 1));

            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
            Assert.AreEqual(1, framebuffer.Attachments.Count);
        }

        [TestMethod]
        public void MultisampledColorTextureMultiSampledRboDepth()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer);
            framebuffer.AddAttachment(FramebufferAttachment.ColorAttachment0, new Texture2DMultisample(8, 8, PixelInternalFormat.Rgba, 1));
            framebuffer.AddAttachment(FramebufferAttachment.DepthAttachment, new SFGraphics.GLObjects.RenderBuffers.Renderbuffer(8, 8, 1, RenderbufferStorage.DepthComponent));

            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
            Assert.AreEqual(2, framebuffer.Attachments.Count);
        }

        [TestMethod]
        public void MultipleColorTextures()
        {
            Framebuffer framebuffer = new Framebuffer(FramebufferTarget.Framebuffer, 8, 8, PixelInternalFormat.Rgba, 3);

            Assert.AreEqual(FramebufferErrorCode.FramebufferComplete, framebuffer.GetStatus());
            Assert.AreEqual(4, framebuffer.Attachments.Count); // 3 + depth
        }
    }
}
