using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace SFGenericModel.Test.RenderSettingsTests
{
    [TestClass]
    public class PolygonModeSettingsEquality
    {
        [TestMethod]
        public void SameObject()
        {
            var settings = new PolygonModeSettings();
            var settings2 = settings;

            Assert.IsTrue(settings.Equals(settings2));
            Assert.IsTrue(settings == settings2);
            Assert.IsFalse(settings != settings2);
        }

        [TestMethod]
        public void DifferentObjects()
        {
            var settings = new PolygonModeSettings();
            var settings2 = new PolygonModeSettings(OpenTK.Graphics.OpenGL.MaterialFace.Front, OpenTK.Graphics.OpenGL.PolygonMode.Point);

            Assert.IsFalse(settings.Equals(settings2));
            Assert.IsFalse(settings == settings2);
            Assert.IsTrue(settings != settings2);
        }
    }
}
