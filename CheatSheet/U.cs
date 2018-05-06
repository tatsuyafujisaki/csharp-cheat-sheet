using System;
using System.Collections.Generic;
using System.Linq;

namespace CheatSheet
{
    static class U
    {
        // Note that using a variable as an index throws an exception.
        // OK -> Swap(ref xs[0], xs[1]);
        // Error -> Swap(ref xs[i], xs[j]);
        static void Swap<T>(ref T a, ref T b)
        {
            var t = a;
            a = b;
            b = t;
        }

        internal static void Swap<T>(IList<T> xs, int i, int j)
        {
            var t = xs[i];
            xs[i] = xs[j];
            xs[j] = t;
        }

        internal static void Reverse<T>(IList<T> xs, int fromIndex, int toIndex)
        {
            for (int i = fromIndex, j = toIndex; i < j; i++, j--)
            {
                Swap(xs, i, j);
            }
        }

        static IEnumerable<ValueTuple<T1, T2>> Concat<T1, T2>(IEnumerable<T1> xs, IEnumerable<T2> ys) => xs.Zip(ys, ValueTuple.Create);

        // k is zero-based.
        // 0 or 1 is returned.
        static int GetBit(int x, int k) => 1 & (x >> k);

        static int GetIndexOfMin(IList<long> xs)
        {
            var min = xs[0];
            var indexOfMin = 0;

            var n = xs.Count;
            for (var i = 1; i < n; i++)
            {
                if (xs[i] < min)
                {
                    min = xs[i];
                    indexOfMin = i;
                }
            }

            return indexOfMin;
        }

        static int GetIndexOfMax(IList<long> xs)
        {
            var max = xs[0];
            var indexOfMax = 0;

            var n = xs.Count;
            for (var i = 1; i < n; i++)
            {
                if (max < xs[i])
                {
                    max = xs[i];
                    indexOfMax = i;
                }
            }

            return indexOfMax;
        }

        // Explanatory wrapper
        static IEnumerable<T> Combine<T>(IEnumerable<T> front, IEnumerable<T> rear) => front.Concat(rear);

        static IEnumerable<T> GetEvenElements<T>(IEnumerable<T> xs) => xs.Where((c, i) => i % 2 == 0);

        static IEnumerable<T> GetOddElements<T>(IEnumerable<T> xs) => xs.Where((c, i) => i % 2 == 1);

        static List<T> GetRange<T>(List<T> xs, int start, int end) => xs.GetRange(start, end - start + 1);

        static List<T> PopRange<T>(List<T> xs, int index, int count)
        {
            var ys = xs.GetRange(index, count);
            xs.RemoveRange(index, count);
            return ys;
        }

        // Explanatory wrapper
        static IEnumerable<T> Flatten<T>(IEnumerable<IEnumerable<T>> xss) => xss.SelectMany(x => x);

        static List<HashSet<T>> CreateHashSetList<T>(int n)
        {
            var hss = new List<HashSet<T>>();

            for (var i = 0; i < n; i++)
            {
                hss.Add(new HashSet<T>());
            }

            return hss;
        }

        // CompressCoordinates(new[] { 100, 10, 1, 10, 10000 }); // { 2, 1, 0, 1, 3, }
        static List<int> CompressCoordinates<T>(T[] xs) => xs.Select(a => xs.Distinct()
                                                                            .OrderBy(v => v)
                                                                            .Select((v, i) => new { v, i })
                                                                            .ToDictionary(p => p.v, p => p.i)[a]).ToList();

        static HashSet<HashSet<T>> CreateNestedHashSet<T>() => new HashSet<HashSet<T>>(HashSet<T>.CreateSetComparer());

        static HashSet<HashSet<T>> BuildSubgraph<T>(HashSet<HashSet<T>> hss)
        {
            while (true)
            {
                var tempHss = CreateNestedHashSet<T>();

                foreach (var hs1 in hss)
                {
                    if (hs1.Count == 1) { continue; }

                    foreach (var hs2 in hss)
                    {
                        if (hs2.Count == 1 || hs1.SetEquals(hs2) || !hs1.Intersect(hs2).Any()) { continue; }

                        var tempHs = new HashSet<T>(hs1);
                        tempHs.UnionWith(hs2);

                        if (!hss.Contains(tempHs))
                        {
                            tempHss.Add(tempHs);
                        }
                    }
                }

                if (!tempHss.Any()) { return hss; }

                hss.UnionWith(tempHss);
            }
        }

        static void RemoveItems<T1, T2>(IDictionary<T1, T2> d, Func<KeyValuePair<T1, T2>, bool> isRemovable)
        {
            foreach (var kvp in d.Where(isRemovable).ToList())
            {
                d.Remove(kvp.Key);
            }
        }
    }
}
