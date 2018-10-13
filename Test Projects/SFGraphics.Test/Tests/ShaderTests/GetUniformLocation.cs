using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests
{
    [TestClass]
    public class GetUniformLocation : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "float1"), shader.GetUniformLocation("float1"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.AreEqual(-1, shader.GetUniformLocation("memes"));
        }

        [TestMethod]
        public void EmptyName()
        {
            Assert.AreEqual(-1, shader.GetUniformLocation(""));
        }

        [TestMethod]
        public void NullName()
        {
            Assert.AreEqual(-1, shader.GetUniformLocation(null));
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

        [TestMethod]
        public void ShaderNotLinked()
        {
            var shader = new SFGraphics.GLObjects.Shaders.Shader();
            Assert.AreEqual(-1, shader.GetUniformLocation("memes"));
        }

        [TestMethod]
        public void ShaderLinkedTwice()
        {
            var shader = new SFGraphics.GLObjects.Shaders.Shader();

            string fragSource1 = RenderTestUtils.ResourceShaders.GetShaderSource("undefinedFunction.frag");
            shader.LoadShader(fragSource1, ShaderType.FragmentShader);

            string fragSource2 = RenderTestUtils.ResourceShaders.GetShaderSource("definedFunction.frag");
            shader.LoadShader(fragSource2, ShaderType.FragmentShader);

            // Uniform locations can change after linking again.
            Assert.AreEqual(GL.GetUniformLocation(shader.Id, "vec3Uniform"), shader.GetUniformLocation("vec3Uniform"));
        }
    }
}
