using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsTest.ShaderTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector3
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void SetVector3ValidName()
            {
                shader.SetVector3("vector3a", new Vector3(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void SetVector3InvalidName()
            {
                shader.SetVector3("memes", new Vector3(1));
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void SetVector3InvalidType()
            {
                shader.SetVector3("float1", 1, 1, 1);
                string expected = "[Warning] No uniform variable float1 of type FloatVec3.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void SetVector3ValidType()
            {
                shader.SetVector3("vector3a", 1, 1, 1);
                string expected = "[Warning] No uniform variable vector3a of type FloatVec3.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void SetVector3FloatsValidName()
            {
                shader.SetVector3("vector3a", 1, 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            [TestCategory("UnsafeRendering")]
            public void SetVector3FloatsInvalidName()
            {
                shader.SetVector3("memes2", 1, 1, 1);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes2.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
