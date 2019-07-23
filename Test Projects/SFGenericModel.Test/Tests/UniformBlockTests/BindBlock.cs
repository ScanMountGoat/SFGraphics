using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.Materials;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Test.UniformBlockTests
{
    [TestClass]
    public class BindBlock
    {
        private Shader shader;

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
            if (shader == null)
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();

        }

        [TestMethod]
        public void InvalidBlockName()
        {
            // Shouldn't throw exception.
            var e = Assert.ThrowsException<ArgumentException>(() =>
            {
                var uniformBlock = new UniformBlock(shader, "memes")
                {
                    BlockBinding = 0
                };
                uniformBlock.BindBlock(shader);
            });
            Assert.AreEqual("uniformBlockName", e.ParamName);
            Assert.IsTrue(e.Message.Contains("memes is not an active uniform block in the specified shader."));
        }

        [TestMethod]
        public void BindingPointZero()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA")
            {
                BlockBinding = 0
            };
            uniformBlock.BindBlock(shader);

            int binding = GetBlockBinding("UniformBlockA");
            Assert.AreEqual(uniformBlock.BlockBinding, binding);
        }

        [TestMethod]
        public void BindingPointOne()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA")
            {
                BlockBinding = 1
            };
            uniformBlock.BindBlock(shader);

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
