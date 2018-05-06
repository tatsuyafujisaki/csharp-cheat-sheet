using System;
using System.Collections.Generic;

namespace CheatSheet.Bool
{
    static class Sorted
    {
        static bool IsSorted(IReadOnlyList<int> xs)
        {
            for (var i = 0; i < xs.Count - 1; i++)
            {
                if (xs[i + 1] < xs[i])
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsSortedDescendingly(IReadOnlyList<int> xs)
        {
            for (var i = 0; i < xs.Count - 1; i++)
            {
                if (xs[i] < xs[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsSorted(IReadOnlyList<string> ss)
        {
            for (var i = 0; i < ss.Count - 1; i++)
            {
                if (0 < string.Compare(ss[i], ss[i + 1], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsSortedDescendingly(IReadOnlyList<string> ss)
        {
            for (var i = 0; i < ss.Count - 1; i++)
            {
                if (string.Compare(ss[i], ss[i + 1], StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}