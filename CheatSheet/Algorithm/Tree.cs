using System;
using System.Collections.Generic;

namespace CheatSheet.Algorithm
{
    static class Tree
    {
        // Usage 1: string
        // var leftLeaf = new Tree.Node<string>("Apple", null, null);
        // var rightLeaf = new Tree.Node<string>("Banana", null, null);
        // var root = new Tree.Node<string>("Orange", leftLeaf, rightLeaf);
        // var bfsResult = Tree.Bfs(root, "Banana", (s1, s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase));
        // var dfsResult = Tree.Dfs(root, "Banana", (s1, s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase));

        // Usage 2: long
        // var leftLeaf = new Tree.Node<long>(10, null, null);
        // var rightLeaf = new Tree.Node<long>(20, null, null);
        // var root = new Tree.Node<long>(30, leftLeaf, rightLeaf);
        // var bfsResult = Tree.Bfs(root, 20, (a, b) => a == b);
        // var dfsResult = Tree.Dfs(root, 20, (a, b) => a == b);

        sealed class Node<T>
        {
            internal List<Node<T>> Children { get; }
            internal T Data { get; }

            Node(T data, List<Node<T>> children)
            {
                Data = data;
                Children = children;
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

                foreach (var child in current.Children)
                {
                    q.Enqueue(child);
                }
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

                // Rewrite the following to loop backwards if children need to be processed from left.
                foreach (var child in current.Children)
                {
                    q.Push(child);
                }
            }

            return null;
        }
    }
}