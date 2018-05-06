using System;

namespace CheatSheet
{
    static class Math1
    {
        // Math.Round(x) and Math.Round(x, fractionalDigitCount) return the nearest *even* whole number, which is useless.
        // e.g. Math.Round(2.5) returns 2 rather than 3.
        // e.g. Math.Round(2.05, 1) returns 2.0 rather than 2.1.

        internal static decimal RoundToNearestWholeNumber(decimal x) => Math.Round(x, MidpointRounding.AwayFromZero);
        internal static decimal Round(decimal x, int fractionalDigitCount) => Math.Round(x, fractionalDigitCount, MidpointRounding.AwayFromZero);
        internal static string RoundAsString(decimal x, int fractionalDigitCount) => x.ToString("#,##0");
        internal static string RoundToTwoDecimalPlacesAsString(decimal x, int fractionalDigitCount) => x.ToString("#,##0.##");

        // Explanatory wrapper
        internal static decimal RoundUpToNearestWholeNumber(decimal x) => Math.Ceiling(x);

        internal static decimal RoundUp(decimal x, int fractionalDigitCount)
        {
            var power = (decimal)Math.Pow(10, fractionalDigitCount);
            return Math.Ceiling(x * power) / power;
        }

        // Explanatory wrapper
        internal static decimal RoundDownToNearestWholeNumber(decimal x) => Math.Floor(x);

        internal static decimal RoundDown(decimal x, int fractionalDigitCount)
        {
            var power = (decimal)Math.Pow(10, fractionalDigitCount);
            return Math.Floor(x * power) / power;
        }
    }
}