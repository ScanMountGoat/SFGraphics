using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector2
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetVector2ValidName()
            {
                shader.SetVector2("vector3a", new Vector2(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2InvalidName()
            {
                shader.SetVector2("memes", new Vector2(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2FloatsValidName()
            {
                shader.SetVector2("vector2a", 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2FloatsInvalidName()
            {
                shader.SetVector2("memes2", 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes2.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2InvalidType()
            {
                shader.SetVector2("float1", 1, 1);
                string expected = "[Warning] No uniform variable float1 of type FloatVec2.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector2ValidType()
            {
                shader.SetVector2("vector2a", 1, 1);
                string expected = "[Warning] No uniform variable vector2a of type FloatVec2.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
