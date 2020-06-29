using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;

namespace SFGenericModel.Test.GenericMeshNonInterleavedTests
{
    [TestClass]
    public class AddBuffer
    {
        private static readonly int vertexCount = 8;

        private class MeshA : GenericMeshNonInterleaved
        {
            public MeshA() : base(new int[16], PrimitiveType.Triangles, DrawElementsType.UnsignedInt, vertexCount) { }
        }

        [TestInitialize]
        public void Initialize()
        {
            OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddBufferNoExistingBuffers()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[8]);
        }

        [TestMethod]
        public void AddMultipleBuffers()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[8]);
            mesh.AddBuffer("buffer2", new byte[8]);
            mesh.AddBuffer("buffer3", new byte[8]);
        }

        [TestMethod]
        public void AddDuplicateBuffer()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", new byte[8]);
            var e = Assert.ThrowsException<System.ArgumentException>(() => mesh.AddBuffer("buffer1", new byte[8]));
            Assert.IsTrue(e.Message.Contains("A buffer with the given name already exists."));
        }
    }
}
