using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddShaderFromSource
    {
        private readonly ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddValidSourceCode()
        {
            Assert.IsTrue(loader.AddShader("validShader", 
                new List<string>() { File.ReadAllText("Shaders/valid.vert") }, 
                new List<string>() { File.ReadAllText("Shaders/valid.frag") }, 
                new List<string>()));
        }

        [TestMethod]
        public void AddValidSourceCodeDuplicate()
        {
            loader.AddShader("validShader",
                new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                new List<string>() { File.ReadAllText("Shaders/valid.frag") },
                new List<string>());

            Assert.IsTrue(loader.AddShader("validShader",
                new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                new List<string>() { File.ReadAllText("Shaders/valid.frag") },
                new List<string>()));
        }

        [TestMethod]
        public void AddInvalidSourceCode()
        {
            Assert.IsFalse(loader.AddShader("validShader",
                new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                new List<string>() { "( ͡° ͜ʖ ͡°)" },
                new List<string>()));
        }
    }
}
