using System;
using System.Collections.Generic;

namespace CheatSheet.Algorithm
{
    static class BinaryTree
    {
        // Usage 1: string
        // var leftLeaf = new BinaryTree.Node<string>("Apple", null, null);
        // var rightLeaf = new BinaryTree.Node<string>("Banana", null, null);
        // var root = new BinaryTree.Node<string>("Orange", leftLeaf, rightLeaf);
        // var bfsResult = BinaryTree.Bfs(root, "Banana", (s1, s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase));
        // var dfsResult = BinaryTree.Dfs(root, "Banana", (s1, s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase));

        // Usage 2: long
        // var leftLeaf = new BinaryTree.Node<long>(10, null, null);
        // var rightLeaf = new BinaryTree.Node<long>(20, null, null);
        // var root = new BinaryTree.Node<long>(30, leftLeaf, rightLeaf);
        // var bfsResult = BinaryTree.Bfs(root, 20, (a, b) => a == b);
        // var dfsResult = BinaryTree.Dfs(root, 20, (a, b) => a == b);

        class Node<T>
        {
            internal Node<T> Left { get; }
            internal Node<T> Right { get; }
            internal T Data { get; }

            Node(T data, Node<T> left, Node<T> right)
            {
                Data = data;
                Left = left;
                Right = right;
            }
        }

        static Node<T> Bfs<T>(Node<T> root, T findMe, Func<T, T, bool> eq)
        {
            var q = new Queue<Node<T>>(new[] { root });

            while (0 < q.Count)
            {
                var current = q.Dequeue();

                if (current == null)
                {
                    continue;
                }

                if (eq(current.Data, findMe))
                {
                    return current;
                }

                q.Enqueue(current.Left);
                q.Enqueue(current.Right);
            }

            return null;
        }

        static Node<T> Dfs<T>(Node<T> root, T findMe, Func<T, T, bool> eq)
        {
            var q = new Stack<Node<T>>(new[] { root });

            while (0 < q.Count)
            {
                var current = q.Pop();

                if (current == null)
                {
                    continue;
                }

                if (eq(current.Data, findMe))
                {
                    return current;
                }

                q.Push(current.Right);
                q.Push(current.Left);
            }

            return null;
        }
    }
}