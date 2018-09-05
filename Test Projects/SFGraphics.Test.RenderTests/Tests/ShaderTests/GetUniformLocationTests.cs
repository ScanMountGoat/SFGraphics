using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests
{
    [TestClass]
    public class GetUniformLocationTests
    {
        public static Shader shader;

        [TestInitialize()]
        public void Initialize()
        {
            if (shader == null)
            { 
                shader = ShaderTestUtils.SetupContextCreateValidShader();
                // Allow for setting uniforms.
                shader.UseProgram();
            }
        }

        [TestMethod]
        public void GetUniformLocationValidName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "float1"), shader.GetUniformLocation("float1"));
        }

        [TestMethod]
        public void GetUniformLocationInvalidName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "memes"), shader.GetUniformLocation("memes"));
        }

        [TestMethod]
        public void GetUniformLocationValidArrayName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "floatArray1"), shader.GetUniformLocation("floatArray1"));
        }

        [TestMethod]
        public void GetUniformLocationValidArrayNameBrackets()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "floatArray1[0]"), shader.GetUniformLocation("floatArray1[0]"));
        }
    }
}
