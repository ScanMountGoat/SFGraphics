using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class JustVertShader : Tests.ContextTest
    {
        [TestMethod]
        public void ValidVertShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.vert");
            shader.LoadShader(shaderSource, ShaderType.VertexShader);

            Assert.IsTrue(shader.LinkStatusIsOk);

            Assert.AreEqual(0, shader.ActiveUniformCount);
            Assert.AreEqual(2, shader.ActiveAttributeCount);
        }

        [TestMethod]
        public void InvalidVertShader()
        {
            // Load the shader file from the embedded resources.
            Shader shader = new Shader();
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.vert");
            shader.LoadShader(shaderSource, ShaderType.VertexShader);

            Assert.IsFalse(shader.LinkStatusIsOk);

            Assert.AreEqual(0, shader.ActiveUniformCount);
            Assert.AreEqual(0, shader.ActiveAttributeCount);
        }
    }
}
