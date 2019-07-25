using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using Tests;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class GetShaderSources : ContextTest
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
            // Empty shaders are ignored.
            Shader shader = new Shader();
            shader.LoadShader("", ShaderType.FragmentShader);
            shader.LoadShader(null, ShaderType.FragmentShader);
            shader.LoadShader("", ShaderType.FragmentShader);

            string[] sources = shader.GetShaderSources();
            Assert.AreEqual(0, sources.Length);
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
