using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Threading;
using System.Diagnostics;
using System;

namespace SFGraphics.Controls
{
    /// <summary>
    /// Adds a dedicated rendering thread to <see cref="OpenTK.GLControl"/>.
    /// <para></para><para></para>
    /// Add rendering code to <see cref="FrameRendering"/>. Frames can be rendered on the rendering thread with 
    /// <see cref="RestartRendering"/> or on the calling thread with <see cref="RenderFrame"/>.
    /// </summary>
    public class GLViewport : OpenTK.GLControl, IDisposable
    {
        /// <summary>
        /// The default graphics mode for rendering. Enables depth/stencil buffers and anti-aliasing. 
        /// </summary>
        public static readonly GraphicsMode defaultGraphicsMode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8, 2);

        /// <summary>
        /// Occurs after frame and context setup and before the front and back buffer are swapped. 
        /// This effects <see cref="RenderFrame"/> and the dedicated render thread.
        /// </summary>
        public event EventHandler FrameRendering;

        /// <summary>
        /// The minimum time in milliseconds between frames.
        /// A value of <c>0</c> unlocks the frame rate but can result in very high CPU usage.
        /// </summary>
        public int RenderFrameInterval { get; set; } = 16;

        /// <summary>
        /// <c>true</c> when frame updates are being run from the dedicated rendering thread.
        /// </summary>
        public bool IsRendering { get; private set; }

        private readonly Thread renderThread;

        private bool renderThreadShouldClose;

        // Use a reset event to avoid busy waiting.
        private readonly ManualResetEvent shouldRender = new ManualResetEvent(true);
        private readonly ManualResetEvent isNotRenderingFrame = new ManualResetEvent(true);

        private bool disposed;

        /// <summary>
        /// Creates a new viewport with <paramref name="graphicsMode"/>.
        /// </summary>
        public GLViewport(GraphicsMode graphicsMode) : base(graphicsMode)
        {
            // Rendering should stop when the application exits.
            renderThread = new Thread(FrameTimingLoop) { IsBackground = true };
        }

        /// <summary>
        /// Creates a new viewport with <see cref="defaultGraphicsMode"/>.
        /// </summary>
        public GLViewport() : this(defaultGraphicsMode)
        {

        }

        /// <summary>
        /// Frees resources if the user forgets to call <see cref="Dispose()"/>.
        /// </summary>
        ~GLViewport()
        {
            Dispose(false);
        }

        /// <summary>
        /// Raises the <see cref="FrameRendering"/> event.
        /// </summary>
        protected virtual void OnNextFrameSetUp()
        {
            FrameRendering?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Renders and displays a frame on the current thread. Subscribe to <see cref="FrameRendering"/> to add custom rendering code.
        /// </summary>
        public void RenderFrame()
        {
            var wasRenderingOnThread = IsRendering;

            // Pause rendering to ensure the context is current on the appropriate thread.
            PauseRendering();

            SetUpAndRenderFrame(wasRenderingOnThread);

            // If rendering was already paused, keep the context current on this thread.
            if (wasRenderingOnThread)
                RestartRendering();
        }

        private void SetUpAndRenderFrame(bool wasRenderingOnThread)
        {
            isNotRenderingFrame.Reset();

            // Set the drawable area to the current dimensions.
            MakeCurrent();
            GL.Viewport(ClientRectangle);

            FrameRendering?.Invoke(this, null);

            // Display the content on screen.
            SwapBuffers();

            // Unbind the context so it can be used on the render thread.
            if (wasRenderingOnThread)
                Context.MakeCurrent(null);

            isNotRenderingFrame.Set();
        }

        /// <summary>
        /// Starts or resumes frame updates with interval specified by <see cref="RenderFrameInterval"/>.
        /// The context is made current on the rendering thread.
        /// </summary>
        public void RestartRendering()
        {
            IsRendering = true;

            // Make sure the context is only current on a single thread.
            if (Context.IsCurrent)
                Context.MakeCurrent(null);

            shouldRender.Set();

            if (!renderThread.IsAlive)
                renderThread.Start();
        }

        /// <summary>
        /// Pauses the rendering thread and blocks until the current frame has finished.
        /// The context is made current on the calling thread.
        /// </summary>
        public void PauseRendering()
        {
            IsRendering = false;

            shouldRender.Reset();

            // Block until rendering has actually stopped before the making context current on the current thread.
            isNotRenderingFrame.WaitOne();
            MakeCurrent();
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            shouldRender.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        /// <param name="disposing"><c>true</c> when called directly by user code</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // Make sure the rendering thread exits.
                renderThreadShouldClose = true;
                shouldRender.Set();
                if (renderThread.IsAlive)
                    renderThread.Join();

                base.Dispose(disposing);

                disposed = true;
            }
        }

        private void FrameTimingLoop()
        {
            var stopwatch = Stopwatch.StartNew();
            while (!renderThreadShouldClose)
            {
                shouldRender.WaitOne();

                // The reset event has to be set for the thread to exit gracefully.
                // Don't attempt to render a frame if the thread is flagged to close.
                if (stopwatch.ElapsedMilliseconds >= RenderFrameInterval && !renderThreadShouldClose)
                {
                    SetUpAndRenderFrame(true);
                    stopwatch.Restart();
                }
            }
        }
    }
}
