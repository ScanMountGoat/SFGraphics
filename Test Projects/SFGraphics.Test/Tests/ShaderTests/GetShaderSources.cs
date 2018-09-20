using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace ShaderTests
{
    [TestClass]
    public class GetShaderSources : Tests.ContextTest
    {
        [TestMethod]
        public void NoShaders()
        {
            Shader shader = new Shader();
            string[] sources = shader.GetShaderSources();
            Assert.AreEqual(0, sources.Length);
        }

        [TestMethod]
        public void EmptyShaders()
        {
            Shader shader = new Shader();
            shader.LoadShader("", ShaderType.FragmentShader);
            shader.LoadShader("", ShaderType.FragmentShader);
            shader.LoadShader("", ShaderType.FragmentShader);

            string[] sources = shader.GetShaderSources();
            Assert.AreEqual(3, sources.Length);
        }

        [TestMethod]
        public void InvalidShaders()
        {
            Shader shader = new Shader();
            shader.LoadShader("a", ShaderType.FragmentShader);
            shader.LoadShader("b", ShaderType.FragmentShader);
            shader.LoadShader("c", ShaderType.FragmentShader);

            string[] sources = shader.GetShaderSources();
            Assert.AreEqual(3, sources.Length);

            // The order won't always be the same
            Assert.IsTrue(sources.Contains("a"));
            Assert.IsTrue(sources.Contains("b"));
            Assert.IsTrue(sources.Contains("c"));
        }
    }
}
