using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace CheatSheet
{
    static class Ldap
    {
        static Dictionary<string, string> GetAdProperties(string id)
        {
            using (var de = new DirectoryEntry($"WinNT://{Environment.UserDomainName}/{id}"))
            {
                return de.Properties
                         .Cast<PropertyValueCollection>()
                         .ToDictionary(p => p.PropertyName, p => p.Value.ToString());
            }
        }

        static string GetProperty(string id, string propertyName)
        {
            using (var de = new DirectoryEntry($"WinNT://{Environment.UserDomainName}/{id}"))
            {
                return de.Properties[propertyName].Value.ToString();
            }
        }

        // Example 1: GetProperty("name", "displayName", displayName);
        // Example 2: GetProperty("name", "mail", mail);
        static string GetProperty(string propertyName, string attributeName, string attributeValue)
        {
            using (var de = new DirectoryEntry())
            {
                using (var ds = new DirectorySearcher(de))
                {
                    ds.Filter = $"(&(objectClass=user)({attributeName}={attributeValue}))";
                    var sr = ds.FindOne();

                    if (sr == null)
                    {
                        return null;
                    }

                    using (var de2 = sr.GetDirectoryEntry())
                    {
                        return de2.Properties[propertyName].Value.ToString();
                    }
                }
            }
        }

        // Example: GetProperty("name", "mail", mail, "displayName", displayName);
        static string GetProperty(string propertyName, string attributeName, string attributeValue1, string attributeName2, string attributeValue2)
        {
            using (var de = new DirectoryEntry())
            {
                using (var ds = new DirectorySearcher(de))
                {
                    ds.Filter = $"(&(objectClass=user)(|({attributeName}={attributeValue1})({attributeName2}={attributeValue2})))";
                    var sr = ds.FindOne();

                    if (sr == null)
                    {
                        return null;
                    }

                    using (var de2 = sr.GetDirectoryEntry())
                    {
                        return de2.Properties[propertyName].Value.ToString();
                    }
                }
            }
        }

        static Dictionary<string, string> GetProperties(string attributeName, string attributeValue)
        {
            using (var de = new DirectoryEntry())
            {
                using (var ds = new DirectorySearcher(de))
                {
                    ds.Filter = $"(&(objectClass=user)({attributeName}={attributeValue}))";
                    var sr = ds.FindOne();

                    if (sr == null)
                    {
                        return null;
                    }

                    using (var de2 = sr.GetDirectoryEntry())
                    {
                        return de2.Properties.Cast<PropertyValueCollection>().ToDictionary(p => p.PropertyName, p => p.Value.ToString());
                    }
                }
            }
        }

        static Dictionary<string, string> GetProperties(string attributeName1, string attributeValue1, string attributeName2, string attributeValue2)
        {
            using (var de = new DirectoryEntry())
            {
                using (var ds = new DirectorySearcher(de))
                {
                    ds.Filter = $"(&(objectClass=user)(|({attributeName1}={attributeValue1})({attributeName2}={attributeValue2})))";
                    var sr = ds.FindOne();

                    if (sr == null)
                    {
                        return null;
                    }

                    using (var de2 = sr.GetDirectoryEntry())
                    {
                        return de2.Properties.Cast<PropertyValueCollection>().ToDictionary(p => p.PropertyName, p => p.Value.ToString());
                    }
                }
            }
        }
    }
}