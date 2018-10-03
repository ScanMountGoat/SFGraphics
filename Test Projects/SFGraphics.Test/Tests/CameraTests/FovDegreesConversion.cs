using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;

namespace CameraTests
{
    [TestClass]
    public class FovDegreesTest
    {
        [TestMethod]
        public void DegreesToRadiansMaxFov()
        {
            // Value is outside of range and should be ignored.
            Camera camera = new Camera();
            float original = camera.FovRadians;
            camera.FovDegrees = 180;
            Assert.AreEqual(original, camera.FovRadians, 0.001);
        }

        [TestMethod]
        public void DegreesToRadiansMinFov()
        {
            // Value is outside of range and should be ignored.
            Camera camera = new Camera();
            float original = camera.FovRadians;
            camera.FovDegrees = 0;
            Assert.AreEqual(original, camera.FovRadians, 0.001);
        }
    }
}
