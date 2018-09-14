using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class LinkError
    {
        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void FunctionNotDefined()
        {
            Shader shader = new Shader();

            // The shader declared but does not define a function.
            string fragSource = RenderTestUtils.ResourceShaders.GetShaderSource("linkError.frag");
            shader.LoadShader(fragSource, ShaderType.FragmentShader);
            Assert.IsFalse(shader.LinkStatusIsOk);
        }
    }
}
