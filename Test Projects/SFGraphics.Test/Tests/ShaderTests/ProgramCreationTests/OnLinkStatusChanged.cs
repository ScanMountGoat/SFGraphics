using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace SFGraphics.Test.ShaderTests.ProgramCreationTests
{
    [TestClass]
    public class OnLinkStatusChanged : GraphicsContextTest
    {
        private Shader shader;
        private readonly List<LinkStatusEventArgs> linkChangedEvents = new List<LinkStatusEventArgs>();
        
        [TestInitialize]
        public override void Initialize()
        {
            // Set up the context for all the tests.
            base.Initialize();
            shader = new Shader();
            shader.LinkStatusChanged += Shader_OnLinkStatusChanged;
        }

        private void Shader_OnLinkStatusChanged(object sender, LinkStatusEventArgs e)
        {
            linkChangedEvents.Add(e);
        }

        [TestMethod]
        public void ValidFragShader()
        {
            string shaderSource = ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShaders(new ShaderObject(shaderSource, ShaderType.FragmentShader));

            Assert.AreEqual(1, linkChangedEvents.Count);
            Assert.AreEqual(true, linkChangedEvents[0].LinkStatus);
        }

        [TestMethod]
        public void ValidInvalidFragShader()
        {
            string shaderSource = ResourceShaders.GetShaderSource("valid.frag");
            shader.LoadShaders(new ShaderObject(shaderSource, ShaderType.FragmentShader));

            string shaderSourceInvalid = ResourceShaders.GetShaderSource("invalid.frag");
            shader.LoadShaders(new ShaderObject(shaderSourceInvalid, ShaderType.FragmentShader));

            Assert.AreEqual(2, linkChangedEvents.Count);
            Assert.AreEqual(true, linkChangedEvents[0].LinkStatus);
            Assert.AreEqual(false, linkChangedEvents[1].LinkStatus);
        }
    }
}
