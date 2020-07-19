using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Samplers;

namespace SFGraphics.Test.SamplerTests
{
    [TestClass]
    public class SetProperties : GraphicsContextTest
    {
        [TestMethod]
        public void MinFilter()
        {
            var sampler = new SamplerObject { MinFilter = TextureMinFilter.LinearMipmapLinear };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureMinFilter, out int param);
            Assert.AreEqual((int)TextureMinFilter.LinearMipmapLinear, param);
        }

        [TestMethod]
        public void MagFilter()
        {
            var sampler = new SamplerObject { MagFilter = TextureMagFilter.Linear };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureMagFilter, out int param);
            Assert.AreEqual((int)TextureMagFilter.Linear, param);
        }

        [TestMethod]
        public void TextureWrapS()
        {
            var sampler = new SamplerObject { TextureWrapS = TextureWrapMode.MirroredRepeat };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureWrapS, out int param);
            Assert.AreEqual((int)TextureWrapMode.MirroredRepeat, param);
        }

        [TestMethod]
        public void TextureWrapT()
        {
            var sampler = new SamplerObject { TextureWrapT = TextureWrapMode.MirroredRepeat };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureWrapT, out int param);
            Assert.AreEqual((int)TextureWrapMode.MirroredRepeat, param);
        }

        [TestMethod]
        public void TextureWrapR()
        {
            var sampler = new SamplerObject { TextureWrapR = TextureWrapMode.MirroredRepeat };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureWrapR, out int param);
            Assert.AreEqual((int)TextureWrapMode.MirroredRepeat, param);
        }

        [TestMethod]
        public void TextureLodBias()
        {
            var sampler = new SamplerObject { TextureLodBias = -1.234f };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureLodBias, out float param);
            Assert.AreEqual(-1.234f, param);
        }

        [TestMethod]
        public void TextureMaxAnisotropy()
        {
            var sampler = new SamplerObject { TextureMaxAnisotropy = 16f };
            GL.GetSamplerParameter(sampler.Id, SamplerParameterName.TextureMaxAnisotropyExt, out float param);
            Assert.AreEqual(16f, param);
        }
    }
}
