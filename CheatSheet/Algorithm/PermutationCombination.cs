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

        // Result:
        // A B
        // A C
        // B A
        // B C
        // C A
        // C B
        internal static void DemonstratePermutation()
        {
            var ss = new List<string> { "A", "B", "C" };

            foreach (var xs in Permutate(ss, 2))
            {
                Console.WriteLine(string.Join(" ", xs));
            }
        }

        // Result:
        // A B
        // A C
        // B C
        internal static void DemonstrateCombinationWithoutRepetition()
        {
            var ss = new List<string> { "A", "B", "C" };

            foreach (var xs in CombineWithoutRepetition(ss, 2))
            {
                Console.WriteLine(string.Join(" ", xs));
            }
        }

        // Result:
        // A A
        // A B
        // A C
        // B B
        // B C
        // C C
        internal static void DemonstrateCombinationWithRepetition()
        {
            var ss = new List<string> { "A", "B", "C" };

            foreach (var xs in CombineWithRepetition(ss, 2))
            {
                Console.WriteLine(string.Join(" ", xs));
            }
        }

        // Create 
        // If the number of elements of the result row is 3 ...
        // listsOfCandidates[0] ... Candidates for the first element
        // listsOfCandidates[1] ... Candidates for the second element
        // listsOfCandidates[2] ... Candidates for the third element
        static IEnumerable<IEnumerable<T>> CreateRows<T>(IEnumerable<HashSet<T>> listsOfCandidates)
        {
            IEnumerable<IEnumerable<T>> CreateRows_(IEnumerable<IEnumerable<T>> resultRows, IEnumerable<HashSet<T>> listsOfCandidateElements_)
            {
                if (listsOfCandidateElements_.Any())
                {
                    var candidateElements = listsOfCandidateElements_.First();
                    listsOfCandidateElements_ = listsOfCandidateElements_.Skip(1);
                    return candidateElements.SelectMany(candidateElement => CreateRows_(resultRows.Select(resultRow => resultRow.Concat(new[] { candidateElement })), listsOfCandidateElements_));
                }

                return resultRows;
            }

            return CreateRows_(new List<List<T>> { new List<T>() }, listsOfCandidates);
        }

        static IEnumerable<string> CreateWords(IEnumerable<IEnumerable<char>> listsOfCandidateChars)
        {
            IEnumerable<string> CreateWords_(IEnumerable<string> words, IEnumerable<IEnumerable<char>> listOfCandidateChars_)
            {
                if (listOfCandidateChars_.Any())
                {
                    var candidateChars = listOfCandidateChars_.First();
                    listOfCandidateChars_ = listOfCandidateChars_.Skip(1);

                    return candidateChars.SelectMany(candidateChar => CreateWords_(words.Select(word => word + candidateChar), listOfCandidateChars_));
                }

                return words;
            }

            return CreateWords_(new[] { "" }, listsOfCandidateChars);
        }

        // Result:
        // 1 3 5
        // 1 3 6
        // 1 4 5
        // 1 4 6
        // 2 3 5
        // 2 3 6
        // 2 4 5
        // 2 4 6
        internal static void DemonstrateCreateRows()
        {
            var listsOfCandidates = new List<HashSet<char>>
            {
                new HashSet<char> { '1', '2' },
                new HashSet<char> { '3', '4' },
                new HashSet<char> { '5', '6' }
            };

            foreach (var tokens in CreateRows(listsOfCandidates))
            {
                foreach (var token in tokens)
                {
                    Console.Write(token + " ");

                }

                Console.WriteLine();
            }
        }

        // Result:
        // ACE
        // ACF
        // ADE
        // ADF
        // BCE
        // BCF
        // BDE
        // BDF
        internal static void DemonstrateCreateWords()
        {
            var listsOfCandidates = new List<HashSet<char>>
            {
                new HashSet<char> { 'A', 'B' },
                new HashSet<char> { 'C', 'D' },
                new HashSet<char> { 'E', 'F' }
            };

            foreach (var word in CreateWords(listsOfCandidates))
            {
                Console.WriteLine(word);
            }
        }
    }
}