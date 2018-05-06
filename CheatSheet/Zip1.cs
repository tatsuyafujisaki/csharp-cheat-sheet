using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

namespace CheatSheet
{
    static class Zip1
    {
        static void Zip(string dirPath, string zipPath)
        {
            // Avoid IOException in ZipFile.CreateFromDirectory(...)
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            // A wrapping directory is excluded in zip.
            ZipFile.CreateFromDirectory(dirPath, zipPath);
        }

        static void Unzip(string zipPath, string directoryPath)
        {
            // Avoid IOException in ZipFile.ExtractToDirectory(...)
            // when directoryPath is not empty.
            if (Directory.Exists(directoryPath) && !Bool.Io.IsEmptyDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            // A wrapping directory is not created.
            ZipFile.ExtractToDirectory(zipPath, directoryPath);
        }

        [SuppressMessage("Microsoft.Usage", "CA2202")]
        static void Unzip(byte[] bs, string directoryPath)
        {
            // Avoid IOException in ZipFile.ExtractToDirectory(...)
            // when directoryPath is not empty.
            if (Directory.Exists(directoryPath) && !Bool.Io.IsEmptyDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            using (var ms = new MemoryStream(bs))
            {
                using (var za = new ZipArchive(ms))
                {
                    // A wrapping directory is not created.
                    za.ExtractToDirectory(directoryPath);
                }
            }
        }

        // Use forward slashes in entryPath.
        // entryPath for an empty directory needs to ends with a forward slash.
        // non-empty directories cannot be deleted because they are not individual entries.
        static void Delete(string zipPath, string entryPath)
        {
            using (var za = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                var zae = za.GetEntry(entryPath);

                zae?.Delete();
            }
        }
    }
}