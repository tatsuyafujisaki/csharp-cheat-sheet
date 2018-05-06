using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CheatSheet.String
{
    static class Japanese
    {
        // https://msdn.microsoft.com/library/ms912047.aspx
        static string Zenkaku(string s) => Strings.StrConv(s, VbStrConv.Wide, 1041);

        // https://msdn.microsoft.com/library/ms912047.aspx
        static string Hankaku(string s) => Strings.StrConv(s, VbStrConv.Narrow, 1041);

        static string ZenkakuWithUnicodeEscaped(string s)
        {
            const char unicodeToEscape = '®';
            return string.Join(unicodeToEscape.ToString(), s.Split(unicodeToEscape).Select(t => Strings.StrConv(t, VbStrConv.Wide, 1041)));
        }

        // https://unicode.org/charts/PDF/UFF00.pdf
        // https://en.wikipedia.org/wiki/Half-width_kana
        static string ConvertKatakanaToZenkaku(string s) => Regex.Replace(s, "[｡-ﾟ]+", m => Strings.StrConv(m.Value, VbStrConv.Wide, 1041));

        // https://unicode.org/charts/PDF/U30A0.pdf
        // https://en.wikipedia.org/wiki/Katakana_(Unicode_block)
        static string ConvertAlphanumericsSymbolsToHankaku(string s) => Regex.Replace(s, "[^゠-ヿ]+", m => Strings.StrConv(m.Value, VbStrConv.Narrow, 1041));

        static string NormalizeJapanese(string s) => ConvertAlphanumericsSymbolsToHankaku(ConvertKatakanaToZenkaku(s));

        static string GetUnicodeString(int n) => Encoding.Unicode.GetString(new[] { (byte)(n % 256), (byte)(n / 256) });

        static string ToJapaneseNumeral(long n)
        {
            if(n == 0)
            {
                return "0";
            }

            var sb = new StringBuilder();

            foreach(var unit in new[] { "", "万", "億", "兆" })
            {
                var m = n % 10000;

                if(0 < m)
                {
                    sb.Insert(0, unit);
                    sb.Insert(0, m);
                }

                n /= 10000;
            }

            if (0 < n)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "Must not be larger than 兆.");
            }

            return sb.ToString();
        }

        // Explanatory wrapper
        static Encoding GetUtf8WithoutBom() => new UTF8Encoding(false);

        static IEnumerable<string> GetCircledNumbers()
        {
            const int circled0 = 9450;
            const int circled1 = 9312;
            const int circled21 = 12881;
            const int circled36 = 12977;

            return new[] { circled0 }
                .Concat(Enumerable.Range(circled1, 20))
                .Concat(Enumerable.Range(circled21, 15))
                .Concat(Enumerable.Range(circled36, 15))
                .Select(GetUnicodeString);
        }

        static string GetCircledNumber(int n)
        {
            const int circled0 = 9450;
            const int circled1 = 9312;
            const int circled21 = 12881;
            const int circled36 = 12977;

            int code;

            if (n == 0)
            {
                code = circled0;
            }
            else if (n <= 20)
            {
                code = circled1 + n - 1;
            }
            else if (n <= 35)
            {
                code = circled21 + n - 21;
            }
            else if (n <= 50)
            {
                code = circled36 + n - 36;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "Must be 50 less.");
            }

            return GetUnicodeString(code);
        }
    }
}