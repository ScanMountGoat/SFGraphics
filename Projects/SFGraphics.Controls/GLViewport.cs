using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SFGraphics.Timing;
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
        /// The time in milliseconds between the start of each frame update.
        /// Defaults to 60 fps.
        /// A value of <c>0</c> unlocks the frame rate but results in very high CPU usage.
        /// </summary>
        public double RenderFrameInterval 
        { 
            get => frameTimer.UpdateInterval; 
            set { frameTimer.UpdateInterval = value; }
        }

        /// <summary>
        /// <c>true</c> when frame updates are being run from the dedicated rendering thread.
        /// </summary>
        public bool IsRendering => frameTimer.IsUpdating;

        private readonly ThreadTimer frameTimer;

        private bool disposed;

        /// <summary>
        /// Creates a new viewport with <paramref name="graphicsMode"/>.
        /// </summary>
        public GLViewport(GraphicsMode graphicsMode) : base(graphicsMode)
        {
            frameTimer = new ThreadTimer() { UpdateInterval = 16.6 };
            SetUpFrameRenderingEvents();
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
        /// Starts or resumes frame updates with interval specified by <see cref="RenderFrameInterval"/>.
        /// The context is made current on the rendering thread.
        /// </summary>
        public void RestartRendering()
        {
            frameTimer.Restart();
        }

        /// <summary>
        /// Pauses the rendering thread and blocks until the current frame has finished.
        /// The context is made current on the calling thread.
        /// </summary>
        public void PauseRendering()
        {
            frameTimer.Stop();
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

        private void SetUpFrameRenderingEvents()
        {
            // Make sure the context is only current on a single thread.
            frameTimer.Starting += (s, e) =>
            {
                if (Context.IsCurrent)
                    Context.MakeCurrent(null);
            };
            frameTimer.Stopped += (s, e) => MakeCurrent();
            frameTimer.Updating += (s, e) => SetUpAndRenderFrame(true);
        }

        private void SetUpAndRenderFrame(bool wasRenderingOnThread)
        {
            // Set the drawable area to the current dimensions.
            MakeCurrent();
            GL.Viewport(ClientRectangle);

            FrameRendering?.Invoke(this, null);

            // Display the content on screen.
            SwapBuffers();

            // Unbind the context so it can be used on the render thread.
            if (wasRenderingOnThread)
                Context.MakeCurrent(null);
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
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
                frameTimer.Dispose();

                base.Dispose(disposing);

                disposed = true;
            }
        }
    }
}
