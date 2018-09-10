using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetTexture
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                if (shader == null)
                    shader = ShaderTestUtils.SetUpContextCreateValidShader();
            }

            [TestMethod]
            public void SetTextureValidName()
            {
                shader.SetTexture("tex2D", 0, TextureTarget.Texture2D, 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("tex2D", ActiveUniformType.Sampler2D);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetTextureInvalidName()
            {
                shader.SetTexture("memes", 0, TextureTarget.Texture2D, 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetTextureInvalidType()
            {
                shader.SetTexture("float1", 0, TextureTarget.Texture2D, 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetTextureInvalidTarget()
            {
                shader.SetTexture("texCube", 0, TextureTarget.Texture2D, 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("texCube", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
