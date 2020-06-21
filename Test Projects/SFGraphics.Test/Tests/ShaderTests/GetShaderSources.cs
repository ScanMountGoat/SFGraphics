using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class GetShaderSources : GraphicsContextTest
    {
        [TestMethod]
        public void NoShaders()
        {
            Shader shader = new Shader();

            // The order may not be the same.
            CollectionAssert.AreEquivalent(new string[0], shader.GetShaderSources());
        }

        [TestMethod]
        public void EmptyShaders()
        {
            Shader shader = new Shader();
            shader.LoadShaders(
                new ShaderObject("", ShaderType.FragmentShader), 
                new ShaderObject(null, ShaderType.FragmentShader));

            // The order may not be the same.
            CollectionAssert.AreEquivalent(new string[] { "", "" }, shader.GetShaderSources());
        }

        [TestMethod]
        public void InvalidShaders()
        {
            Shader shader = new Shader();
            shader.LoadShaders(
                new ShaderObject("a", ShaderType.FragmentShader),
                new ShaderObject("b", ShaderType.FragmentShader),
                new ShaderObject("c", ShaderType.FragmentShader));

            // The order may not be the same.
            CollectionAssert.AreEquivalent(new string[] { "a", "b", "c" }, shader.GetShaderSources());
        }
    }
}
