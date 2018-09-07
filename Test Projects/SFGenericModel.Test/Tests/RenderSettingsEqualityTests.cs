using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace SFGenericModel.Test
{
    [TestClass]
    public class RenderSettingsEqualityTests
    {
        [TestMethod]
        public void SameObject()
        {
            RenderSettings settings = new RenderSettings();
            Assert.IsTrue(settings.Equals(settings));
        }

        [TestMethod]
        public void DefaultSettings()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();

            Assert.IsTrue(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentAlphaTest()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.alphaTestSettings.enableAlphaTesting = !settings.alphaTestSettings.enableAlphaTesting;
            Assert.IsFalse(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentFaceCull()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.faceCullingSettings.enableFaceCulling = !settings.faceCullingSettings.enableFaceCulling;

            Assert.IsFalse(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentAlphaBlend()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.alphaBlendSettings.enableAlphaBlending = !settings.alphaBlendSettings.enableAlphaBlending;

            Assert.IsFalse(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentDepthTest()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.depthTestSettings.enableDepthTest = !settings.depthTestSettings.enableDepthTest;

            Assert.IsFalse(settings.Equals(settings2));
        }
    }
}
