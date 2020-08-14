using System;
using System.Diagnostics;
using System.Threading;

namespace SFGraphics.Timing
{
    /// <summary>
    /// Executes methods on a dedicated thread with an interval specified by <see cref="UpdateInterval"/>.
    /// Make sure to call <see cref="Dispose"/> to ensure the update thread exits gracefully.
    /// </summary>
    public sealed class ThreadTimer : IDisposable
    {
        /// <summary>
        /// Occurs on the calling thread for <see cref="Start"/> or <see cref="Restart"/>.
        /// </summary>
        public event EventHandler Starting;

        /// <summary>
        /// Occurs on the dedicated update thread every <see cref="UpdateInterval"/> milliseconds 
        /// when <see cref="IsUpdating"/> is <c>true</c>.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Occurs on the calling thread for <see cref="Stop"/> or <see cref="Restart"/> once the most recent update has finished.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// The time in milliseconds between the start of each update event.
        /// The actual precision of the timing is hardware dependent.
        /// </summary>
        public double UpdateInterval { get; set; }

        /// <summary>
        /// <c>true</c> when the dedicated update thread is triggering <see cref="Updating"/> events.
        /// </summary>
        public bool IsUpdating { get; private set; }

        private readonly Thread updateThread;

        // Use reset events to avoid busy waiting while stopped.
        private readonly ManualResetEvent shouldUpdate = new ManualResetEvent(true);
        private readonly ManualResetEvent isNotUpdating = new ManualResetEvent(true);

        private bool disposed;
        private bool updateThreadShouldClose;

        /// <summary>
        /// Creates a new <see cref="ThreadTimer"/>. Updating can be started with <see cref="Start"/> or <see cref="Restart"/>.
        /// </summary>
        public ThreadTimer()
        {
            updateThread = new Thread(UpdateLoop) { IsBackground = true };
        }

        /// <summary>
        /// Starts updates on the dedicated thread.
        /// </summary>
        public void Start()
        {
            IsUpdating = true;

            Starting?.Invoke(this, EventArgs.Empty);

            shouldUpdate.Set();

            if (!updateThread.IsAlive)
                updateThread.Start();
        }

        /// <summary>
        /// Stops updates on the dedicated thread.
        /// </summary>
        public void Stop()
        {
            IsUpdating = false;

            shouldUpdate.Reset();

            // Block until updating has actually stopped.
            isNotUpdating.WaitOne();
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Stops updates on the dedicated thread and then starts updates again.
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }

        private void UpdateLoop()
        {
            var stopwatch = Stopwatch.StartNew();
            while (!updateThreadShouldClose)
            {
                shouldUpdate.WaitOne();

                // The reset event has to be set for the thread to exit gracefully.
                // Don't attempt to render a frame if the thread is flagged to close.
                // Precision is implementation dependent but should be more precise than ElapsedMilliseconds.
                if (((double)stopwatch.ElapsedTicks * 1000 / Stopwatch.Frequency) >= UpdateInterval && !updateThreadShouldClose)
                {
                    stopwatch.Restart();
                    Update();
                }
            }
        }

        private void Update()
        {
            isNotUpdating.Reset();

            Updating?.Invoke(this, EventArgs.Empty);

            isNotUpdating.Set();
        }

        /// <summary>
        /// Frees unmanaged resources and terminates the render thread.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                CloseUpdateThread();
                shouldUpdate.Dispose();
                isNotUpdating.Dispose();
                GC.SuppressFinalize(this);

                disposed = true;
            }
        }

        private void CloseUpdateThread()
        {
            // Make sure the update thread exits.
            updateThreadShouldClose = true;
            shouldUpdate.Set();
            if (updateThread.IsAlive)
                updateThread.Join();
        }
    }
}
