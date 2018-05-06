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

        // If the number of elements of the result row is 3 ...
        // listsOfCandidates[0] ... Candidates the first element
        // listsOfCandidates[1] ... Candidates the second element
        // listsOfCandidates[2] ... Candidates the third element
        static IEnumerable<IEnumerable<T>> Combine<T>(IEnumerable<HashSet<T>> listsOfCandidates)
        {
            IEnumerable<IEnumerable<T>> Combine_(IEnumerable<IEnumerable<T>> resultRows, IEnumerable<HashSet<T>> listsOfCandidates_)
            {
                if (listsOfCandidates_.Any())
                {
                    var candidates = listsOfCandidates_.First();
                    listsOfCandidates_ = listsOfCandidates_.Skip(1);
                    return candidates.SelectMany(candidateElement => Combine_(resultRows.Select(resultRow => resultRow.Concat(new[] { candidateElement })), listsOfCandidates_));
                }

                return resultRows;
            }

            return Combine_(new List<List<T>> { new List<T>() }, listsOfCandidates);
        }

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