using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Threading;
using System.Diagnostics;

namespace SFGraphics.Controls
{
    /// <summary>
    /// Provides functionality similar to <see cref="OpenTK.GameWindow"/> for <see cref="OpenTK.GLControl"/>.
    /// Rendered frames can be triggered manually or automatically with a separate thread.
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
        /// The minimum time in milliseconds between frames for the dedicated rendering thread.
        /// </summary>
        public int RenderFrameInterval { get; set; } = 5;

        private readonly Thread renderThread = null;
        private bool shouldRenderFrames = false;

        /// <summary>
        /// Creates a new viewport with <see cref="defaultGraphicsMode"/>.
        /// </summary>
        public GLViewport() : base(defaultGraphicsMode)
        {
            renderThread = new Thread(new ThreadStart(FrameTimingLoop));

            // HACK: Make sure the frames are rendered on the UI thread.
            // This limits performance but prevents attempts to make the context current on more than one thread.
            Paint += GLViewport_Paint;
        }

        private void GLViewport_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            RenderFrame();
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

        /// <summary>
        /// Starts or resumes calling <see cref="RenderFrame"/> on the render thread with interval specified by <see cref="RenderFrameInterval"/>.
        /// </summary>
        public void ResumeRendering()
        {
            shouldRenderFrames = true;
            if (!renderThread.IsAlive)
                renderThread.Start();
        }

        /// <summary>
        /// Pauses frame updates for the render thread. Frames can still be rendered manually with <see cref="RenderFrame"/>.
        /// </summary>
        public void PauseRendering()
        {
            shouldRenderFrames = false;
        }

        private void FrameTimingLoop()
        {
            // This may cause the application to continue to run in the background.
            // TODO: Support killing the thread either manually or automatically.
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (shouldRenderFrames)
                {
                    if (stopwatch.ElapsedMilliseconds > RenderFrameInterval)
                    {
                        Invalidate();
                        stopwatch.Restart();
                    }
                }
                else
                {
                    // HACK: Avoid wasting CPU time if we don't need to render anything.
                    Thread.Sleep(1);
                }
            }
        }
    }
}
