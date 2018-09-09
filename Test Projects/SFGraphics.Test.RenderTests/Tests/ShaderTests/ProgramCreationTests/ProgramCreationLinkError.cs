using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.RenderTests.ShaderTests.ProgramCreationTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ProgramCreationLinkError
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]
            public void LinkError()
            {
                Shader shader = new Shader();

                // The shader declared but does not define a function.
                string fragSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.linkError.frag");
                shader.LoadShader(fragSource, ShaderType.FragmentShader);
                Assert.IsFalse(shader.LinkStatusIsOk);
            }
        }
    }
}
