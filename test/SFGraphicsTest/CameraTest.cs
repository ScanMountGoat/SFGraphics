using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


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
}
