using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using SFGraphics.Cameras;

namespace SFGraphics.Test.CameraTests
{
    [TestClass]
    public class RenderDimensions
    {
        [TestMethod]
        public void SquareAspect()
        {
            Camera camera = new Camera { RenderHeight = 1, RenderWidth = 1 };
            Assert.AreEqual(Matrix4.CreatePerspectiveFieldOfView(camera.FovRadians, 1, camera.NearClipPlane, camera.FarClipPlane), camera.PerspectiveMatrix);
        }

        [TestMethod]
        public void NonSquareAspect()
        {
            Camera camera = new Camera { RenderHeight = 1, RenderWidth = 2 };
            Assert.AreEqual(Matrix4.CreatePerspectiveFieldOfView(camera.FovRadians, 2, camera.NearClipPlane, camera.FarClipPlane), camera.PerspectiveMatrix);
        }

        [TestMethod]
        public void InvalidValues()
        {
            Camera camera = new Camera { RenderHeight = -1, RenderWidth = 0 };

            Assert.AreEqual(1, camera.RenderHeight);
            Assert.AreEqual(1, camera.RenderWidth);
            Assert.AreEqual(Matrix4.CreatePerspectiveFieldOfView(camera.FovRadians, 1, camera.NearClipPlane, camera.FarClipPlane), camera.PerspectiveMatrix);
        }
    }
}
