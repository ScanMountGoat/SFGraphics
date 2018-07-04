using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphicsRenderTests.ShaderTests
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
                shader = SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetMatrix4x4ValidName()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("matrix4a", ref matrix4);
                string expected = "[Warning] Attempted to set undeclared uniform variable vector3a.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetMatrix4x4InvalidName()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("memes", ref matrix4);
                string expected = "[Warning] Attempted to set undeclared uniform variable memes.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetMatrix4x4InvalidType()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("float1", ref matrix4);
                string expected = "[Warning] No uniform variable float1 of type FloatMat4.";
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetMatrix4x4ValidType()
            {
                Matrix4 matrix4 = Matrix4.Identity;
                shader.SetMatrix4x4("matrix4a", ref matrix4);
                string expected = "[Warning] No uniform variable vector4a of type FloatMat4.";
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
