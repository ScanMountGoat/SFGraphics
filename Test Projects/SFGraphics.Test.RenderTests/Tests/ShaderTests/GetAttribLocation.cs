using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests
{
    [TestClass]
    public class GetAttribLocation
    {
        public static Shader shader;

        [TestInitialize()]
        public void Initialize()
        {
            if (shader == null)
                shader = ShaderTestUtils.SetUpContextCreateValidShader();
        }

        [TestMethod]
        public void ValidName()
        {
            Assert.AreEqual(GL.GetAttribLocation(shader.Id, "position"), shader.GetAttribLocation("position"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.AreEqual(GL.GetAttribLocation(shader.Id, "memes"), shader.GetAttribLocation("memes"));
        }
    }
}
