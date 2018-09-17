using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using SFGenericModel;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.VertexAttributes;

namespace GenericMeshTests
{
    [TestClass]
    public class AttribSet
    {
        private class TestMesh : GenericMesh<float>
        {
            public List<VertexAttributeInfo> vertexAttributes = new List<VertexAttributeInfo>();
            public TestMesh() : base(new List<float>(), PrimitiveType.Lines)
            {

            }

            public override List<VertexAttributeInfo> GetVertexAttributes()
            {
                return vertexAttributes;
            }
        }

        private Shader shader;
        private TestMesh mesh;
        private List<AttribSetEventArgs> eventArgs = new List<AttribSetEventArgs>();

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            if (shader == null)
            {
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
            }
            mesh = new TestMesh();
            mesh.OnInvalidAttribSet += Mesh_OnInvalidAttribSet;
            eventArgs.Clear();
        }

        private void Mesh_OnInvalidAttribSet(GenericMesh<float> sender, AttribSetEventArgs e)
        {
            eventArgs.Add(e);
        }

        [TestMethod]
        public void ValidAttributeLocation()
        {
            mesh.vertexAttributes.Add(new VertexAttributeInfo("position", ValueCount.Three, VertexAttribPointerType.Float));
            mesh.ConfigureVertexAttributes(shader);
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidAttributeLocation()
        {
            mesh.vertexAttributes.Add(new VertexAttributeInfo("position", ValueCount.Three, VertexAttribPointerType.Float));
            mesh.vertexAttributes.Add(new VertexAttributeInfo("memes", ValueCount.Three, VertexAttribPointerType.Float));
            mesh.ConfigureVertexAttributes(shader);
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
