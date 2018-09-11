using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;

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
            public void ValidName()
            {
                shader.SetTexture("tex2D", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("tex2D", ActiveUniformType.Sampler2D);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidName()
            {
                shader.SetTexture("memes", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidType()
            {
                shader.SetTexture("float1", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void InvalidTarget()
            {
                shader.SetTexture("texCube", new Texture2D(), 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("texCube", ActiveUniformType.Sampler2D);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
