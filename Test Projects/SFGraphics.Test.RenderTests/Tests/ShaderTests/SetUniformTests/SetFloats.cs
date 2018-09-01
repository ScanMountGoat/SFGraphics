using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetFloats
        {
            Shader shader;
            private float[] values = new float[] { 1.5f, 2.5f, 3.5f };

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderSetup.SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetFloatsValidName()
            {
                shader.SetFloats("floatArray1", values);
                string expected = "[Warning] Attempted to set undeclared uniform variable floatArray1";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatsInvalidType()
            {
                shader.SetFloats("intArray1", values);
                string expected = "[Warning] No uniform variable intArray1 of type Float";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatsValidType()
            {
                shader.SetFloats("floatArray1", values);
                string expected = "[Warning] No uniform variable floatArray1 of type Float";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetFloatsInvalidName()
            {
                shader.SetFloats("memesArray", values);
                string expected = "[Warning] Attempted to set undeclared uniform variable memesArray";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
