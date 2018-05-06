using System;
using System.IO;

namespace CheatSheet
{
    sealed class TemporaryFile : IDisposable
    {
        readonly string subdirectoryPath;
        internal readonly string Path1;

        internal TemporaryFile()
        {
            Path1 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            // The above does not create a file.
            // To create a file, use the following.
            // Path = System.IO.Path.GetTempFileName();
        }

        internal TemporaryFile(string fileName)
        {
            // Create a subdirectory in case the fileName exists and is locked.
            subdirectoryPath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fffffff"));

            Directory.CreateDirectory(subdirectoryPath);

            Path1 = Path.Combine(subdirectoryPath, fileName);
        }

        public void Dispose()
        {
            if (Directory.Exists(subdirectoryPath))
            {
                Io.Directory1.TryDeleteDirectory(subdirectoryPath);
            }
            else
            {
                Io.File1.TryDeleteFile(Path1);
            }
        }
    }
}