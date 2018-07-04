using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector4
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetVector4ValidName()
            {
                shader.SetVector4("vector4a", new Vector4(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable vector4a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4InvalidName()
            {
                shader.SetVector4("memes", new Vector4(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4FloatsValidName()
            {
                shader.SetVector4("vector4a", 1, 1, 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable vector4a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4FloatsInvalidName()
            {
                shader.SetVector4("memes2", 1, 1, 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes2.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4InvalidType()
            {
                shader.SetVector4("float1", 1, 1, 1, 1);
                string expected = "[Warning] No uniform variable float1 of type FloatVec4.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4ValidType()
            {
                shader.SetVector4("vector4a", 1, 1, 1, 1);
                string expected = "[Warning] No uniform variable vector4a of type FloatVec4.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
