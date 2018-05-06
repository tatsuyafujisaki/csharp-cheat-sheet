using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CheatSheet
{
    [TestClass]
    public static class Test
    {
        [TestMethod]
        public static void TestSign()
        {
            // 1,234.567891 is printed as +1,234.567891". (Notice the plus sign)
            // -1,234.567891 is printed as -1,234.567891".
            // 0 is printed as 0.
            foreach (var x in new[] { 1234.567891, -1234.567891, 0 })
            {
                Console.WriteLine(x.ToString("+#,##0.######;-#,##0.######;0"));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public static void TestDouble()
        {
            // 1,234.567891 is printed as 1,234.567891.
            // -1,234.567891 is printed as -1,234.567891.
            // 0 is printed as 0.
            foreach (var x in new[] { 1234.567891, -1234.567891, 0 })
            {
                Console.WriteLine(x.ToString("#,##0.######"));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public static void TestRange()
        {
            Console.WriteLine(Enumerable.Range(5, 3).ToList()); // { 5, 6, 7 }
            Console.WriteLine(Enumerable.Repeat(7, 3).ToList()); // { 7, 7, 7 }
        }

        [TestMethod]
        public static void TestRadix()
        {
            // Radix conversion
            Console.WriteLine(Convert.ToString(255, 2)); // "11111111"
            Console.WriteLine(Convert.ToString(255, 8)); // "377"
            Console.WriteLine(Convert.ToString(255, 16)); // "ff"
            Console.WriteLine(Convert.ToInt32("11111111", 2)); // 255
            Console.WriteLine(Convert.ToInt32("377", 8)); // 255
            Console.WriteLine(Convert.ToInt32("ff", 16)); // 255
        }
    }
}