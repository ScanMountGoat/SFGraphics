using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGenericModel.VertexAttributes;
using SFGenericModel.ShaderGenerators;
using OpenTK.Graphics.OpenGL;

namespace ShaderGeneratorTests
{
    [TestClass]
    public class TextureShaderGenerator
    {
        [TestInitialize]
        public void Initialize()
        {
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();
        }

        [TestMethod]
        public void NoTextures()
        {
            var pos = new VertexAttributeInfo("pos", ValueCount.Four, VertexAttribPointerType.Float);
            var uv0 = new VertexAttributeInfo("uv", ValueCount.Four, VertexAttribPointerType.Float);
            Shader shader = SFGenericModel.ShaderGenerators.TextureShaderGenerator.CreateShader(new List<TextureRenderInfo>(), pos, uv0);
            Assert.IsTrue(shader.LinkStatusIsOk);
        }
    }
}
