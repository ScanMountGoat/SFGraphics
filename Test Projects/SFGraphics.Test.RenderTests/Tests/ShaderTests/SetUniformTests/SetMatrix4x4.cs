using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetMatrix4x4
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                if (shader == null)
                    shader = ShaderTestUtils.SetUpContextCreateValidShader();
            }

            [TestMethod]
            public void ValidName()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("matrix4a", ref matrix4);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("matrix4a", ActiveUniformType.FloatMat4);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidName()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("memes", ref matrix4);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatMat4);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidType()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("float1", ref matrix4);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatMat4);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void ValidType()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("matrix4a", ref matrix4);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("matrix4a", ActiveUniformType.FloatMat4);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
