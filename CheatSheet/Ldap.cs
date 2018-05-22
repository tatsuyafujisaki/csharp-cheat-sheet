using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace CheatSheet
{
    static class Ldap
    {
        static Dictionary<string, object> GetProperties(string path)
        {
            using (var de = new DirectoryEntry(path))
            {
                return de.Properties.Cast<PropertyValueCollection>().ToDictionary(pvc => pvc.PropertyName, pvc => pvc.Value);
            }
        }

        static T GetProperty<T>(string path, string propertyName)
        {
            using (var de = new DirectoryEntry(path))
            {
                return (T)de.Properties[propertyName].Value;
            }
        }
    }
}