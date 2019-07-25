using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Cameras;
using SFGraphics.Utils;

namespace SFGraphics.Test.CameraTests
{
    [TestClass]
    public class RotationMatrix
    {
        [TestMethod]
        public void RotationXDegrees()
        {
            Camera camera = new Camera {RotationXDegrees = 30};
            Assert.AreEqual(Matrix4.CreateRotationX((float)VectorUtils.GetRadians(30)), camera.RotationMatrix);
        }

        [TestMethod]
        public void RotationXRadians()
        {
            Camera camera = new Camera {RotationXRadians = 0.5f};
            Assert.AreEqual(Matrix4.CreateRotationX(0.5f), camera.RotationMatrix);
        }

        [TestMethod]
        public void RotationYDegrees()
        {
            Camera camera = new Camera {RotationYDegrees = 30};
            Assert.AreEqual(Matrix4.CreateRotationY((float)VectorUtils.GetRadians(30)), camera.RotationMatrix);
        }
        [TestMethod]
        public void RotationYRadians()
        {
            Camera camera = new Camera {RotationYRadians = 0.5f};
            Assert.AreEqual(Matrix4.CreateRotationY(0.5f), camera.RotationMatrix);
        }

        [TestMethod]
        public void RotationXYRadians()
        {
            Camera camera = new Camera {RotationYRadians = 0.75f, RotationXRadians = 0.5f};

            Matrix4 expected = Matrix4.CreateRotationY(0.75f) * Matrix4.CreateRotationX(0.5f);
            Assert.AreEqual(expected, camera.RotationMatrix);
        }
    }
}
