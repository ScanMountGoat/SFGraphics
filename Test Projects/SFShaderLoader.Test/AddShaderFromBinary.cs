using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class AddShaderFromBinary
    {
        private readonly ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void AddValidBinary()
        {
            // TODO: Use actual shader files.
            Assert.IsTrue(loader.AddShader("validShader", new List<BinaryShaderData>(), new List<BinaryShaderData>(), new List<BinaryShaderData>()));
        }

        [TestMethod]
        public void AddInvalidBinary()
        {
            // TODO: Use actual shader files.
            Assert.IsFalse(loader.AddShader("validShader", new List<BinaryShaderData>(), new List<BinaryShaderData>(), new List<BinaryShaderData>()));
        }
    }
}
