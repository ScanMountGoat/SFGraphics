using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Timing;

namespace SFTiming.Test
{
    [TestClass]
    public class ThreadTimerEvents
    {
        [TestMethod]
        public void CreateDispose()
        {
            var timer = new ThreadTimer();
            timer.Dispose();
        }

        [TestMethod]
        public void CreateDisposeAfterStarting()
        {
            var timer = new ThreadTimer();
            timer.Start();
            timer.Dispose();
        }

        [TestMethod]
        public void StartStop()
        {
            using (var timer = new ThreadTimer())
            {
                timer.Start();
                timer.Stop();
            }
        }

        [TestMethod]
        public void StartTwice()
        {
            using (var timer = new ThreadTimer())
            {
                timer.Start();
                timer.Start();
            }
        }

        [TestMethod]
        public void StopTwice()
        {
            using (var timer = new ThreadTimer())
            {
                timer.Stop();
                timer.Stop();
            }
        }
    }
}
