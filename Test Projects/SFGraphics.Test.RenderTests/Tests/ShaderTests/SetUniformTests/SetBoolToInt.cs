using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;


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
                shader = ShaderSetup.SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetBoolValidName()
            {
                shader.SetBoolToInt("boolInt1", true);
                string expected = "[Warning] Attempted to set undeclared uniform variable boolInt1";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolInvalidName()
            {
                shader.SetBoolToInt("memes", true);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolInvalidType()
            {
                shader.SetBoolToInt("float1", true);
                string expected = "[Warning] No uniform variable float1 of type Int";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetBoolValidType()
            {
                shader.SetBoolToInt("int1", true);
                string expected = "[Warning] No uniform variable int1 of type Int";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
