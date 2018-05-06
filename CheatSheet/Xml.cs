using System;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CheatSheet
{
    static class Xml
    {
        static bool Exist(XNode xn, string xpath) => xn.XPathSelectElement(xpath) != null;
        static string Get(XNode xn, string xpath) => xn.XPathSelectElement(xpath)?.Value;
        static string GetParent(string xpath) => xpath.Substring(0, xpath.LastIndexOf('/'));
        static string GetBottom(string xpath) => xpath.Substring(xpath.LastIndexOf('/') + 1);

        static bool? GetBool(XNode xn, string xpath)
        {
            var xe = xn.XPathSelectElement(xpath);

            if (xe == null)
            {
                return null;
            }

            return bool.TryParse(xe.Value, out var result) ? result : (bool?)null;
        }

        static int? GetInt(XNode xn, string xpath)
        {
            var xe = xn.XPathSelectElement(xpath);

            if (xe == null)
            {
                return null;
            }

            return int.TryParse(xe.Value, out var result) ? result : (int?)null;
        }

        static decimal? GetDecimal(XNode n, string xpath)
        {
            var xe = n.XPathSelectElement(xpath);

            if (xe == null)
            {
                return null;
            }

            return decimal.TryParse(xe.Value, out var result) ? result : (decimal?)null;
        }

        static double? GetDouble(XNode n, string xpath)
        {
            var xe = n.XPathSelectElement(xpath);

            if (xe == null)
            {
                return null;
            }

            return double.TryParse(xe.Value, out var result) ? result : (double?)null;
        }

        static DateTime? GetDateTime(XNode xn, string xpath, string format)
        {
            var xe = xn.XPathSelectElement(xpath);

            if (xe == null)
            {
                return null;
            }

            return DateTime.TryParseExact(xe.Value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ? result : (DateTime?)null;
        }

        static void Merge(XNode xn, string xpath, object value = null)
        {
            var parentXPath = GetParent(xpath);
            var xe = xn.XPathSelectElement(parentXPath);

            // If parentXPath dose not exist, create it.
            if (xe == null)
            {
                Merge(xn, parentXPath);
                xe = xn.XPathSelectElement(parentXPath);
            }

            var name = GetBottom(xpath);

            if (value == null)
            {
                // Create </Name>
                xe.Add(new XElement(name, null));

                // The following don't create an empty element.
                // xe.SetElementValue(name, ""); ... Creates <Name></Name>.
                // xe.SetElementValue(name, null); ... Removes the element.
            }
            else
            {
                xe.SetElementValue(name, value);
            }
        }

        static void Remove(XNode xn, string xpath)
        {
            var xe = xn.XPathSelectElement(xpath);

            if (xe != null)
            {
                xe.Remove();
            }
        }
    }
}
