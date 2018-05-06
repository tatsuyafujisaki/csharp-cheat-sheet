using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CheatSheet.Poller
{
    static class FilePoller
    {
        static HashSet<Timer> perpetualTimers = new HashSet<Timer>();
        static Dictionary<DateTime, Timer> timers = new Dictionary<DateTime, Timer>();

        // Usage: PollForever(@"C:\Foo.txt", TimeSpan.Zero, TimeSpan.FromMinutes(1))
        internal static void PollForever(string path, TimeSpan period, TimeSpan dueTime)
        {
            perpetualTimers.Add(new Timer(state =>
             {
                 if (File.Exists(path))
                 {
                     File.Delete(path);
                 }
             }, null, dueTime, period));
        }

        // Usage:
        // var are = new AutoResetEvent(false);
        // var timer = FilePoller.PollUntilFound(are, @"C:\Foo.txt", TimeSpan.Zero, TimeSpan.FromMinutes(1));
        // are.WaitOne();
        internal static void PollUntilFound(AutoResetEvent are, string path, TimeSpan dueTime, TimeSpan period)
        {
            var dt = DateTime.Now;

            timers.Add(dt, new Timer(state =>
             {
                 if (File.Exists(path))
                 {
                     timers[dt].Dispose();
                     timers.Remove(dt);
                     File.Delete(path);
                     are.Set();
                 }
             }, null, dueTime, period));
        }

        // Usage: PollUntilFoundThenExit(@"C:\Foo.txt", TimeSpan.Zero, TimeSpan.FromMinutes(1))
        internal static void PollUntilFoundThenExit(string path, TimeSpan dueTime, TimeSpan period)
        {
            var dt = DateTime.Now;

            timers.Add(dt, new Timer(state =>
            {
                if (File.Exists(path))
                {
                    timers[dt].Dispose();
                    timers.Remove(dt);
                    File.Delete(path);
                    Environment.Exit(0);
                }
            }, null, dueTime, period));
        }
    }
}