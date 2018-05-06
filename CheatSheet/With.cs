using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CheatSheet
{
    static class With
    {
        static void MemoryStream(string s, Action<MemoryStream> f)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                f(ms);
            }
        }

        static void FileStream(string path, Action<FileStream> f)
        {
            using (var fs = File.OpenRead(path))
            {
                f(fs);
            }
        }

        static void StringReader(string s, Action<StringReader> f)
        {
            using (var tr = new StringReader(s))
            {
                f(tr);
            }
        }

        static void DataTable(Dictionary<string, Type> columns, IEnumerable<object[]> rows, Action<DataTable> f)
        {
            using (var dt = new DataTable())
            {
                foreach (var column in columns)
                {
                    dt.Columns.Add(column.Key, column.Value);
                }

                foreach (var row in rows)
                {
                    dt.Rows.Add(row);
                }

                f(dt);
            }
        }

        static void Zip(string path, Action<ZipArchiveEntry> f)
        {
            using (var za = ZipFile.OpenRead(path))
            {
                foreach (var e in za.Entries)
                {
                    f(e);
                }
            }
        }
    }
}