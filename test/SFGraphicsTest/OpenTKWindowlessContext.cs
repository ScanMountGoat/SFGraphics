using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SFGraphicsTest
{
    class OpenTKWindowlessContext
    {
        /// <summary>
        /// Create a dummy context, so OpenGL functions actually work.
        /// This may or may not work for actual rendering.
        /// </summary>
        public static void CreateDummyContext()
        {
            GraphicsMode mode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 0, ColorFormat.Empty, 1);
            GameWindow window = new GameWindow(640, 480, mode, "", OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 3, 3, GraphicsContextFlags.Default);
            window.Visible = false;
            window.MakeCurrent();
        }
    }
}
