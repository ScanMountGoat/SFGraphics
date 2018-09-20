﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetBoolToInt : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetBoolToInt("boolInt1", true);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("boolInt1", ActiveUniformType.Int);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetBoolToInt("memes", true);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Int);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetBoolToInt("float1", true);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Int);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void ValidType()
        {
            shader.SetBoolToInt("int1", true);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("int1", ActiveUniformType.Int);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }
    }
}