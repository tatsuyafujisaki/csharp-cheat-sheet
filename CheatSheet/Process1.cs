using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CheatSheet
{
    static class Process1
    {
        internal static void KillOldSelf()
        {
            var me = Process.GetCurrentProcess();

            foreach (var p in Process.GetProcessesByName(me.ProcessName).Where(p => p.Id != me.Id))
            {
                p.Kill();
            }
        }

        static void Kill(string name)
        {
            // Remove an extension or the process will not be found.
            foreach (var p in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(name)))
            {
                p.Kill();
            }
        }

        internal static void StartIfNotExists(string name)
        {
            // Remove an extension or the process will not be found.
            if (!Process.GetProcessesByName(Path.GetFileNameWithoutExtension(name)).Any())
            {
                Process.Start(name);
            }
        }

        // Explanatory wrapper
        internal static void StartAndForget(string fileName, string arguments) => Process.Start(fileName, arguments);

        internal static int StartAndWait(string fileName, string arguments)
        {
            using (var p = Process.Start(fileName, arguments))
            {
                const int millisecondsToTimout = 60_000;

                if (!p.WaitForExit(millisecondsToTimout))
                {
                    throw new TimeoutException($"{fileName}({arguments}) did not exit in {millisecondsToTimout} milliseconds and timed out.");
                }

                return p.ExitCode;
            }
        }

        internal static ValueTuple<int, string, string> StartAndGetResult(string fileName, string arguments)
        {
            using (var p = Process.Start(new ProcessStartInfo(fileName, arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }))
            {
                const int millisecondsToTimout = 60_000;

                if (!p.WaitForExit(millisecondsToTimout))
                {
                    throw new TimeoutException($"{fileName}({arguments}) did not exit in {millisecondsToTimout} milliseconds and timed out.");
                }

                return ValueTuple.Create(p.ExitCode, p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());
            }
        }

        static ValueTuple<string, string, string> CompareFiles(string inPath1, string inPath2)
        {
            var (exitCode, output, error) = StartAndGetResult("fc.exe", $"/n {inPath1} {inPath2}");

            switch (exitCode)
            {
                case 0:
                    return ValueTuple.Create("No differences", output, error);
                case 1:
                    return ValueTuple.Create("Some differences", output, error);
                default:
                    throw new ApplicationException(error);
            }
        }
    }
}