using CheatSheet.Factory;
using System;
using System.Globalization;
using System.IO;

namespace CheatSheet.DateTime1
{
    static class DateTime1
    {
        // Explanatory wrapper
        static string Yyyymmddhhmm() => DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

        // Explanatory wrapper
        static string GetDateWithPadZero(DateTime dt) => dt.ToString("yyyy-MM-dd");

        // Explanatory wrapper
        // 2020- 1- 1
        static string GetDateWithPadSpace(DateTime dt) => $"{dt.Year}-{dt.Month,2}-{dt.Day,2}";

        static DateTime GetDate(string s, string format) => DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);

        static (bool, DateTime) TryParse(string s, string format) => ValueTuple.Create(DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt), dt);

        static string GetAbbreviatedEnglishMonthName(DateTime dt) => dt.ToString("MMM", Flyweight.GetCultureInfo("en-US"));
        static string GetFullEnglishMonthName(DateTime dt) => dt.ToString("MMMM", Flyweight.GetCultureInfo(("en-US")));
        static string GetJapaneseDayOfWeek(DateTime dt) => dt.ToString("ddd", Flyweight.GetCultureInfo("ja-JP"));

        static DateTime GetValidDate(int year, int month, int day) => new DateTime(year, month, Math.Min(DateTime.DaysInMonth(year, month), day));

        static string AppendTimestamp(string path) => Io.File1.AppendSuffix(path, DateTime.Now.ToString("-yyyy-MM-dd_HH-mm-ss"));

        static void AppendTimestampToExistingFile(string path)
        {
            if (path == null || !File.Exists(path))
            {
                return;
            }

            var newPath = Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}-{DateTime.Now:yyyy-MM-dd_HH-mm}{Path.GetExtension(path)}");

            File.Delete(newPath);
            File.Move(path, newPath);
        }

        static double DiffInYear(DateTime dtFrom, DateTime dtTo) => (dtTo - dtFrom).TotalDays / 365.25;
        static double DiffInMonth(DateTime dtFrom, DateTime dtTo) => (dtTo - dtFrom).TotalDays / 365.25 / 12;

        static DateTime GetNextBusinessDay(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return dt.AddDays(3);
                case DayOfWeek.Saturday:
                    return dt.AddDays(2);
                default:
                    return dt.AddDays(1);
            }
        }
    }
}