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

namespace SFGraphicsTest.ShaderTests
{
    [TestClass]
    public class ShaderTest
    {
        [TestClass]
        public class ProgramCreationJustFragShader
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

        [TestClass]
        public class ProgramCreationJustVertShader
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void ValidVertShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validVert.vert");
                shader.LoadShader(shaderSource, ShaderType.VertexShader);

                Assert.IsTrue(shader.ProgramCreatedSuccessfully());
            }

            [TestMethod]
            public void InvalidVertShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.invalidVert.vert");
                shader.LoadShader(shaderSource, ShaderType.VertexShader);

                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }

        [TestClass]
        public class ProgramCreationNoShaders
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void NoShaders()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }

        [TestClass]
        public class SetFloat
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();

                // Load the shader file from the embedded resources.
                shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            }

            [TestMethod]
            public void SetFloatValidName()
            {
                shader.SetFloat("float1", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable float1.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatInvalidName()
            {
                shader.SetFloat("memes", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }

        [TestClass]
        public class SetInt
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();

                // Load the shader file from the embedded resources.
                shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);
            }

            [TestMethod]
            public void SetIntValidName()
            {
                shader.SetInt("int1", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable int1.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatInvalidName()
            {
                shader.SetInt("memes", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }

        [TestClass]
        public class SetBoolToInt
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();

                // Load the shader file from the embedded resources.
                shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);
            }

            [TestMethod]
            public void SetBoolValidName()
            {
                shader.SetBoolToInt("boolInt1", true);
                string expected = "[Warning] Attempted to set undeclared uniform variable boolInt1.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolInvalidName()
            {
                shader.SetBoolToInt("memes", true);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
