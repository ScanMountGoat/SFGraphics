using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGenericModel.Materials;
using SFGraphics.GLObjects.Shaders;
using System;

namespace SFGenericModel.Test.UniformBlockTests
{
    [TestClass]
    public class SetValue
    {
        private struct TestStruct
        {
            public readonly long val1;
            public readonly long val2;
            public readonly long val3;
            public readonly long val4;
            public readonly long val5;
        }

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
        public void ValidNameLargerThanBlock()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA");
            var e = Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
                uniformBlock.SetValue("blockAFloat", new TestStruct()));
        }

        [TestMethod]
        public void ValidNameSmallerThanType()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA");
            Assert.IsTrue(uniformBlock.SetValue("blockAFloat", (byte)128));
        }

        [TestMethod]
        public void InvalidName()
        {
            var uniformBlock = new UniformBlock(shader, "UniformBlockA");
            Assert.IsFalse(uniformBlock.SetValue("ಠ_ಠ", 1.5f));
        }
    }
}
