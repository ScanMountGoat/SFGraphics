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

namespace SFGraphicsTest
{
    [TestClass]
    public class ShaderTest
    {
        [TestClass]
        public class FragShaderCompiled
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void ValidFragShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);

                Assert.IsTrue(shader.ProgramCreatedSuccessfully());
            }

            [TestMethod]
            public void InvalidFragShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.invalidFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);

                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }
    }
}
