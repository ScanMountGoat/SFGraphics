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
                if (shader == null)
                    shader = ShaderTestUtils.SetUpContextCreateValidShader();
            }

            [TestMethod]
            public void ValidNameValidType()
            {
                shader.SetUint("uintArray1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("uintArray1", ActiveUniformType.UnsignedInt);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidName()
            {
                shader.SetUint("memes", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.UnsignedInt);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidType()
            {
                shader.SetUint("float1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.UnsignedInt);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
