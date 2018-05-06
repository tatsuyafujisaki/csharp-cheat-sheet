using System.Numerics;

namespace CheatSheet
{
    // Reference
    // https://en.wikipedia.org/wiki/Modular_multiplicative_inverse
    // http://www.geeksforgeeks.org/multiplicative-inverse-under-modulo-m
    // https://codeaccepted.wordpress.com/2014/02/15/output-the-answer-modulo-109-7
    // http://nagoyacoder.web.fc2.com/topcoder/topcoder_cpp4.html
    static class Modulo
    {
        const long M = 10 ^ 9 + 7;

        static long Add(long a, long b, long m = M) => (a + b) % m;

        static long Sub(long a, long b, long m = M) => (a + m - b) % m;

        static long Mul(long a, long b, long m = M) => ((a % m) * (b % m)) % m;

        static long Div(long a, long b, long m = M) => (a * ModInverseByExtendedGcd(b, m)) % m;

        // Get MMI (Modular Multiplicative Inverse) using the extended Euclidean algorithm.
        // Assumption: a and m are coprime. i.e., gcd(a, m) = 1.
        // The time complexity is O(log(m))
        static long ModInverseByExtendedGcd(long a, long m)
        {
            if (m == 1)
            {
                return 0;
            }

            var m0 = m;

            long x0 = 0;
            long x1 = 1;

            while (1 < a)
            {
                var q = a / m;

                var t = m;
                m = a % m;
                a = t;

                t = x0;

                x0 = x1 - q * x0;
                x1 = t;
            }

            return 0 <= x1 ? x1 : x1 + m0;
        }

        // Get MMI (Modular Multiplicative Inverse) using a built-in function.
        // Assumption: m is price. a and m are coprime. i.e., gcd(a, m) = 1.
        // The time complexity is O(log(m)) but slower than ModInverseByExtendedGcd(...)
        static long ModInverseByFelmatsLittleTheorem(int a, int m) => (long)BigInteger.ModPow(a, m - 2, m);
    }
}