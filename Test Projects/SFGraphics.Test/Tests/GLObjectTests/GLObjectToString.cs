﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Framebuffers;
using SFGraphics.GLObjects.RenderBuffers;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.VertexArrays;

namespace SFGraphics.Test.GLObjectTests
{
    [TestClass]
    public class GLObjectToString : GraphicsContextTest
    {
        [TestMethod]
        public void Shader()
        {
            Shader glObject = new Shader();
            Assert.AreEqual($"ShaderProgram ID: {glObject.Id}", glObject.ToString());
        }

        [TestMethod]
        public void VertexArrayObject()
        {
            VertexArrayObject glObject = new VertexArrayObject();
            Assert.AreEqual($"VertexArrayObject ID: {glObject.Id}", glObject.ToString());
        }

        [TestMethod]
        public void BufferObject()
        {
            BufferObject glObject = new BufferObject(BufferTarget.ArrayBuffer);
            Assert.AreEqual($"BufferObject ID: {glObject.Id}", glObject.ToString());
        }

        [TestMethod]
        public void Texture()
        {
            // We only need to test one subclass.
            Texture glObject = new Texture2D();
            Assert.AreEqual($"Texture ID: {glObject.Id}", glObject.ToString());
        }

        [TestMethod]
        public void RenderbufferObject()
        {
            Renderbuffer glObject = new Renderbuffer(1, 1, RenderbufferStorage.Rgba16);
            Assert.AreEqual($"RenderbufferObject ID: {glObject.Id}", glObject.ToString());
        }

        [TestMethod]
        public void FramebufferObject()
        {
            Framebuffer glObject = new Framebuffer(FramebufferTarget.Framebuffer);
            Assert.AreEqual($"FramebufferObject ID: {glObject.Id}", glObject.ToString());
        }
    }
}