using System;
using System.Collections.Generic;
using System.Linq;

namespace CheatSheet
{
    static class Collection
    {
        internal static class Samples
        {
            class Country
            {
                internal string Name;
                internal IEnumerable<string> Cities;
            }

            internal static void SampleOfSelectMany1()
            {
                var countries = new List<Country>
                {
                    new Country { Name = "Japan", Cities = new List<string> { "Tokyo", "Osaka" } },
                    new Country { Name = "USA", Cities = new List<string> { "San Francisco", "New York" } }
                };

                var cities = countries.SelectMany(c => c.Cities);

                foreach (var x in countries.SelectMany(c => c.Cities, (country, City) => new { country.Name, City }))
                {
                    Console.WriteLine($"Country = {x.Name}, City = {x.City}");
                }
            }

            internal static void SampleOfSelectMany2()
            {
                foreach (var x in new[] { "Sushi", "Tempura", "Pizza" }.SelectMany(food => food == "Pizza" ? new[] { "Coffee", "Wine" } : new[] { "Matcha", "Sake" }, (Food, Drink) => new { Food, Drink }))
                {
                    Console.WriteLine($"{x.Food} x {x.Drink}");
                }
            }
        }

        static class Array1
        {
            static class Array2D
            {
                static class Chess
                {
                    static void FillBishopControls(char[,] matrix, int mainDiagonalId, int antiDiagonalId)
                    {
                        const char bishopControl = '!';
                        const char bishopSign = '*';

                        var n = matrix.GetLength(0);

                        for (var rowIndex = 0; rowIndex < n; rowIndex++)
                        {
                            for (var columnIndex = 0; columnIndex < n; columnIndex++)
                            {
                                if (matrix[rowIndex, columnIndex] == bishopSign)
                                {
                                    continue;
                                }

                                if (mainDiagonalId == GetMainDiagonalId(rowIndex, columnIndex))
                                {
                                    matrix[rowIndex, columnIndex] = bishopControl;
                                }
                                else if (antiDiagonalId == GetAntidiagonalId(rowIndex, columnIndex))
                                {
                                    matrix[rowIndex, columnIndex] = bishopControl;
                                }
                            }
                        }
                    }

                    static void FillRookControls(char[,] matrix, int rowIndex, int columnIndex)
                    {
                        const char rookControl = '!';
                        const char blankSign = '*';

                        for (var i = 0; i < matrix.GetLength(0); i++)
                        {
                            if (matrix[rowIndex, i] == blankSign)
                            {
                                matrix[rowIndex, i] = rookControl;
                            }

                            if (matrix[i, columnIndex] == blankSign)
                            {
                                matrix[i, columnIndex] = rookControl;
                            }
                        }
                    }
                }

                static int GetRowCount<T>(T[,] matrix) => matrix.GetLength(0);
                static int GetColumnCount<T>(T[,] matrix) => matrix.GetLength(1);
                static int GetTotalCount<T>(T[,] matrix) => matrix.Length;

                static int GetMainDiagonalId(int rowIndex, int columnIndex) => rowIndex - columnIndex;
                static int GetAntidiagonalId(int rowIndex, int columnIndex) => rowIndex + columnIndex;

                static T[,] DeepCopy2DArray<T>(T[,] matrix) => (T[,])matrix.Clone();

                static void Set<T>(T[,] matrix, T item)
                {
                    var n = matrix.GetLength(0);


                    for (var rowIndex = 0; rowIndex < n; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < n; columnIndex++)
                        {
                            matrix[rowIndex, columnIndex] = item;
                        }
                    }
                }

                static void Replace<T>(T[,] matrix, T from, T to)
                {
                    var n = matrix.GetLength(0);

                    for (var rowIndex = 0; rowIndex < n; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < n; columnIndex++)
                        {
                            if (matrix[rowIndex, columnIndex].Equals(from))
                            {
                                matrix[rowIndex, columnIndex] = to;
                            }
                        }
                    }
                }

                static void Print(char[,] matrix)
                {
                    var n = matrix.GetLength(0);

                    for (var rowIndex = 0; rowIndex < n; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < n - 1; columnIndex++)
                        {
                            Console.Write(string.Concat(matrix[rowIndex, columnIndex], ' '));
                        }

                        Console.WriteLine(matrix[rowIndex, n - 1]);
                    }
                }

                static bool[,] ToBoolMatrix(char[,] matrix, char charForTrue)
                {
                    var n = matrix.GetLength(0);

                    var boolMatrix = new bool[n, n];

                    for (var rowIndex = 0; rowIndex < n; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < n; columnIndex++)
                        {
                            if (matrix[rowIndex, columnIndex] == charForTrue)
                            {
                                boolMatrix[rowIndex, columnIndex] = true;
                            }
                        }
                    }

                    return boolMatrix;
                }

                static char[,] ToCharMatrix(bool[,] matrix, char charForTrue, char charForFalse)
                {
                    var n = matrix.GetLength(0);

                    var charMatrix = new char[n, n];

                    for (var rowIndex = 0; rowIndex < n; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < n; columnIndex++)
                        {
                            charMatrix[rowIndex, columnIndex] = matrix[rowIndex, columnIndex] ? charForTrue : charForFalse;
                        }
                    }

                    return charMatrix;
                }
            }

            static class JaggedArray
            {
                static int GetRowCount<T>(IReadOnlyCollection<T[]> xss) => xss.Count;

                // Assume the jagged array is a rectangle.
                static int GetColumnCount<T>(IReadOnlyList<T[]> xss) => xss[0].Length;

                // Assume the jagged array is a rectangle.
                static int GetTotalCount<T>(IReadOnlyList<T[]> xss) => xss.Count * xss[0].Length;

                static T[][] DeepCopyJaggedArray<T>(IEnumerable<IEnumerable<T>> xss) => xss.Select(x => x.ToArray()).ToArray();
            }

            static T[] DeepCopy1DArray<T>(T[] xs)
            {
                var n = xs.Length;
                var ys = new T[n];
                Array.Copy(xs, ys, n);
                return ys;
            }
            // Second fastest
            // static T[] DeepCopy1DArray<T>(T[] xs) => (T[])xs.Clone();
            // Third fastest
            // static T[] DeepCopy1DArray<T>(IEnumerable<T> xs) => xs.ToArray();

            // Explanatory wrapper
            static void SortArray<T>(T[] xs) => Array.Sort(xs);

            // Explanatory wrapper
            static void SortList<T>(List<T> xs) => xs.Sort();

            // Explanatory wrapper
            // For both Array and List
            static IEnumerable<T> DescendinglySort<T>(IEnumerable<T> xs) => xs.OrderByDescending(x => x);

            static void SortByItem1(List<ValueTuple<long, long>> xs) => xs.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            static void SortByItem1ThenItem2(List<ValueTuple<long, long>> xs)
            {
                xs.Sort((a, b) =>
                {
                    var result = a.Item1.CompareTo(b.Item1);
                    return result != 0 ? result : a.Item2.CompareTo(b.Item2);
                });
            }

            static int GetIndexOfUnsortedItem(IReadOnlyList<int> xs)
            {
                for (var i = 1; i < xs.Count; i++)
                {
                    if (xs[i] < xs[i - 1])
                    {
                        return i;
                    }
                }

                // All items are sorted
                return -1;
            }

            static int GetDescendinglyUnsortedIndex(IReadOnlyList<int> xs)
            {
                for (var i = 1; i < xs.Count; i++)
                {
                    if (xs[i - 1] < xs[i])
                    {
                        return i;
                    }
                }

                // Sorted
                return -1;
            }

            static int GetIndexOfUnsortedItem(IReadOnlyList<string> ss)
            {
                for (var i = 1; i < ss.Count; i++)
                {
                    if (0 < string.Compare(ss[i - 1], ss[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return i;
                    }
                }

                // Sorted
                return -1;
            }

            static int GetDescendinglyUnsortedIndex(IReadOnlyList<string> ss)
            {
                for (var i = 1; i < ss.Count; i++)
                {
                    if (string.Compare(ss[i - 1], ss[i], StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        return i;
                    }
                }

                // Sorted
                return -1;
            }
        }

        // Reference System.Collecitons.Immutable if ImmutableHashSet is necessary.
        static class HashSet1
        {
            static HashSet<T> DeepCopy<T>(IEnumerable<T> hs) => new HashSet<T>(hs);
        }
    }
}