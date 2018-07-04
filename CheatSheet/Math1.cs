using System;

namespace CheatSheet
{
    static class Math1
    {
        // Math.Round(x) and Math.Round(x, fractionalDigitCount) return the nearest *even* whole number, which is useless.
        // e.g. Math.Round(2.5) returns 2 rather than 3.
        // e.g. Math.Round(2.05, 1) returns 2.0 rather than 2.1.

        internal static decimal Round(decimal d) => Math.Round(d, MidpointRounding.AwayFromZero);
        internal static decimal Round(decimal d, int fractionalDigitCount) => Math.Round(d, fractionalDigitCount, MidpointRounding.AwayFromZero);
        internal static string RoundToString(decimal d, int fractionalDigitCount) => d.ToString("#,##0");
        internal static string RoundToTwoDecimalPlacesToString(decimal d, int fractionalDigitCount) => d.ToString("#,##0.##");

        // Explanatory wrapper
        internal static decimal RoundUp(decimal d) => Math.Ceiling(d);

        internal static decimal RoundUp(decimal d, int fractionalDigitCount)
        {
            var power = (decimal)Math.Pow(10, fractionalDigitCount);
            return Math.Ceiling(d * power) / power;
        }

        // Explanatory wrapper
        internal static decimal RoundDown(decimal x) => Math.Floor(x);

        internal static decimal RoundDown(decimal x, int fractionalDigitCount)
        {
            var power = (decimal)Math.Pow(10, fractionalDigitCount);
            return Math.Floor(x * power) / power;
        }
    }
}