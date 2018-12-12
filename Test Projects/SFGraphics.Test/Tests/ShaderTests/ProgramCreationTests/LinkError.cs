using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class LinkError : Tests.ContextTest
    {
        [TestMethod]
        public void FunctionNotDefined()
        {
            Shader shader = new Shader();

            // The shader declared but does not define a function.
            string fragSource = RenderTestUtils.ResourceShaders.GetShaderSource("undefinedFunction.frag");
            shader.LoadShader(fragSource, ShaderType.FragmentShader);
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
