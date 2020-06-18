using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddShaderFromBinary
    {
        private readonly ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddValidBinary()
        {
            var shader = new Shader();
            shader.LoadShaders(File.ReadAllText("Shaders/valid.vert"), File.ReadAllText("Shaders/valid.frag"));
            shader.GetProgramBinary(out byte[] binary, out BinaryFormat format);

            Assert.IsTrue(loader.AddShader("validShader", binary, format));
        }

        [TestMethod]
        public void AddInvalidBinary()
        {
            var shader = new Shader();
            shader.LoadShaders(File.ReadAllText("Shaders/valid.vert"), File.ReadAllText("Shaders/valid.frag"));
            shader.GetProgramBinary(out byte[] binary, out BinaryFormat format);

            // Use an empty binary to try and trigger an exception with GL.ProgramBinary.
            Assert.IsFalse(loader.AddShader("validShader", new byte[0], format));
        }
    }
}
