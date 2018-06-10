using System;

namespace CheatSheet.Factory
{
    static class Singleton
    {
        static readonly Random R = new Random();

        internal static Random GetRandom() => R;
    }
}