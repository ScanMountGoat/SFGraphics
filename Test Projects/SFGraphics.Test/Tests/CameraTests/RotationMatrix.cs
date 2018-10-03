using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;

namespace CameraTests
{
    [TestClass]
    public class RotationMatrix
    {
        [TestMethod]
        public void RotationXDegrees()
        {
            Camera camera = new Camera();
            camera.RotationXDegrees = 30;
            Assert.AreEqual(Matrix4.CreateRotationX((float)SFGraphics.Utils.VectorUtils.GetRadians(30)), camera.RotationMatrix);
        }

        [TestMethod]
        public void RotationXRadians()
        {
            Camera camera = new Camera();
            camera.RotationXRadians = 0.5f;
            Assert.AreEqual(Matrix4.CreateRotationX(0.5f), camera.RotationMatrix);
        }

        [TestMethod]
        public void RotationYDegrees()
        {
            Camera camera = new Camera();
            camera.RotationYDegrees = 30;
            Assert.AreEqual(Matrix4.CreateRotationY((float)SFGraphics.Utils.VectorUtils.GetRadians(30)), camera.RotationMatrix);
        }
        [TestMethod]
        public void RotationYRadians()
        {
            Camera camera = new Camera();
            camera.RotationYRadians = 0.5f;
            Assert.AreEqual(Matrix4.CreateRotationY(0.5f), camera.RotationMatrix);
        }

        [TestMethod]
        public void RotationXYRadians()
        {
            Camera camera = new Camera();
            camera.RotationYRadians = 0.75f;
            camera.RotationXRadians = 0.5f;

            Matrix4 expected = Matrix4.CreateRotationY(0.75f) * Matrix4.CreateRotationX(0.5f);
            Assert.AreEqual(expected, camera.RotationMatrix);
        }
    }
}
