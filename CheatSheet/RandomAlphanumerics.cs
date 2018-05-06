using System;

namespace CheatSheet
{
    // If what you need is not truely radom alphanumerics,
    // cosider using DateTime.Now.ToString("yyyyMMdd-hhmmss.fffffff") for simplicity.
    static class RandomAlphanumerics
    {
        const string Alphanumerics = "abcdefghijklmnopqrstuvwxyz0123456789";
        static readonly Random Random1 = new Random();

        internal static string Generate(int length)
        {
            var cs = new char[length];

            for (var i = 0; i < length; i++)
            {
                cs[i] = Alphanumerics[Random1.Next(Alphanumerics.Length)];
            }

            return new string(cs);
        }
    }
}