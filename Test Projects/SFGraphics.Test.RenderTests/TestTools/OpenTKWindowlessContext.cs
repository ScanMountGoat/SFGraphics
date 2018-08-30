using OpenTK;
using OpenTK.Graphics;

namespace SFGraphics.Test.RenderTests.TestTools
{
    class OpenTKWindowlessContext
    {
        /// <summary>
        /// Creates and binds a dummy context, so OpenGL functions will work.
        /// If a context is already bound, no context is created.
        /// </summary>
        /// <param name="major">OpenGL major version</param>
        /// <param name="minor">OpenGL minor version</param>
        public static void BindDummyContext(int major = 3, int minor = 3)
        {
            if (GraphicsContext.CurrentContext == null)
            {
                GraphicsMode mode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 0, ColorFormat.Empty, 1);
                GameWindow window = new GameWindow(640, 480, mode, "", GameWindowFlags.Default,
                    DisplayDevice.Default, major, minor, GraphicsContextFlags.Default);
                window.Visible = false;
                window.MakeCurrent();
            }
        }
    }
}
