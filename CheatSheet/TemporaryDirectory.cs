using System;
using System.IO;

namespace CheatSheet
{
    sealed class TemporaryDirectory : IDisposable
    {
        internal readonly string Path1;

        internal TemporaryDirectory()
        {
            Path1 = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fffffff"));

            Directory.CreateDirectory(Path1);
        }

        public void Dispose()
        {
            if (Directory.Exists(Path1))
            {
                Directory.Delete(Path1, true);
            }
        }
    }
}