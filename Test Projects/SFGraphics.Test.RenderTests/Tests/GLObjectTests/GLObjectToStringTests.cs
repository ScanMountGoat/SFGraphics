using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.VertexArrays;
using SFGraphics.GLObjects.RenderBuffers;
using SFGraphics.GLObjects.Framebuffers;

namespace SFGraphics.Test.RenderTests.GLObjectTests
{
    [TestClass()]
    public class GLObjectToStringTests
    {
        [TestInitialize]
        public void SetUpExtensions()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void ShaderToString()
        {
            Shader glObject = new Shader();
            Assert.AreEqual($"ShaderProgram ID: { glObject.Id }", glObject.ToString());
        }

        [TestMethod]
        public void VertexArrayObjectToString()
        {
            VertexArrayObject glObject = new VertexArrayObject();
            Assert.AreEqual($"VertexArrayObject ID: { glObject.Id }", glObject.ToString());
        }

        [TestMethod]
        public void BufferObjectToString()
        {
            BufferObject glObject = new BufferObject(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer);
            Assert.AreEqual($"BufferObject ID: { glObject.Id }", glObject.ToString());
        }

        [TestMethod]
        public void TextureToString()
        {
            // We only need to test one subclass.
            Texture glObject = new Texture2D();
            Assert.AreEqual($"Texture ID: { glObject.Id }", glObject.ToString());
        }

        [TestMethod]
        public void RenderbufferObjectToString()
        {
            Renderbuffer glObject = new Renderbuffer(1, 1, OpenTK.Graphics.OpenGL.RenderbufferStorage.Rgba16);
            Assert.AreEqual($"RenderbufferObject ID: { glObject.Id }", glObject.ToString());
        }

        [TestMethod]
        public void FramebufferObjectToString()
        {
            Framebuffer glObject = new Framebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.Framebuffer);
            Assert.AreEqual($"FramebufferObject ID: { glObject.Id }", glObject.ToString());
        }
    }
}