using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace CheatSheet.Bool
{
    static class Io
    {
        static bool IsBinaryFile(string path) => File.ReadAllText(path).Any(c => char.IsControl(c) && c != '\r' && c != '\n');

        // Use forward slashes in entryPath.
        // entryPath for an empty directory needs to ends with a forward slash.
        // non-empty directories cannot be deleted because they don't appear as individual entries.
        static bool Contains(string zipPath, string entryPath)
        {
            using (var za = ZipFile.OpenRead(zipPath))
            {
                return za.GetEntry(entryPath) != null;
            }
        }

        internal static bool IsEmptyDirectory(string path) => !Directory.EnumerateFileSystemEntries(path).Any();

        static bool HasExtension(string path, string extension) => String1.EqualsIgnoreCase(Path.GetExtension(path), extension);

        // Explanatory wrapper
        static bool IsRelativePath(string path) => !Path.IsPathRooted(path);

        static bool IsUnc(string path) => path.StartsWith(@"\\");
        // Second fastest
        // return new Uri(path).IsUnc;

        // new DriveInfo(path) throws System.ArgumentException when the path is UNC.
        static bool IsRunningOnFixedDrive() => !IsUnc(AppDomain.CurrentDomain.BaseDirectory) && new DriveInfo(AppDomain.CurrentDomain.BaseDirectory).DriveType == DriveType.Fixed;

        static bool DriveExists(char driveLetter) => DriveInfo.GetDrives().Any(drive => drive.Name == driveLetter + @":\\");

        internal static bool IsWritableDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);

                // Trying writing is the only way to confirm whether the file is writable.
                // "var foo =" is omitted for simplicity.
                using (File.Create(Path.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                {
                }

                return true;
            }
            catch
            {
                // Catch not only UnauthorizedAccessException but also other exceptions because I care whether the file is writeble for whatever reasons.
                return false;
            }
        }

        static bool IsWritableFile(string path)
        {
            try
            {
                // Trying writing is the only way to confirm whether the file is writable.
                // "var foo =" is omitted.
                using (File.Create(path, 1, FileOptions.DeleteOnClose))
                {
                }

                return true;
            }
            catch
            {
                // Catch not only UnauthorizedAccessException but also other exceptions because I care whether the file is writeble for whatever reasons.
                return false;
            }
        }
    }
}