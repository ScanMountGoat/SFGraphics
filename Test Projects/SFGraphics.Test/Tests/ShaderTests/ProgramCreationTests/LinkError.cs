using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class LinkError : GraphicsContextTest
    {
        [TestMethod]
        public void FunctionNotDefined()
        {
            Shader shader = new Shader();

            // The shader declared but does not define a function.
            string fragSource = ResourceShaders.GetShaderSource("undefinedFunction.frag");
            shader.LoadShaders(new ShaderObject(fragSource, ShaderType.FragmentShader));
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
