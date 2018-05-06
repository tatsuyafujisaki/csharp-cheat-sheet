using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SearchOption = System.IO.SearchOption;

namespace CheatSheet
{
    static class Io
    {
        internal static class DirectoryAndFile
        {
            static IEnumerable<string> WalkDirectoriesAndFiles(string path, bool recursive) => recursive ? Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories) : Directory.GetFileSystemEntries(path);
        }

        internal static class Directory1
        {
            static IEnumerable<string> WalkDirectories(string path, bool recursive) => recursive ? Directory.GetDirectories(path, "*", SearchOption.AllDirectories) : Directory.GetDirectories(path);

            // Explanatory wrapper
            static string GetCurrentDirectory() => Environment.CurrentDirectory;
            // Second fastest
            // Directory.GetCurrentDirectory();

            // Explanatory wrapper
            static void SetCurrentDirectory(string path) => Environment.CurrentDirectory = path;
            // Second fastest
            // Directory.SetCurrentDirectory(path);

            // Explanatory wrapper
            static string CreateTemporaryFileAndGetAbsolutePath() => Path.GetTempFileName();

            // Explanatory wrapper
            static string NotCreateTemporaryFileAndGetRelativePath() => Path.GetRandomFileName();

            static string Desktopize(params string[] paths) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.Combine(paths));

            static string CombineExecutableDirectory(params string[] paths) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(paths));

            // Explanatory wrapper
            // Recursively create all directory in the path.
            // No need to check if the directory already exists because the case makes no error.
            // IOException if the directory cannot be created.
            static void CreateDirectory(string path) => Directory.CreateDirectory(path);

            static void RecreateDirectory(string path)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                do
                {
                    try
                    {
                        // Directory.CreateDirectory(...) can throw UnauthorizedAccessException immedicately after Directory.Delete(...).
                        // The UnauthorizedAccessException period is longer if the directory is on a network drive.
                        Directory.CreateDirectory(path);
                    }
                    catch
                    {
                    }
                } while (!Directory.Exists(path));
            }

            static void CopyDirectory(string sourcePath, string destinationPath) => FileSystem.CopyDirectory(sourcePath, destinationPath, true);

            static void CopyFileToDirectory(string sourceFilePath, string destinationDirectoryPath) => File.Copy(sourceFilePath, Path.Combine(destinationDirectoryPath, Path.GetFileName(sourceFilePath)));

            // A trailing separator needs to be deleted, or Directory.GetParent("C:\Foo\") returns "C:\Foo".
            static string GetParentDirectory(string path) => Directory.GetParent(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
                .FullName;

            static string GetBottomDirectory(string path) => Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            internal static string[] SplitIntoDirectories(string path) => path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            internal static void TryDeleteDirectory(string path)
            {
                try
                {
                    // Avoid DirectoryNotFoundException.
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
                catch
                {
                    // ignored
                }
            }

            static void DeleteDirectory(string path)
            {
                // Avoid DirectoryNotFoundException.
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }

            static void DeleteEmptyDirectories(string path)
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    DeleteEmptyDirectories(dir);
                }

                if (Bool.Io.IsEmptyDirectory(path))
                {
                    Directory.Delete(path);
                }
            }

            static void DeleteOldFiles(string path, int days)
            {
                foreach (var fi in Directory.GetFiles(path).Select(file => new FileInfo(file))
                    .Where(fi => fi.CreationTime < DateTime.Today.AddDays(days)))
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch
                    {
                    }
                }
            }

            static string GetUnc(string path)
            {
                string GetUnc(char driveLetter)
                {
                    using (var rk = Registry.CurrentUser.OpenSubKey(@"Network\" + driveLetter))
                    {
                        return rk?.GetValue("RemotePath").ToString();
                    }
                }

                // Remove enclosing double quotation marks if they exist.
                path = path.Replace("\"", "");

                // Do nothing if a given path is relative or already UNC.
                if (!Path.IsPathRooted(path) || path.StartsWith(@"\\"))
                {
                    return path;
                }

                switch (path.Length)
                {
                    case 1:
                        return char.IsLetter(path[0]) ? GetUnc(path[0]) : null;
                    case 2:
                        return char.IsLetter(path[0]) && path[1] == ':' ? GetUnc(path[0]) : null;
                    default:
                        return char.IsLetter(path[0]) && path[1] == ':' && path[2] == '\\'
                            ? Path.Combine(GetUnc(path[0]), path.Substring(3))
                            : null;
                }
            }
        }

        internal static class File1
        {
            static IEnumerable<string> WalkFiles(string path, bool recursive) => recursive ? Directory.GetFiles(path, "*", SearchOption.AllDirectories) : Directory.GetFiles(path);

            // Explanatory wrapper
            static string GetFirstLine(string path) => File.ReadLines(path).First();

            // Explanatory wrapper
            static string GetLastLine(string path) => File.ReadLines(path).Last();

            static List<List<string>> GetLines(string csvPath) => File.ReadAllLines(csvPath).Select(line => line.Split(',').ToList()).ToList();
            static List<List<string>> GetLines(string csvPath, IEnumerable<int> columnIndexes) => File.ReadAllLines(csvPath).Select(line => line.Split(',').Where((_, i) => columnIndexes.Contains(i)).ToList()).ToList();
            static HashSet<List<string>> GetHashSet(string csvPath) => new HashSet<List<string>>(File.ReadAllLines(csvPath).Select(line => line.Split(',').ToList()));
            static HashSet<List<string>> GetHashSet(string csvPath, IEnumerable<int> columnIndexes) => new HashSet<List<string>>(File.ReadAllLines(csvPath).Select(line => line.Split(',').Where((_, i) => columnIndexes.Contains(i)).ToList()));

            // Explanatory wrapper
            static string GetFullPath(string relativePath) => Path.GetFullPath(relativePath);

            static string GetFullPathWithoutExtension(string path)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                var directoryName = Path.GetDirectoryName(path);
                return directoryName == null ? fileName : Path.Combine(directoryName, fileName);
            }

            static byte[] ReadLockedBinaryFile(string path)
            {
                using (var tf = new TemporaryFile())
                {
                    File.Copy(path, tf.Path1);
                    return File.ReadAllBytes(tf.Path1);
                }
            }

            static void CreateEmptyFile(string path) => File.CreateText(path).Close();

            // Explanatory wrapper
            static void ChangeExtension(string path, string extension) => Path.ChangeExtension(path, extension);

            static void DeleteFile(string path)
            {
                // Avoid ArgumentNullException by File.Delete("")
                // Avoid ArgumentException by File.Delete(null)
                // Avoid DirectoryNotFoundException by File.Delete(@"C:\NonExistingDirectory\Foo.txt")
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            internal static void TryDeleteFile(string path)
            {
                try
                {
                    // Avoid ArgumentNullException by File.Delete("")
                    // Avoid ArgumentException by File.Delete(null)
                    // Avoid DirectoryNotFoundException by File.Delete(@"C:\NonExistingDirectory\Foo.txt")
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                    // ignored
                }
            }

            static void CopyLockedBinaryFile(string lockedFilepath, string newFilePath)
            {
                using (var fs = new FileStream(lockedFilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    File.WriteAllBytes(newFilePath, Converter.ToBytes(fs));
                }
            }

            internal static string AppendSuffix(string path, string suffix)
            {
                var fileName = string.Concat(Path.GetFileNameWithoutExtension(path), suffix, Path.GetExtension(path));
                var directoryName = Path.GetDirectoryName(path);
                return directoryName == null ? fileName : Path.Combine(directoryName, fileName);
            }

            // Useful when you repeat a test that makes a destructive change to a file.
            static string ReadyAndGetTestFilePath()
            {
                const string original = "Original.txt";
                const string exposed = "Exposed.txt";
                File.Copy(original, exposed, true);
                return exposed;
            }
        }
    }
}