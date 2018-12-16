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
        public void DefaultPosition()
        {
            Camera camera = new Camera();
            Assert.AreEqual(0, camera.ViewVector.X, delta);
            Assert.AreEqual(0, camera.ViewVector.Y, delta);
            Assert.AreEqual(1, camera.ViewVector.Z, delta);

        }
    }
}
