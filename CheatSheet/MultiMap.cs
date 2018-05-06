using System.Collections.Generic;
using System.Linq;

namespace CheatSheet
{
    // Each key can have multiple values.
    sealed class MultiMap<T1, T2>
    {
        readonly Dictionary<T1, HashSet<T2>> _p = new Dictionary<T1, HashSet<T2>>();

        // Usage: paths.Keys
        Dictionary<T1, HashSet<T2>>.KeyCollection Keys => _p.Keys;

        // Usage: paths.Values
        Dictionary<T1, HashSet<T2>>.ValueCollection Values => _p.Values;

        // Usage: paths[key]
        HashSet<T2> this[T1 key] => _p[key];

        internal bool ContainsKey(T1 key) => _p.ContainsKey(key);

        internal bool ContainsValue(T2 value) => _p.Values.SelectMany(x => x).Contains(value);

        internal bool Contains(T1 key, T2 value) => _p.ContainsKey(key) && _p[key].Contains(value);

        internal void Add(T1 key, T2 value)
        {
            if (_p.ContainsKey(key))
            {
                _p[key].Add(value);
            }
            else
            {
                _p.Add(key, new HashSet<T2> { value });
            }
        }

        internal void Delete(T1 key, T2 value)
        {
            if (_p.ContainsKey(key))
            {
                // No error when the key does not have the value.
                _p[key].Remove(value);
            }
        }
    }
}