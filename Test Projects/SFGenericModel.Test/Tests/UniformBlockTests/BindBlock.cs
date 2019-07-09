using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.Materials;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Test.UniformBlockTests
{
    [TestClass]
    public class BindBlock
    {
        private UniformBlock uniformBlock = null;
        private Shader shader = null;

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
            uniformBlock = new UniformBlock();
            shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
        }

        [TestMethod]
        public void InvalidBlockName()
        {
            // Shouldn't throw exception.
            uniformBlock.BindingPoint = 0;
            uniformBlock.BindBlock(shader, "memes");
        }

        [TestMethod]
        public void BindingPointZero()
        {
            uniformBlock.BindingPoint = 0;
            uniformBlock.BindBlock(shader, "UniformBlockA");

            int binding = GetBlockBinding("UniformBlockA");
            Assert.AreEqual(uniformBlock.BindingPoint, binding);
        }

        [TestMethod]
        public void BindingPointOne()
        {
            uniformBlock.BindingPoint = 1;
            uniformBlock.BindBlock(shader, "UniformBlockA");

            int binding = GetBlockBinding("UniformBlockA");
            Assert.AreEqual(uniformBlock.BindingPoint, binding);
        }

        private int GetBlockBinding(string name)
        {
            GL.GetActiveUniformBlock(shader.Id,
                shader.GetUniformBlockIndex(name),
                ActiveUniformBlockParameter.UniformBlockBinding, out int binding);
            return binding;
        }
    }
}
