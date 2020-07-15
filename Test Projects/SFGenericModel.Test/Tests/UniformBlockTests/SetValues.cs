using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.Materials;
using SFGraphics.GLObjects.Shaders;

namespace SFGenericModel.Test.UniformBlockTests
{
    [TestClass]
    public class SetValues
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
            Assert.IsTrue(uniformBlock.SetValues("blockAVec4", new float[4]));
        }

        [TestMethod]
        public void InvalidName()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA");
            Assert.IsFalse(uniformBlock.SetValues("ಠ_ಠ", new float[4]));
        }
    }
}
