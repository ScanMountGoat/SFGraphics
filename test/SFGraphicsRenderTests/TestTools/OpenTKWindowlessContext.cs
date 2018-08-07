using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace SFGraphicsRenderTests.TestTools
{
    class OpenTKWindowlessContext
    {
        /// <summary>
        /// Create a dummy context, so OpenGL functions actually work.
        /// This may or may not work for actual rendering.
        /// </summary>
        /// <param name="major">OpenGL major version</param>
        /// <param name="minor">OpenGL minor version</param>
        public static GameWindow CreateDummyContext(int major = 3, int minor = 3)
        {
            GraphicsMode mode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 0, ColorFormat.Empty, 1);
            GameWindow window = new GameWindow(640, 480, mode, "", GameWindowFlags.Default, 
                DisplayDevice.Default, major, minor, GraphicsContextFlags.Default);
            window.Visible = false;
            window.MakeCurrent();
            return window;
        }
    }
}
