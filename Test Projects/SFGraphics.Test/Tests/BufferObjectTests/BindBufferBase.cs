using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace BufferObjectTests
{
    [TestClass]
    public class BindBufferBase : BufferTest
    {
        [TestMethod]
        public void InvalidTarget()
        {
            // Shouldn't throw exception.
            buffer.BindBase(BufferRangeTarget.UniformBuffer, -1);
        }
    }
}
