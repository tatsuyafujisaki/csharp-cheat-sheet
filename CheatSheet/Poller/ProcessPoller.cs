using System;
using System.Collections.Generic;
using System.Threading;

namespace CheatSheet.Poller
{
    static class ProcessPoller
    {
        static HashSet<Timer> perpetualTimers = new HashSet<Timer>();

        // Usage: PollForever("Process1.exe", TimeSpan.Zero, TimeSpan.FromMinutes(1))
        internal static void PollForever(string processName, TimeSpan dueTime, TimeSpan period) => perpetualTimers.Add(new Timer(state => Process1.StartIfNotExists(processName), null, dueTime, period));
    }
}