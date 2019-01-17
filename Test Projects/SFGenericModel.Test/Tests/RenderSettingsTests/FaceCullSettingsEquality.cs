using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace SFGenericModel.Test.RenderSettingsTests
{
    [TestClass]
    public class FaceCullingSettingsEquality
    {
        [TestMethod]
        public void SameObject()
        {
            var settings = new FaceCullingSettings();
            var settings2 = settings;

            Assert.IsTrue(settings.Equals(settings2));
            Assert.IsTrue(settings == settings2);
            Assert.IsFalse(settings != settings2);
        }

        [TestMethod]
        public void DifferentObjects()
        {
            var settings = new FaceCullingSettings();
            var settings2 = new FaceCullingSettings(false, OpenTK.Graphics.OpenGL.CullFaceMode.Front);

            Assert.IsFalse(settings.Equals(settings2));
            Assert.IsFalse(settings == settings2);
            Assert.IsTrue(settings != settings2);
        }
    }
}
