using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;

namespace SFGraphics.Test.CameraTests
{
    [TestClass]
    public class ResetPosition
    {
        [TestMethod]
        public void ResetPositionToDefault()
        {
            Camera camera = new Camera() { Position = new OpenTK.Vector3(-1, -1, -1) };
            camera.ResetToDefaultPosition();
            Assert.AreEqual(camera.DefaultPosition, camera.Position);
        }
    }
}
