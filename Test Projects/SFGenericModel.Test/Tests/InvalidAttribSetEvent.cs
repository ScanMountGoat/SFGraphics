using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.VertexAttributes;

namespace SFGenericModel.Test.GenericMeshTests
{
    [TestClass]
    public class AttribSet
    {
        private class TestMesh : GenericMesh<float>
        {
            public List<VertexAttribute> vertexAttributes = new List<VertexAttribute>();
            public TestMesh() : base(new List<float>(), PrimitiveType.Lines)
            {

            }

            public override List<VertexAttribute> GetVertexAttributes()
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

        private void Mesh_OnInvalidAttribSet(object sender, AttribSetEventArgs e)
        {
            eventArgs.Add(e);
        }

        [TestMethod]
        public void ValidAttributeLocation()
        {
            mesh.vertexAttributes.Add(new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float, false));
            mesh.ConfigureVertexAttributes(shader);
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void ValidAttributeIntLocation()
        {
            mesh.vertexAttributes.Add(new VertexIntAttribute("intAttrib", ValueCount.One, VertexAttribIntegerType.Int));
            mesh.ConfigureVertexAttributes(shader);
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidAttributeIntLocation()
        {
            mesh.vertexAttributes.Add(new VertexIntAttribute("memes", ValueCount.One, VertexAttribIntegerType.Int));
            mesh.ConfigureVertexAttributes(shader);
            Assert.AreEqual(1, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidAttributeLocation()
        {
            mesh.vertexAttributes.Add(new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float, false));
            mesh.vertexAttributes.Add(new VertexFloatAttribute("memes", ValueCount.Three, VertexAttribPointerType.Float, false));
            mesh.ConfigureVertexAttributes(shader);
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}
