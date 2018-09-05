using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetBoolToInt
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTestUtils.SetupContextCreateValidShader();
            }

            [TestMethod]
            public void SetBoolValidName()
            {
                shader.SetBoolToInt("boolInt1", true);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("boolInt1", ActiveUniformType.Int);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolInvalidName()
            {
                shader.SetBoolToInt("memes", true);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Int);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolInvalidType()
            {
                shader.SetBoolToInt("float1", true);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Int);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolValidType()
            {
                shader.SetBoolToInt("int1", true);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("int1", ActiveUniformType.Int);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
