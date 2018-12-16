using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;

namespace CameraTests
{
    [TestClass]
    public class ViewVector
    {
        private static readonly float delta = 0.01f;

        [TestMethod]
        public void LookForward()
        {
            Camera camera = new Camera();
            Assert.AreEqual(0, camera.ViewVector.X, delta);
            Assert.AreEqual(0, camera.ViewVector.Y, delta);
            Assert.AreEqual(1, camera.ViewVector.Z, delta);
        }

        [TestMethod]
        public void LookForwardTranslated()
        {
            Camera camera = new Camera()
            {
                Position = new Vector3(5, -15, 20)
            };

            Assert.AreEqual(0, camera.ViewVector.X, delta);
            Assert.AreEqual(0, camera.ViewVector.Y, delta);
            Assert.AreEqual(1, camera.ViewVector.Z, delta);
        }

        [TestMethod]
        public void LookFromAbove()
        {
            Camera camera = new Camera()
            {
                RotationXDegrees = 90
            };

            Assert.AreEqual(0, camera.ViewVector.X, delta);
            Assert.AreEqual(1, camera.ViewVector.Y, delta);
            Assert.AreEqual(0, camera.ViewVector.Z, delta);
        }

        [TestMethod]
        public void LookFromAboveRotated()
        {
            // Rotating in the other axis shouldn't have any effect.
            Camera camera = new Camera()
            {
                RotationXDegrees = 90,
                RotationYDegrees = 90
            };

            Assert.AreEqual(0, camera.ViewVector.X, delta);
            Assert.AreEqual(1, camera.ViewVector.Y, delta);
            Assert.AreEqual(0, camera.ViewVector.Z, delta);
        }

        [TestMethod]
        public void LookFromRight()
        {
            Camera camera = new Camera()
            {
                RotationYDegrees = -90
            };

            Assert.AreEqual(1, camera.ViewVector.X, delta);
            Assert.AreEqual(0, camera.ViewVector.Y, delta);
            Assert.AreEqual(0, camera.ViewVector.Z, delta);
        }
    }
}
