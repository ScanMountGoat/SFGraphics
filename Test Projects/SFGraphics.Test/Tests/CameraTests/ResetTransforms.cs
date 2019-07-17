using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Cameras;

namespace SFGraphics.Test.CameraTests
{
    [TestClass]
    public class ResetTransforms
    {
        [TestMethod]
        public void ResetAllTransforms()
        {
            Camera camera = new Camera() { Translation = new OpenTK.Vector3(-1, -1, -1) };
            camera.ResetTransforms();
            Assert.AreEqual(Vector3.Zero, camera.Translation);
            Assert.AreEqual(Vector3.Zero, camera.TransformedPosition);
            Assert.AreEqual(0, camera.RotationXDegrees);
            Assert.AreEqual(0, camera.RotationYDegrees);
        }
    }
}
