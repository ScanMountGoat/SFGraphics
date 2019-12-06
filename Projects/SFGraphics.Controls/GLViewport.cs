using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Threading;
using System.Diagnostics;

namespace SFGraphics.Controls
{
    /// <summary>
    /// Provides functionality similar to <see cref="OpenTK.GameWindow"/> for <see cref="OpenTK.GLControl"/>.
    /// <para></para><para></para>
    /// Frame timing can be handled manually or with a dedicated thread using <see cref="ResumeRendering"/>.
    /// </summary>
    public class GLViewport : OpenTK.GLControl, System.IDisposable
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
        /// The minimum time in milliseconds between frames.
        /// A value of <c>0</c> unlocks the frame rate but can result in very high CPU usage.
        /// </summary>
        public int RenderFrameInterval { get; set; } = 16;

        private readonly Thread renderThread;

        private bool renderThreadShouldClose;

        // Use a reset event to avoid busy waiting.
        private readonly ManualResetEvent shouldRenderEvent = new ManualResetEvent(true);

        private bool disposed;

        /// <summary>
        /// Creates a new viewport with <see cref="defaultGraphicsMode"/>.
        /// </summary>
        public GLViewport() : base(defaultGraphicsMode)
        {
            // Rendering should stop when the application exits.
            renderThread = new Thread(FrameTimingLoop) { IsBackground = true };
        }

        /// <summary>
        /// Frees resources if the user forgets to call <see cref="Dispose()"/>.
        /// </summary>
        ~GLViewport()
        {
            Dispose(false);
        }

        /// <summary>
        /// Sets up, renders, and displays a frame. Subscribe to <see cref="OnRenderFrame"/> to add custom rendering code.
        /// </summary>
        public void RenderFrame()
        {
            // TODO: Render on the current thread?
            // This will cause issues with VAOs.
            // Should this method be public?

            // Set the drawable area to the current dimensions.
            MakeCurrent();
            GL.Viewport(ClientRectangle);

            OnRenderFrame?.Invoke(this, null);

            // Display the content on screen.
            SwapBuffers();
        }

        /// <summary>
        /// Starts or resumes frame updates with interval specified by <see cref="RenderFrameInterval"/>.
        /// </summary>
        public void ResumeRendering()
        {
            shouldRenderEvent.Set();

            if (!renderThread.IsAlive)
                renderThread.Start();
        }

        /// <summary>
        /// Pauses automatic frame updates. 
        /// Frames can still be rendered manually with <see cref="RenderFrame"/>.
        /// </summary>
        public void PauseRendering()
        {
            shouldRenderEvent.Reset();
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            shouldRenderEvent.Dispose();
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        /// <param name="disposing"><c>true</c> when called directly by user code</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                base.Dispose(disposing);
                renderThreadShouldClose = true;

                disposed = true;
            }
        }

        private void FrameTimingLoop()
        {
            // TODO: What happens when rendering is paused and the form is closed?
            var stopwatch = Stopwatch.StartNew();
            while (!renderThreadShouldClose)
            {
                shouldRenderEvent.WaitOne();

                if (stopwatch.ElapsedMilliseconds >= RenderFrameInterval)
                {
                    RenderFrame();
                    stopwatch.Restart();
                }
            }
        }
    }
}
