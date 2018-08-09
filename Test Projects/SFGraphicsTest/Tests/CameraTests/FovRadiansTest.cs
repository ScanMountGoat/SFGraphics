using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;


namespace SFGraphicsTest.CameraTests
{
    public partial class CameraTest
    {
        [TestClass]
        public class FovRadiansTest
        {
            [TestMethod]
            public void RadiansToDegreesMaxFov()
            {
                // Value is outside of range and should be ignored.
                var camera = new Camera();
                float original = camera.FovDegrees;
                camera.FovRadians = (float)Math.PI;
                Assert.AreEqual(original, camera.FovDegrees, 0.001);
            }

            [TestMethod]
            public void RadiansToDegreesMinFov()
            {
                // Value is outside of range and should be ignored.
                var camera = new Camera();
                float original = camera.FovDegrees;
                camera.FovRadians = 0;
                Assert.AreEqual(original, camera.FovDegrees, 0.001);
            }

            [TestMethod]
            public void RadiansToDegreesInsideRange()
            {
                var camera = new Camera();
                camera.FovRadians = (float)Math.PI / 2.0f;
                Assert.AreEqual(90, camera.FovDegrees, 0.001);
            }
        }
    }
}
