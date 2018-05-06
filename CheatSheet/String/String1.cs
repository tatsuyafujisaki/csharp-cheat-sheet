using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CheatSheet.String
{
    static class String1
    {
        static string Left(string s, int n) => s.Length < n ? s : s.Substring(0, n);

        static string Right(string s, int n) => s.Length < n ? s : s.Substring(s.Length - n);

        internal static string SingleQuote(string s) => $"'{s}'";

        static string DoubleQuote(string s) => $"\"{s}\"";

        static int CountChar(string s, char c) => s.Count(x => x == c);

        static int CountSubstring(string s, string substring) => Regex.Matches(s, substring, RegexOptions.IgnoreCase).Count;

        static int CountOverlappedSubstring(string s, string substring)
        {
            var count = 0;
            var i = 0;

            while (true)
            {
                i = s.IndexOf(substring, i, StringComparison.OrdinalIgnoreCase);

                if (i == -1) { break; }

                i++;
                count++;
            }

            return count;
        }

        static string Repeat(string s, int n) => string.Concat(Enumerable.Repeat(s, n));

        static string UnifySpaces(string s) => Regex.Replace(s, " {2,}", " ");

        static string[] Split(string s, string separator) => s.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

        // Split a string into lines excluding blank lines.
        // Using both "\r" and "\n" is better than using Environment.NewLine as it is equivalent to "\r\n".
        static string[] GetLines(string s) => s.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Split a string by whitespaces (both space and newline).
        // RemoveEmptyEntries is used to treat continuous whitespaces as one separator.
        static string[] GetTokens(string s) => s.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

        // 2 is the total character count.
        static string PadWithSpace(int n) => $"{n,2}";

        static string PadWithZero(int n, int totalCharacterCount) => n.ToString("D" + totalCharacterCount);

        static string GetStringBeforeChar(string s, char c) => s.Substring(0, s.IndexOf(c));

        static int GetIndexOfNth(string s, string substring, int oneBasedNth)
        {
            var i = -1;

            for (var j = 0; j < oneBasedNth; j++)
            {
                i = s.IndexOf(substring, i + 1, StringComparison.OrdinalIgnoreCase);
                if (i == -1) { return -1; }
            }

            return i;
        }

        static string GetBetween(string s, string from, string to, bool inclusive)
        {
            var m = Regex.Match(s, inclusive ? $"({from}.*?{to})" : $"{from}(.*?){to}", RegexOptions.IgnoreCase);
            return m.Success ? m.Groups[1].Value : null;
        }

        static string DeleteBetween(string s, string from, string to, bool inclusive) => Regex.Replace(s, $"{from}.*?{to}", inclusive ? "" : from + to, RegexOptions.IgnoreCase);

        static string ReplaceBetween(string s, string from, string to, string newInbetween, bool inclusive) => Regex.Replace(s, $"{from}.*?{to}", inclusive ? newInbetween : from + newInbetween + to, RegexOptions.IgnoreCase);

        static IEnumerable<string> DeleteBlankLines(IEnumerable<string> ss) => ss.Where(s => !string.IsNullOrWhiteSpace(s));

        static IEnumerable<string> GetSpecificChars(string s, string chars) => Regex.Split(s, "[^" + chars + "]").Where(x => x != string.Empty);

        static string GetDigits(string s) => Regex.Match(s, @"\d+").Value;

        // Explanatory wrapper
        // Make the first letter uppercase and make the other letters lowercase.
        static string SentenceCase(string s) => Strings.StrConv(s, VbStrConv.ProperCase);

        static string ReplaceAt(string s, int i, char c) => new StringBuilder(s) { [i] = c }.ToString();

        static string ExtractDigits(string s) => string.Concat(s.ToCharArray().Where(char.IsDigit));
        // Second fastest
        // static string ExtractDigits(string s) => new string(s.Where(char.IsDigit).ToArray());

        static string RemoveOneSubstring(string s, string substring) => s.Remove(s.IndexOf(substring, StringComparison.OrdinalIgnoreCase), substring.Length);

        static string RemoveOneCharacter(string s, char c) => s.Remove(s.IndexOf(c), 1);

        static string RemoveLastCharacter(string s) => s.Remove(s.Length - 1);

        static string Rot13(string s) => new string(s.ToCharArray().Select(c => 'a' <= c && c <= 'm' || 'A' <= c && c <= 'M' ? (char)(c + 13) : 'n' <= c && c <= 'z' || 'N' <= c && c <= 'Z' ? (char)(c - 13) : c).ToArray());
    }
}