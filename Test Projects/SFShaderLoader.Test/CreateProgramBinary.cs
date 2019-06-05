using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class CreateProgramBinary
    {
        private readonly ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void ValidNameValidShader()
        {
            loader.AddShader("validShader",
                            new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                            new List<string>() { File.ReadAllText("Shaders/valid.frag") },
                            new List<string>());

            loader.CreateProgramBinary("validShader", out byte[] shaderBinary, out BinaryFormat binaryFormat);
            var shader = new Shader();
            shader.LoadProgramBinary(shaderBinary, binaryFormat);

            Assert.IsTrue(shader.LinkStatusIsOk);
        }

        [TestMethod]
        public void InvalidName()
        {
            loader.AddShader("validShader",
                            new List<string>() { File.ReadAllText("Shaders/valid.vert") },
                            new List<string>() { File.ReadAllText("Shaders/valid.frag") },
                            new List<string>());

            var e = Assert.ThrowsException<System.ArgumentException>(() => 
                loader.CreateProgramBinary("( ͡° ͜ʖ ͡°)", out byte[] shaderBinary, out BinaryFormat binaryFormat));
            Assert.AreEqual("shaderName", e.ParamName);
        }
    }
}
