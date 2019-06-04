using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SFShaderLoader.Test
{
    [TestClass]
    public class GetShader
    {
        private readonly ShaderLoader loader = new ShaderLoader();

        [TestInitialize]
        public void Setup()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            // TODO: Use actual shader files.
            loader.AddShader("validShader", new List<string>(), new List<string>(), new List<string>());
        }

        [TestMethod]
        public void GetInvalidName()
        {
            Assert.IsNull(loader.GetShader("invalidShader"));
        }

        [TestMethod]
        public void GetValidName()
        {
            Assert.IsNotNull(loader.GetShader("validShader"));
        }
    }
}
