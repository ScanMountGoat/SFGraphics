using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Controls
{
    /// <summary>
    /// Provides an easier to use interface for <see cref="OpenTK.GLControl"/>.
    /// </summary>
    public class GLViewport : OpenTK.GLControl
    {
        /// <summary>
        /// The default graphics mode for rendering. Enables depth/stencil buffers and antialiasing. 
        /// </summary>
        public static readonly GraphicsMode defaultGraphicsMode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8, 16);

        public delegate void OnRenderFrameEventHandler(object sender, System.EventArgs e);

        /// <summary>
        /// Occurs after frame setup and before the front and back buffer are swapped. To render a frame, use <see cref="RenderFrame"/>.
        /// </summary>
        public event OnRenderFrameEventHandler OnRenderFrame;

        /// <summary>
        /// Creates a new viewport with <see cref="defaultGraphicsMode"/>.
        /// </summary>
        public GLViewport() : base(defaultGraphicsMode)
        {

        }

        /// <summary>
        /// Sets up, renders, and displays a frame. Subscribe to <see cref="OnRenderFrame"/> to add custom rendering code.
        /// </summary>
        public void RenderFrame()
        {
            // Set the drawable area to the current dimensions.
            MakeCurrent();
            GL.Viewport(ClientRectangle);

            OnRenderFrame?.Invoke(this, null);

            // Display the content on screen.
            SwapBuffers();
        }
    }
}
