using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetInt
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetIntValidName()
            {
                shader.SetInt("int1", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable int1.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetIntInvalidName()
            {
                shader.SetInt("memes", 0);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
