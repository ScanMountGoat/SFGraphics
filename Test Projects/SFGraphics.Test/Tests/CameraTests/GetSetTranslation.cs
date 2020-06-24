using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;

namespace SFGraphics.Test.Tests.CameraTests
{
    [TestClass]
    public class GetSetTranslation
    {
        [TestMethod]
        public void GetSetPropValues()
        {
            var camera = new Camera { Translation = new OpenTK.Vector3(1, 2, 3) };
            Assert.AreEqual(1, camera.TranslationX);
            Assert.AreEqual(2, camera.TranslationY);
            Assert.AreEqual(3, camera.TranslationZ);
            camera.TranslationX = 3;
            camera.TranslationY = 2;
            camera.TranslationZ = 1;
            Assert.AreEqual(new OpenTK.Vector3(3, 2, 1), camera.Translation);
        }
    }
}
