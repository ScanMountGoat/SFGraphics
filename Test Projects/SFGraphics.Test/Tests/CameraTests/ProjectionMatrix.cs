using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;
using OpenTK;

namespace CameraTests
{
    [TestClass]
    public class ProjectionMatrix
    {
        [TestMethod]
        public void ChangeWidthHeightFov()
        {
            Camera camera = new Camera();
            camera.RenderWidth = 1;
            camera.RenderHeight = 2;
            camera.FovRadians = 0.5f;
            var expected = Matrix4.CreatePerspectiveFieldOfView(0.5f, 0.5f, camera.NearClipPlane, camera.FarClipPlane);
            Assert.AreEqual(expected, camera.PerspectiveMatrix);
        }
    }
}
