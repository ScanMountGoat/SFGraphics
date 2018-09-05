using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetFloatArray
        {
            Shader shader;
            private float[] values = new float[] { 1.5f, 2.5f, 3.5f };

            [TestInitialize()]
            public void Initialize()
            {
                if (shader == null)
                    shader = ShaderTestUtils.SetUpContextCreateValidShader();
            }

            [TestMethod]
            public void SetFloatsValidName()
            {
                shader.SetFloat("floatArray1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("floatArray1", ActiveUniformType.Float);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatsInvalidType()
            {
                shader.SetFloat("intArray1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("intArray1", ActiveUniformType.Float);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatsValidType()
            {
                shader.SetFloat("floatArray1", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("floatArray1", ActiveUniformType.Float);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatsInvalidName()
            {
                shader.SetFloat("memesArray", values);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memesArray", ActiveUniformType.Float);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
