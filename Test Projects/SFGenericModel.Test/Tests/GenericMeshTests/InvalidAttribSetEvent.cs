using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.MeshEventArgs;
using SFGenericModel.VertexAttributes;

namespace SFGenericModel.Test.GenericMeshTests
{
    [TestClass]
    public class AttribSet
    {
        private struct TestVertex
        {
            [VertexFloatAttribute(nameof(position), ValueCount.Three, VertexAttribPointerType.Float, false)]
            public readonly OpenTK.Vector3 position;

            [VertexIntAttribute(nameof(intAttrib), ValueCount.One, VertexAttribIntegerType.Int)]
            public readonly int intAttrib;

            // These attribute properties are properly configured, but the names aren't present in the vertex shader.
            [VertexIntAttribute("( ͡° ͜ʖ ͡°)", ValueCount.One, VertexAttribIntegerType.Int)]
            public readonly int invalidIntAttribName;

            [VertexFloatAttribute("(ಠ_ಠ)", ValueCount.One, VertexAttribPointerType.Float, false)]
            public readonly float invalidFloatAttribName;
        }

        private class TestMesh : GenericMesh<TestVertex>
        {
            public TestMesh() : base(new List<TestVertex>(), PrimitiveType.Lines)
            {

            }
        }

        private Shader shader;
        private TestMesh mesh;
        private readonly List<AttribSetEventArgs> eventArgs = new List<AttribSetEventArgs>();

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
            mesh = new TestMesh();

            mesh.OnInvalidAttribSet += Mesh_OnInvalidAttribSet;
        }

        [TestMethod]
        public void ConfigureVertexAttributes()
        {
            mesh.Draw(shader);

            Assert.AreEqual(2, eventArgs.Count);

            // The enums are the same, so don't specifically check for integer types.
            Assert.AreEqual("( ͡° ͜ʖ ͡°)", eventArgs[0].Name);
            Assert.AreEqual(VertexAttribPointerType.Int, eventArgs[0].Type);
            Assert.AreEqual(ValueCount.One, eventArgs[0].ValueCount);

            Assert.AreEqual("(ಠ_ಠ)", eventArgs[1].Name);
            Assert.AreEqual(VertexAttribPointerType.Float, eventArgs[1].Type);
            Assert.AreEqual(ValueCount.One, eventArgs[1].ValueCount);
        }

        private void Mesh_OnInvalidAttribSet(object sender, AttribSetEventArgs e)
        {
            eventArgs.Add(e);
        }
    }
}
