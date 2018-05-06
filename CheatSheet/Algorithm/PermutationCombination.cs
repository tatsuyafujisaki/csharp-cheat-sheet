using System;
using System.Collections.Generic;
using System.Linq;

namespace CheatSheet.Algorithm
{
    static class PermutationCombination
    {
        internal static long Factorial(long n)
        {
            long result = 1;

            for (var i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        // a.k.a. nPr
        internal static long PermutationCount(int n, int r)
        {
            long result = 1;

            for (var i = n - r + 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        // a.k.a. nCr
        internal static long CombinationCount(int n, int r) => PermutationCount(n, r) / Factorial(r);

        // Permutate(xs, n - 1) returns rows where each row has n-1 elements,
        // so you need to add a not-yet-added element, to each row.
        internal static IEnumerable<IEnumerable<T>> Permutate<T>(IEnumerable<T> xs, int k) =>
            k == 1
            ? xs.Select(x => new T[] { x })
            : Permutate(xs, k - 1).SelectMany(resultRow => xs.Where(x => !resultRow.Contains(x)), (resultRow, x) => resultRow.Append(x));

        internal static IEnumerable<IEnumerable<T>> CombineWithoutRepetition<T>(IEnumerable<T> xs, int k) =>
            k == 1
            ? xs.Select(x => new T[] { x })
            : xs.SelectMany((x, i) => CombineWithoutRepetition(xs.Skip(i + 1), k - 1).Select(resultRow => (new[] { x }).Concat(resultRow)));

        internal static IEnumerable<IEnumerable<T>> CombineWithRepetition<T>(IEnumerable<T> xs, int k) =>
            k == 1
            ? xs.Select(x => new T[] { x })
            : xs.SelectMany((x, i) => CombineWithRepetition(xs.Skip(i), k - 1).Select(resultRow => (new[] { x }).Concat(resultRow)));

        internal static void DemonstratePermutation<T>(IEnumerable<T> xs, int k)
        {
            foreach (var ys in Permutate(xs, k))
            {
                Console.WriteLine(string.Join(" ", ys));
            }
        }

        internal static void DemonstrateCombinationWithoutRepetition<T>(IEnumerable<T> xs, int k)
        {
            foreach (var ys in CombineWithoutRepetition(xs, k))
            {
                Console.WriteLine(string.Join(" ", ys));
            }
        }

        internal static void DemonstrateCombinationWithRepetition<T>(IEnumerable<T> xs, int k)
        {
            foreach (var ys in CombineWithRepetition(xs, k))
            {
                Console.WriteLine(string.Join(" ", ys));
            }
        }
    }
}