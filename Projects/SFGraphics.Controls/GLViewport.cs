﻿using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Threading;
using System.Diagnostics;

namespace SFGraphics.Controls
{
    /// <summary>
    /// Provides functionality similar to <see cref="OpenTK.GameWindow"/> for <see cref="OpenTK.GLControl"/>.
    /// <para></para><para></para>
    /// Frame timing can be handled manually or with a dedicated thread using <see cref="RestartRendering"/>.
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
        /// Occurs after frame setup and before the front and back buffer are swapped. To render a frame, use <see cref="SetUpAndRenderFrame"/>.
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
        private readonly ManualResetEvent shouldRender = new ManualResetEvent(true);
        private readonly ManualResetEvent isNotRendering = new ManualResetEvent(true);

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
        /// Renders and displays a frame on the current thread. Subscribe to <see cref="OnRenderFrame"/> to add custom rendering code.
        /// </summary>
        public void RenderFrame()
        {
            // Pause rendering to ensure the context is current on the appropriate thread.
            PauseRendering();

            SetUpAndRenderFrame();

            // TODO: Only restart rendering if it was rendering previously.
            RestartRendering();
        }

        private void SetUpAndRenderFrame()
        {
            isNotRendering.Reset();

            // Set the drawable area to the current dimensions.
            MakeCurrent();
            GL.Viewport(ClientRectangle);

            OnRenderFrame?.Invoke(this, null);

            // Display the content on screen.
            SwapBuffers();

            // Unbind the context so it can be used on another thread.
            Context.MakeCurrent(null);
            isNotRendering.Set();
        }

        /// <summary>
        /// Starts or resumes frame updates with interval specified by <see cref="RenderFrameInterval"/>.
        /// The context is made current on the rendering thread.
        /// </summary>
        public void RestartRendering()
        {
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
            shouldRender.Reset();

            // Block until rendering has actually stopped before the making context current on the current thread.
            isNotRendering.WaitOne();
            MakeCurrent();
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            shouldRender.Dispose();
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
                // Make sure the rendering thread exits.
                renderThreadShouldClose = true;
                shouldRender.Set();
                renderThread.Join();

                base.Dispose(disposing);

                disposed = true;
            }
        }

        private void FrameTimingLoop()
        {
            // TODO: What happens when rendering is paused and the form is closed?
            var stopwatch = Stopwatch.StartNew();
            while (!renderThreadShouldClose)
            {
                shouldRender.WaitOne();

                if (stopwatch.ElapsedMilliseconds >= RenderFrameInterval)
                {
                    SetUpAndRenderFrame();
                    stopwatch.Restart();
                }
            }
        }
    }
}
