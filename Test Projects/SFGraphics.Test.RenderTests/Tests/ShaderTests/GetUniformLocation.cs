using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests
{
    [TestClass]
    public class GetUniformLocation
    {
        public static Shader shader;

        [TestInitialize()]
        public void Initialize()
        {
            if (shader == null)
            { 
                if (shader == null)
                    shader = RenderTestUtils.ShaderTestUtils.SetUpContextCreateValidShader();
            }
        }

        [TestMethod]
        public void ValidName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "float1"), shader.GetUniformLocation("float1"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "memes"), shader.GetUniformLocation("memes"));
        }

        [TestMethod]
        public void ValidArrayName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "floatArray1"), shader.GetUniformLocation("floatArray1"));
        }

        [TestMethod]
        public void ValidArrayNameBrackets()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "floatArray1[0]"), shader.GetUniformLocation("floatArray1[0]"));
        }
    }
}
