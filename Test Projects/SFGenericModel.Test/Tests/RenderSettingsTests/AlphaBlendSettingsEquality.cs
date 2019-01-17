using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace SFGenericModel.Test.RenderSettingsTests
{
    [TestClass]
    public class AlphaBlendSettingsEquality
    {
        [TestMethod]
        public void SameObject()
        {
            var settings = new AlphaBlendSettings();
            var settings2 = settings;

            Assert.IsTrue(settings.Equals(settings2));
            Assert.IsTrue(settings == settings2);
            Assert.IsFalse(settings != settings2);
        }

        [TestMethod]
        public void DifferentObjects()
        {
            var settings = new AlphaBlendSettings();
            var settings2 = new AlphaBlendSettings(false, 
                OpenTK.Graphics.OpenGL.BlendingFactor.Src1Alpha, 
                OpenTK.Graphics.OpenGL.BlendingFactor.Src1Alpha, 
                OpenTK.Graphics.OpenGL.BlendEquationMode.FuncReverseSubtract,
                OpenTK.Graphics.OpenGL.BlendEquationMode.FuncReverseSubtract);

            Assert.IsFalse(settings.Equals(settings2));
            Assert.IsFalse(settings == settings2);
            Assert.IsTrue(settings != settings2);
        }
    }
}
