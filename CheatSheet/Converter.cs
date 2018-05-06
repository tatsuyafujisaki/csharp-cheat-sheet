using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CheatSheet
{
    static class Converter
    {
        internal static string ToString(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        internal static byte[] ToBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        // Explanatory wrapper
        static string ToString(byte[] bs) => Convert.ToBase64String(bs);

        // Explanatory wrapper
        static byte[] ToBytes(string s) => Convert.FromBase64String(s);

        // Explanatory wrapper
        static char ToCharFromAscii(int ascii) => (char)ascii;

        static char ToCharFromInt(int n)
        {
            if (n < 0 || 9 < n)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "Must be between 0 and 9.");
            }

            return n.ToString()[0];

            // Second fastest
            // return (char)(x + '0');
        }

        static int ToInt(char c) => c - '0';
        // Second fastest
        // => (int) char.GetNumericValue(c);

        // Usage: double x = ToValue<double>("3.14");
        static T ToValue<T>(string s) => (T)Convert.ChangeType(s, typeof(T));

        static int[] ToInt(string[] ss) => Array.ConvertAll(ss, int.Parse);

        static Dictionary<string, string> ToDictionary(string csvPath) => File.ReadLines(csvPath).Where(c => c.Contains(',')).ToDictionary(s => s.Substring(0, s.IndexOf(',')), s => s.Substring(s.IndexOf(',') + 1));

        static string ToString(DataSet ds)
        {
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }

        static List<DataRow> ToDataRows(DataTable dt) => dt.AsEnumerable().ToList();

        static string ToCsv<T>(params T[] xs) => string.Join(",", xs);

        static IEnumerable<T> ToIEnumerable<T>(T[,] xs) => xs.Cast<T>();

        static IEnumerable<T> ToIEnumerable<T>(IEnumerable xs) => xs.Cast<T>();

        // Make rows distinct based on an element. This can be applied to a property of a class too.
        static IEnumerable<(T1, T2)> Distinct<T1, T2>(IEnumerable<(T1, T2)> vts) => vts.GroupBy(x => x.Item1).Select(xs => xs.First());
    }
}