using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Controls
{
    /// <summary>
    /// Provides functionality similar to <see cref="OpenTK.GameWindow"/> for <see cref="OpenTK.GLControl"/>.
    /// </summary>
    public class GLViewport : OpenTK.GLControl
    {
        /// <summary>
        /// The default graphics mode for rendering. Enables depth/stencil buffers and anti-aliasing. 
        /// </summary>
        public static readonly GraphicsMode defaultGraphicsMode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8, 16);

        /// <summary>
        /// Describes the arguments used for a rendered frame. 
        /// </summary>
        /// <param name="sender">The control rendering the frame</param>
        /// <param name="e">information about the rendered frame</param>
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
