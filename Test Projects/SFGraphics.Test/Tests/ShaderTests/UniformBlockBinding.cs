﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class UniformBlockBinding : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.UniformBlockBinding("UniformBlockA", 3);
            GL.GetActiveUniformBlock(shader.Id,
                shader.GetUniformBlockIndex("UniformBlockA"),
                ActiveUniformBlockParameter.UniformBlockBinding, out int binding);
            Assert.AreEqual(3, binding);
        }

        [TestMethod]
        public void InvalidName()
        {
            // Shouldn't throw graphics exceptions.
            shader.UniformBlockBinding("memes", 3);
        }


        [TestMethod]
        public void InvalidBinding()
        {
            // Shouldn't throw graphics exceptions.
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                shader.UniformBlockBinding("UniformBlock", -1));

            Assert.IsTrue(e.Message.Contains("Binding points must be non negative."));
            Assert.AreEqual("bindingPoint", e.ParamName);
        }
    }
}
