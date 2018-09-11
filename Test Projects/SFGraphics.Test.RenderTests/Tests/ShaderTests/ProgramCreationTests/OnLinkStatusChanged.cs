using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;

namespace SFGraphics.Test.RenderTests.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class OnLinkStatusChanged
    {
        private Shader shader;
        private List<bool> linkChangedEvents = new List<bool>();

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
            shader = new Shader();
            shader.OnLinkStatusChanged += Shader_OnLinkStatusChanged;
        }

        private void Shader_OnLinkStatusChanged(Shader sender, bool linkStatusIsOk)
        {
            linkChangedEvents.Add(linkStatusIsOk);
        }

        [TestMethod]
        public void ValidFragShader()
        {
            string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validFrag.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            CollectionAssert.AreEqual(new List<bool>() { true }, linkChangedEvents);
        }

        [TestMethod]
        public void ValidInvalidFragShader()
        {
            string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validFrag.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            string shaderSourceInvalid = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.invalidFrag.frag");
            shader.LoadShader(shaderSourceInvalid, ShaderType.FragmentShader);

            CollectionAssert.AreEqual(new List<bool>() { true, false }, linkChangedEvents);
        }
    }
}
