using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class OnLinkStatusChanged
    {
        private Shader shader;
        private List<LinkStatusEventArgs> linkChangedEvents = new List<LinkStatusEventArgs>();

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
            shader = new Shader();
            shader.OnLinkStatusChanged += Shader_OnLinkStatusChanged;
        }

        private void Shader_OnLinkStatusChanged(Shader sender, LinkStatusEventArgs linkStatusIsOk)
        {
            linkChangedEvents.Add(linkStatusIsOk);
        }

        [TestMethod]
        public void ValidFragShader()
        {
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            Assert.AreEqual(1, linkChangedEvents.Count);
            Assert.AreEqual(true, linkChangedEvents[0].LinkStatus);
        }

        [TestMethod]
        public void ValidInvalidFragShader()
        {
            string shaderSource = RenderTestUtils.ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShader(shaderSource, ShaderType.FragmentShader);

            string shaderSourceInvalid = RenderTestUtils.ResourceShaders.GetShaderSource("invalid.frag");
            shader.LoadShader(shaderSourceInvalid, ShaderType.FragmentShader);

            Assert.AreEqual(2, linkChangedEvents.Count);
            Assert.AreEqual(true, linkChangedEvents[0].LinkStatus);
            Assert.AreEqual(false, linkChangedEvents[1].LinkStatus);
        }
    }
}
