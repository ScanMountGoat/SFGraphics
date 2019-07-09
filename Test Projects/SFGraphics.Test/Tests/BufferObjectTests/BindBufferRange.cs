using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.BufferObjectTests
{
    [TestClass]
    public class BindBufferRange : BufferTest
    {
        [TestMethod]
        public void InvalidTarget()
        {
            // Shouldn't throw exception.
            buffer.BindRange(BufferRangeTarget.UniformBuffer, -1, 0, 0);
        }
    }
}
