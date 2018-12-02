using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Cameras;

namespace CameraTests
{
    [TestClass]
    public class RenderDimensions
    {
        [TestMethod]
        public void SquareAspect()
        {
            Camera camera = new Camera();
            camera.RenderHeight = 1;
            camera.RenderWidth = 1;
            Assert.AreEqual(OpenTK.Matrix4.CreatePerspectiveFieldOfView(camera.FovRadians, 1, camera.NearClipPlane, camera.FarClipPlane), camera.PerspectiveMatrix);
        }

        [TestMethod]
        public void NonSquareAspect()
        {
            Camera camera = new Camera();
            camera.RenderHeight = 1;
            camera.RenderWidth = 2;
            Assert.AreEqual(OpenTK.Matrix4.CreatePerspectiveFieldOfView(camera.FovRadians, 2, camera.NearClipPlane, camera.FarClipPlane), camera.PerspectiveMatrix);
        }

        [TestMethod]
        public void InvalidValues()
        {
            Camera camera = new Camera();
            camera.RenderHeight = -1;
            camera.RenderWidth = 0;

            Assert.AreEqual(1, camera.RenderHeight);
            Assert.AreEqual(1, camera.RenderWidth);
            Assert.AreEqual(OpenTK.Matrix4.CreatePerspectiveFieldOfView(camera.FovRadians, 1, camera.NearClipPlane, camera.FarClipPlane), camera.PerspectiveMatrix);
        }
    }
}
