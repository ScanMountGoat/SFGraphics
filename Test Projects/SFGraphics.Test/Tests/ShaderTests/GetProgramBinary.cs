using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class GetProgramBinary : GraphicsContextTest
    {
        [TestMethod]
        public void ValidProgram()
        {
            var shader = ShaderTestUtils.CreateValidShader();
            Assert.IsTrue(shader.GetProgramBinary(out byte[] binary, out BinaryFormat binaryFormat));
        }

        [TestMethod]
        public void InvalidProgram()
        {
            var shader = new Shader();
            Assert.IsFalse(shader.GetProgramBinary(out byte[] binary, out BinaryFormat binaryFormat));
        }
    }
}
