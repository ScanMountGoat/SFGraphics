using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGenericModel.Materials;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Test.UniformBlockTests
{
    [TestClass]
    public class SetFloat
    {
        private UniformBlock uniformBlock = null;
        private Shader shader = null;

        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            if (shader == null)
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();

            uniformBlock = new UniformBlock
            {
                BindingPoint = 0
            };
            uniformBlock.BindBlock(shader, "UniformBlockA");
        }

        [TestMethod]
        public void InvalidName()
        {
            // Shouldn't throw exception.
            uniformBlock.SetFloat("memes", 0.75f);
        }

        [TestMethod]
        public void ValidName()
        {
            uniformBlock.SetFloat("blockAFloat", 0.75f);
            // TODO: How to test this?
        }
    }
}
