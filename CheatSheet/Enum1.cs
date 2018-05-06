using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CheatSheet
{
    enum ChineseZodiac
    {
        Rat,
        Ox,
        Tiger,
        Rabbit,
        Dragon,
        Snake,
        Horse,
        Goat,
        Monkey,
        Rooster,
        Dog,
        Pig
    }

    enum WesternZodiac
    {
        Aries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius,
        Capricorn,
        Aquarius,
        Pisces
    }

    static class Enum1
    {
        static string[] GetEnumNames<T>() => Enum.GetNames(typeof(T));

        static IEnumerable<T> GetEnumValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();

        static T GetEnum<T>(string s) => (T)Enum.Parse(typeof(T), s, true);

        internal static ChineseZodiac GetChineseZodiac(int year) => (ChineseZodiac)((year + 8) % 12);

        internal static WesternZodiac GetWesternZodiac(int month, int day)
        {
            switch (month)
            {
                case 1:
                    return day < 20 ? WesternZodiac.Capricorn : WesternZodiac.Aquarius;
                case 2:
                    return day < 19 ? WesternZodiac.Aquarius : WesternZodiac.Pisces;
                case 3:
                    return day < 21 ? WesternZodiac.Pisces : WesternZodiac.Aries;
                case 4:
                    return day < 20 ? WesternZodiac.Aries : WesternZodiac.Taurus;
                case 5:
                    return day < 21 ? WesternZodiac.Taurus : WesternZodiac.Gemini;
                case 6:
                    return day < 22 ? WesternZodiac.Gemini : WesternZodiac.Cancer;
                case 7:
                    return day < 23 ? WesternZodiac.Cancer : WesternZodiac.Leo;
                case 8:
                    return day < 23 ? WesternZodiac.Leo : WesternZodiac.Virgo;
                case 9:
                    return day < 23 ? WesternZodiac.Virgo : WesternZodiac.Libra;
                case 10:
                    return day < 24 ? WesternZodiac.Libra : WesternZodiac.Scorpio;
                case 11:
                    return day < 23 ? WesternZodiac.Scorpio : WesternZodiac.Sagittarius;
                case 12:
                    return day < 22 ? WesternZodiac.Sagittarius : WesternZodiac.Capricorn;
                default:
                    throw new InvalidEnumArgumentException(nameof(month), (int)month, typeof(WesternZodiac));
            }
        }
    }
}
