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
            if (shader == null)
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();

            uniformBlock = new UniformBlock(shader, "UniformBlockA");
        }

        [TestMethod]
        public void InvalidBlockName()
        {
            // Shouldn't throw exception.
            uniformBlock.BlockBinding = 0;
            uniformBlock.BindBlock(shader, "memes");
        }

        [TestMethod]
        public void BindingPointZero()
        {
            uniformBlock.BlockBinding = 0;
            uniformBlock.BindBlock(shader, "UniformBlockA");

            int binding = GetBlockBinding("UniformBlockA");
            Assert.AreEqual(uniformBlock.BlockBinding, binding);
        }

        [TestMethod]
        public void BindingPointOne()
        {
            uniformBlock.BlockBinding = 1;
            uniformBlock.BindBlock(shader, "UniformBlockA");

            int binding = GetBlockBinding("UniformBlockA");
            Assert.AreEqual(uniformBlock.BlockBinding, binding);
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
