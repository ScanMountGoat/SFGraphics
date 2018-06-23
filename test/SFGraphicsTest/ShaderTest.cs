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
                Shader shader = new Shader();
                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }

        [TestClass]
        public class ProgramCreationValidFragInvalidVert
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void ValidFragInvalidVert()
            {
                Shader shader = new Shader();

                // Load the shader files from the embedded resources.
                string fragSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
                shader.LoadShader(fragSource, ShaderType.FragmentShader);
                // Force an update of compilation/link status.
                Assert.IsTrue(shader.ProgramCreatedSuccessfully());

                // Make sure the compilation/link status still updates.
                string vertSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.invalidVert.vert");
                shader.LoadShader(vertSource, ShaderType.VertexShader);
                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }

        [TestClass]
        public class ProgramCreationLinkError
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.CreateDummyContext();
            }

            [TestMethod]
            public void LinkError()
            {
                Shader shader = new Shader();

                // The shader declared but does not define a function.
                string fragSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.linkError.frag");
                shader.LoadShader(fragSource, ShaderType.FragmentShader);
                Assert.IsFalse(shader.ProgramCreatedSuccessfully());
            }
        }

        private static Shader SetupContextCreateValidFragShader()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.CreateDummyContext();

            // Load the shader file from the embedded resources.
            // Used for testing shader setters.
            Shader shader = new Shader();
            string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphicsTest.Shaders.validFrag.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);
            return shader;
        }

        [TestClass]
        public class SetFloat
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
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
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetIntValidName()
            {
                shader.SetInt("int1", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable int1.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetIntInvalidName()
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
                shader = SetupContextCreateValidFragShader();
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

        [TestClass]
        public class SetVector3
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetVector3ValidName()
            {
                shader.SetVector3("vector3a", new Vector3(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3InvalidName()
            {
                shader.SetVector3("memes", new Vector3(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3FloatsValidName()
            {
                shader.SetVector3("vector3a", 1, 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3FloatsInvalidName()
            {
                shader.SetVector3("memes2", 1, 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes2.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }

        [TestClass]
        public class SetVector2
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetVector2ValidName()
            {
                shader.SetVector2("vector3a", new Vector2(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2InvalidName()
            {
                shader.SetVector2("memes", new Vector2(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2FloatsValidName()
            {
                shader.SetVector2("vector2a", 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2FloatsInvalidName()
            {
                shader.SetVector2("memes2", 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes2.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
