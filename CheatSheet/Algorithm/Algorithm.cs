using System;
using System.Collections;
using System.Collections.Generic;

namespace CheatSheet.Algorithm
{
    static class Algorithm
    {
        static long Sum(long from, long to) => (to - from + 1) * (to + from) / 2;

        // https://en.wikipedia.org/wiki/Integer_factorization
        static IEnumerable<long> GetFactors(long n)
        {
            var factors = new List<long>();

            for (var divisor = 2; 1 < n; divisor++)
            {
                while (n % divisor == 0)
                {
                    factors.Add(divisor);
                    n /= divisor;
                }
            }

            return factors;
        }

        static bool IsPrime(long n)
        {
            if (n == 1) { return false; }

            var sqrt = (long)Math.Sqrt(n);

            for (long i = 2; i <= sqrt; ++i)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        // Get primes up to n inclusive.
        static IEnumerable<int> GetPrimes(int n)
        {
            var primes = new List<int>((int)(n / (Math.Log(n) - 1.08366))) { 2 };
            var composite = new BitArray(n + 1);
            var squareRoot = (int)Math.Sqrt(n);

            for (var i = 3; i <= n; i += 2)
            {
                if (composite[i]) { continue; }

                primes.Add(i);

                if (squareRoot < i) { continue; }

                for (var j = i * i; j <= n; j += 2 * i)
                {
                    composite[j] = true;
                }
            }
            return primes;
        }

        static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                var t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        static long Lcm(long a, long b) => a * b / Gcd(a, b);

        static long Fibonacci(long n)
        {
            if (n == 0) { return 0; }

            long a = 0;
            long b = 1;

            while (0 < --n)
            {
                var t = a + b;
                a = b;
                b = t;
            }

            return b;
        }
    }
}
