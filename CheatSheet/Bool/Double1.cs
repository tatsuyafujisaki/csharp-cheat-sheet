using System;

namespace CheatSheet.Bool
{
    static class Double1
    {
        static bool Equals(double a, double b) => Math.Abs(a - b) <= double.Epsilon;
    }
}
