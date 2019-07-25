using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Cameras;

namespace SFGraphics.Test.CameraTests
{
    [TestClass]
    public class ProjectionMatrix
    {
        [TestMethod]
        public void ChangeWidthHeightFov()
        {
            Camera camera = new Camera { RenderWidth = 1, RenderHeight = 2, FovRadians = 0.5f };
            var expected = Matrix4.CreatePerspectiveFieldOfView(0.5f, 0.5f, camera.NearClipPlane, camera.FarClipPlane);
            Assert.AreEqual(expected, camera.PerspectiveMatrix);
        }
    }
}
