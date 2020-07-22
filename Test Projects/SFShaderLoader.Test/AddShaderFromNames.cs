using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddShaderFromNames
    {
        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddAllSources()
        {
            var loader = new ShaderLoader();
            loader.AddSource("a", File.ReadAllText("Shaders/FunctionA.glsl"), ShaderType.FragmentShader);
            loader.AddSource("b", File.ReadAllText("Shaders/MultipleFunctions.frag"), ShaderType.FragmentShader);
            Assert.IsTrue(loader.AddShader("shader", "a", "b"));
            Assert.IsTrue(loader.GetShader("shader").LinkStatusIsOk);
        }

        [TestMethod]
        public void MissingSource()
        {
            var loader = new ShaderLoader();
            loader.AddSource("b", File.ReadAllText("Shaders/MultipleFunctions.frag"), ShaderType.FragmentShader);
            Assert.IsFalse(loader.AddShader("shader", "b"));
        }

        [TestMethod]
        public void InvalidKey()
        {
            var loader = new ShaderLoader();
            loader.AddSource("b", File.ReadAllText("Shaders/MultipleFunctions.frag"), ShaderType.FragmentShader);
            Assert.IsFalse(loader.AddShader("shader", "b", "(╯ຈل͜ຈ) ╯︵ ┻━┻"));
        }
    }
}
