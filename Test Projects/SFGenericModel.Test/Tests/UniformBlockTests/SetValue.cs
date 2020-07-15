using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.Materials;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Test.UniformBlockTests
{
    [TestClass]
    public class SetValue
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
        public void ValidName()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA");
            Assert.IsTrue(uniformBlock.SetValue("blockAFloat", 1.5f));
        }

        [TestMethod]
        public void InvalidName()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA");
            Assert.IsFalse(uniformBlock.SetValue("ಠ_ಠ", 1.5f));
        }
    }
}
