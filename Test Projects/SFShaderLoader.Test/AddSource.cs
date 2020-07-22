using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddSource
    {
        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddValidSourceFrag()
        {
            var loader = new ShaderLoader();
            Assert.IsTrue(loader.AddSource("frag1", File.ReadAllText("Shaders/valid.frag"), ShaderType.FragmentShader));
        }

        [TestMethod]
        public void AddValidSourceVert()
        {
            var loader = new ShaderLoader();
            Assert.IsTrue(loader.AddSource("vert1", File.ReadAllText("Shaders/valid.vert"), ShaderType.VertexShader));
        }

        [TestMethod]
        public void AddValidSourceDuplicate()
        {
            var loader = new ShaderLoader();
            Assert.IsTrue(loader.AddSource("frag1", File.ReadAllText("Shaders/valid.frag"), ShaderType.FragmentShader));
            Assert.IsTrue(loader.AddSource("frag1", File.ReadAllText("Shaders/valid.frag"), ShaderType.FragmentShader));
        }

        [TestMethod]
        public void AddInvalidSource()
        {
            var loader = new ShaderLoader();
            Assert.IsFalse(loader.AddSource("frag1", @"¯\_( ͡° ͜ʖ ͡°)_/¯", ShaderType.FragmentShader));
        }
    }
}
