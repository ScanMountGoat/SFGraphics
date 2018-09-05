using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetUintArray
        {
            Shader shader;
            uint[] values = new uint[] { 1, 2, 3 };

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTestUtils.SetupContextCreateValidShader();
            }

            [TestMethod]
            public void SetUintValidNameValidType()
            {
                shader.SetUint("uintArray1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("uintArray1", ActiveUniformType.UnsignedInt);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetUintInvalidName()
            {
                shader.SetUint("memes", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.UnsignedInt);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetUintInvalidType()
            {
                shader.SetUint("float1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.UnsignedInt);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
