using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using RenderTestUtils;
using SFGraphics.GLObjects.BufferObjects;

namespace SFGenericModel.Test.GenericMeshNonInterleavedTests
{
    [TestClass]
    public class AddBufferFromExisting
    {
        private static readonly int vertexCount = 8;
        private static BufferObject meshBuffer;

        private class MeshA : GenericMeshNonInterleaved
        {
            public MeshA() : base(new uint[16], PrimitiveType.Triangles, vertexCount) { }
        }

        [TestInitialize]
        public void Initialize()
        {
            OpenTKWindowlessContext.BindDummyContext();
            meshBuffer = new BufferObject(BufferTarget.ArrayBuffer);
            meshBuffer.SetData(new byte[8], BufferUsageHint.StaticDraw);
        }

        [TestMethod]
        public void AddBufferNoExistingBuffers()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", meshBuffer);
        }

        [TestMethod]
        public void AddMultipleBuffers()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", meshBuffer);
            mesh.AddBuffer("buffer2", meshBuffer);
            mesh.AddBuffer("buffer3", meshBuffer);
        }

        [TestMethod]
        public void AddDuplicateBuffer()
        {
            var mesh = new MeshA();
            mesh.AddBuffer("buffer1", meshBuffer);
            var e = Assert.ThrowsException<System.ArgumentException>(() => mesh.AddBuffer("buffer1", meshBuffer));
            Assert.IsTrue(e.Message.Contains("A buffer with the given name already exists."));
        }
    }
}
