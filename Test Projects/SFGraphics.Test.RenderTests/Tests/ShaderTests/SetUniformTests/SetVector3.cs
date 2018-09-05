using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector3
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTestUtils.SetupContextCreateValidShader();
            }

            [TestMethod]
            public void SetVector3ValidName()
            {
                shader.SetVector3("vector3a", new Vector3(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3InvalidName()
            {
                shader.SetVector3("memes", new Vector3(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec3);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3InvalidType()
            {
                shader.SetVector3("float1", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec3);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3ValidType()
            {
                shader.SetVector3("vector3a", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3FloatsValidName()
            {
                shader.SetVector3("vector3a", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector3FloatsInvalidName()
            {
                shader.SetVector3("memes2", 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec3);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
