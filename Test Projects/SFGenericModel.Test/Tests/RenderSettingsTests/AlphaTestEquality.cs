using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace RenderSettingsTests
{
    [TestClass]
    public class AlphaTestSettingsEquality
    {
        [TestMethod]
        public void SameObject()
        {
            var settings = new AlphaTestSettings();
            var settings2 = settings;

            Assert.IsTrue(settings.Equals(settings2));
            Assert.IsTrue(settings == settings2);
            Assert.IsFalse(settings != settings2);
        }

        [TestMethod]
        public void DifferentObjects()
        {
            var settings = new AlphaTestSettings();
            var settings2 = new AlphaTestSettings(false, OpenTK.Graphics.OpenGL.AlphaFunction.Equal, 1);

            Assert.IsFalse(settings.Equals(settings2));
            Assert.IsFalse(settings == settings2);
            Assert.IsTrue(settings != settings2);
        }
    }
}
