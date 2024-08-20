using System.Timers;

namespace XOuranos.Utilities
{
    /// <summary>
    /// Extension methods for the <see cref="Timer"/> class.
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// Reset a timer from the start.
        /// </summary>
        /// <param name="timer">The timer to reset.</param>
        public static void Reset(this System.Timers.Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}
