using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.ShaderTests
{
    [TestClass]
    public class GetProgramBinary : Tests.ContextTest
    {
        [TestMethod]
        public void ValidProgram()
        {
            var shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
            Assert.IsTrue(shader.GetProgramBinary(out byte[] binary, out BinaryFormat binaryFormat));
        }

        [TestMethod]
        public void InvalidProgram()
        {
            var shader = new GLObjects.Shaders.Shader();
            Assert.IsFalse(shader.GetProgramBinary(out byte[] binary, out BinaryFormat binaryFormat));
        }
    }
}
