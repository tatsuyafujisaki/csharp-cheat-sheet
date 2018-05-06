using System;

namespace CheatSheet
{
    static class Radix
    {
        static string ToBase(long n, int targetBase)
        {
            switch (targetBase)
            {
                case 2:
                case 8:
                case 10:
                case 16:
                    return Convert.ToString(n, targetBase);
                default:
                    const long bits = 64;
                    const string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

                    if (n == 0)
                    {
                        return "0";
                    }

                    if (targetBase < 2 || digits.Length < targetBase)
                    {
                        throw new ArgumentOutOfRangeException(nameof(targetBase), targetBase, "Must be between 2 and the length of predefined digits.");
                    }

                    var i = bits - 1;
                    var cs = new char[bits];

                    while (0 < n)
                    {
                        cs[i--] = digits[(int)n % targetBase];
                        n /= targetBase;
                    }

                    return new string(cs, (int)(i + 1), (int)(bits - i - 1));
            }
        }

        static long ToDecimal(string s, int sourceBase)
        {
            switch (sourceBase)
            {
                case 2:
                case 8:
                case 10:
                case 16:
                    return Convert.ToInt64(s, sourceBase);
                default:
                    const string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

                    if (string.IsNullOrEmpty(s))
                    {
                        return 0;
                    }

                    if (sourceBase < 2 || digits.Length < sourceBase)
                    {
                        throw new ArgumentOutOfRangeException(nameof(sourceBase), sourceBase, "Must be between 2 and the length of predefined digits.");
                    }

                    s = s.ToLower();

                    long result = 0;
                    long multiplier = 1;

                    for (var i = s.Length - 1; 0 <= i; i--)
                    {
                        result += digits.IndexOf(s[i]) * multiplier;
                        multiplier *= sourceBase;
                    }

                    return result;
            }
        }

        static long ConvertBase(long nInSourceBase, long sourceBase, long targetBase)
        {
            if (nInSourceBase == 0) { return 0; }

            return targetBase * ConvertBase(nInSourceBase / sourceBase, sourceBase, targetBase) + (nInSourceBase % sourceBase);
        }
    }
}