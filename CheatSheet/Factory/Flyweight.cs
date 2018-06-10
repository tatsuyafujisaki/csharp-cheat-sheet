using System.Collections.Generic;
using System.Globalization;

namespace CheatSheet.Factory
{
    static class Flyweight
    {
        static Dictionary<string, CultureInfo> D = new Dictionary<string, CultureInfo>();

        internal static CultureInfo GetCultureInfo(string name)
        {
            if (!D.ContainsKey(name))
            {
                D.Add(name, new CultureInfo(name));
            }

            return D[name];
        }
    }
}