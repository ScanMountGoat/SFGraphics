using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector2
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTestUtils.SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetVector2ValidName()
            {
                shader.SetVector2("vector2a", new Vector2(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2InvalidName()
            {
                shader.SetVector2("memes", new Vector2(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec2);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2FloatsValidName()
            {
                shader.SetVector2("vector2a", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2FloatsInvalidName()
            {
                shader.SetVector2("memes2", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec2);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2InvalidType()
            {
                shader.SetVector2("float1", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec2);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2ValidType()
            {
                shader.SetVector2("vector2a", 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
