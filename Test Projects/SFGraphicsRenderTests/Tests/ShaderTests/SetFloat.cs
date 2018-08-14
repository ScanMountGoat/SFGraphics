using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetFloat
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTest.SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetFloatValidName()
            {
                shader.SetFloat("float1", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable float1";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatInvalidType()
            {
                shader.SetFloat("int1", 0);
                string expected = "[Warning] No uniform variable int1 of type Float";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatValidType()
            {
                shader.SetFloat("float1", 0);
                string expected = "[Warning] No uniform variable float1 of type Float";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatInvalidName()
            {
                shader.SetFloat("memes", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
