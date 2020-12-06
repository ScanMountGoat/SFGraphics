using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddShaderFromSource
    {
        private ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            loader = new ShaderLoader();
        }

        [TestMethod]
        public void AddValidSourceCode()
        {
            Assert.IsTrue(loader.AddShader("validShader", 
                new List<string>() { File.ReadAllText("Shaders/valid.vert") }, 
                new List<string>() { File.ReadAllText("Shaders/valid.frag") }, 
                new List<string>()));

            CollectionAssert.AreEqual(new string[] { "validShader" }, loader.ShaderNames.ToArray());
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

            CollectionAssert.AreEqual(new string[] { "validShader" }, loader.ShaderNames.ToArray());
        }

        [TestMethod]
        public void AddInvalidSourceCode()
        {
            Assert.IsFalse(loader.AddShader("invalidShader1",
                new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                new List<string>() { "( ͡° ͜ʖ ͡°)" },
                new List<string>()));

            Assert.IsFalse(loader.AddShader("invalidShader2",
                new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                new List<string>() { "( ͡° ͜ʖ ͡°)" },
                new List<string>()));

            CollectionAssert.AreEqual(new string[] { "invalidShader1", "invalidShader2" }, loader.ShaderNames.ToArray());
        }
    }
}
