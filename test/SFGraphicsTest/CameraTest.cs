using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;

namespace SFGraphicsTest
{
    [TestClass]
    public class CameraTest
    {
        [TestClass]
        public class FovRadiansTest
        {
            [TestMethod]
            public void RadiansToDegreesMaxFov()
            {
                // Value is outside of range and should be ignored.
                Camera camera = new Camera();
                float original = camera.FovDegrees;
                camera.FovRadians = (float)Math.PI;
                Assert.AreEqual(original, camera.FovDegrees, 0.001);
            }

            [TestMethod]
            public void RadiansToDegreesMinFov()
            {
                // Value is outside of range and should be ignored.
                Camera camera = new Camera();
                float original = camera.FovDegrees;
                camera.FovRadians = 0;
                Assert.AreEqual(original, camera.FovDegrees, 0.001);
            }
        }

        [TestClass]
        public class FovDegreesTest
        {


        }
    }
}
