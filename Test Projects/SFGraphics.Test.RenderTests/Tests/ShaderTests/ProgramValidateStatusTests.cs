using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests
{
    [TestClass]
    public class ProgramValidateStatusTests
    {
        public static Shader shader;

        [TestInitialize()]
        public void Initialize()
        {
            // We can't share shaders between tests.
            shader = ShaderTestUtils.SetUpContextCreateValidShader();
        }

        [TestMethod]
        public void ValidStatus()
        {
            shader.SetTexture("tex2D", 0, TextureTarget.Texture2D, 0);
            shader.SetTexture("texCube", 0, TextureTarget.TextureCubeMap, 1);

            Assert.IsTrue(shader.ProgramStatusIsValid);
        }

        [TestMethod]
        public void TwoTypesPerTextureUnit()
        {
            shader.SetTexture("tex2D", 0, TextureTarget.Texture2D, 0);
            shader.SetTexture("tex2D", 0, TextureTarget.TextureCubeMap, 0);

            // There can only be a single texture type for each active texture.
            Assert.IsFalse(shader.ProgramStatusIsValid);
        }
    }
}
