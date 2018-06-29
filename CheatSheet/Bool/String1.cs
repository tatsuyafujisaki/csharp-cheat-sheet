using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CheatSheet.Bool
{
    static class String1
    {
        internal static bool EqualsIgnoreCase(string s1, string s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        // The following throws an error when s1 is null.
        // s1.Equals(s2, StringComparison.OrdinalIgnoreCase);

        internal static bool EqualsIgnoreCase(HashSet<string> hs, IEnumerable<string> ss)
        {
            if (hs.Comparer != StringComparer.OrdinalIgnoreCase)
            {
                hs = new HashSet<string>(hs, StringComparer.OrdinalIgnoreCase);
            }

            return hs.SetEquals(ss);
        }

        static bool StartsWithIgnoreCase(string s1, string s2) => s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase);
        internal static bool ContainsIgnoreCase(string s, string findMe) => -1 < s.IndexOf(findMe, StringComparison.OrdinalIgnoreCase);
        internal static bool ContainsIgnoreCase(IEnumerable<string> ss, string s) => ss.Contains(s, StringComparer.OrdinalIgnoreCase);

        // Cannot make it generic as '==' is unavailable for a generic type.
        static bool IsAllSame(IEnumerable<int> xs) => xs.All(x => x == xs.First());

        // s.All(...) returns true if s is an empty string.
        static bool IsLetter(string s) => !string.IsNullOrEmpty(s) && s.All(char.IsLetter);
        // Second fastest
        // => Regex.IsMatch(s, @"^[a-zA-Z]+$");
        // Third fastest
        // => Regex.IsMatch(s, @"^[a-z]+$", RegexOptions.IgnoreCase);

        // s.All(...) returns true if s is an empty string.
        static bool IsDigit(string s) => !string.IsNullOrEmpty(s) && s.All(char.IsDigit);
        // Second fastest
        // => int.TryParse(s, out var n);
        // Third fastest
        // => Regex.IsMatch(s, @"^[\d]+$");

        static bool IsOneToFiveDigits(string s) => Regex.IsMatch(s, @"^[a-zA-Z]{1,5}$");

        static bool IsFourDigits(string s) => Regex.IsMatch(s, @"^[\d]{4}$");

        // s.All(...) returns true if s is an empty string.
        static bool IsAlphanumeric(string s) => !string.IsNullOrEmpty(s) && s.All(char.IsLetterOrDigit);
        // Second fastest
        // => Regex.IsMatch(s, @"^[a-zA-Z\d]+$");
        // Third fastest
        // => Regex.IsMatch(s, @"^[a-z\d]+$", RegexOptions.IgnoreCase);

        internal static bool IsNumeric(string s) => double.TryParse(s, out var _);

        // All non-repeating decimals and some repeating decimals can be retained in computers
        // while other repeating decimals cannot.
        // Retainable repeating decimal => 10 / 3 (= 3.333 ...)
        // Unretainable repeating decimal => 1 / 3 (= 0.333...)
        static bool IsUnretainableRepeatingDecimal(decimal numerator, decimal denominator) => numerator != numerator / denominator * denominator;

        static bool IsValidHhmm(string s) => Regex.IsMatch(s, @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");

        static bool WishToContinue()
        {
            Console.Write("Do you wish to continue? (y/N): ");
            return ContainsIgnoreCase(new[] { "Y", "Yes" }, Console.ReadLine());
        }

        static bool Eq<T>(ISet<T> s1, ISet<T> s2) => s1.SetEquals(s2);

        static bool IsValidDate(string s, string format = "yyyy-MM-dd") => DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _);

        static bool IsBetween(int from, int x, int to) => from <= x && x <= to;
        static bool IsBetween(DateTime from, DateTime dt, DateTime to) => from <= dt && dt <= to;
        static bool Overlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2) => start1 <= end2 && start2 <= end1;

        // Explanatory wrapper
        static bool IsNullOrDbNull(DataRow dr, string columnName) => dr.IsNull(columnName);

        // Explanatory wrapper
        static bool IsDbNull(object o) => o == DBNull.Value;
        // Second fastest
        // => x is DBNull;
        // Third fastest 
        // => Convert.IsDBNull(x);
    }
}