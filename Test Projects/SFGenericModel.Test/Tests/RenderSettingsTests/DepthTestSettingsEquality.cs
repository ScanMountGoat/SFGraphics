using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace SFGenericModel.Test.RenderSettingsTests
{
    [TestClass]
    public class DepthTestSettingsEquality
    {
        [TestMethod]
        public void SameObject()
        {
            var settings = new DepthTestSettings();
            var settings2 = settings;

            Assert.IsTrue(settings.Equals(settings2));
            Assert.IsTrue(settings == settings2);
            Assert.IsFalse(settings != settings2);
        }

        [TestMethod]
        public void DifferentObjects()
        {
            var settings = new DepthTestSettings();
            var settings2 = new DepthTestSettings(false, false, OpenTK.Graphics.OpenGL.DepthFunction.Always);

            Assert.IsFalse(settings.Equals(settings2));
            Assert.IsFalse(settings == settings2);
            Assert.IsTrue(settings != settings2);
        }
    }
}
