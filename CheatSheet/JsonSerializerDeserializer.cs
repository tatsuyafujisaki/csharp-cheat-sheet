using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CheatSheet
{
    static class JsonSerializerDeserializer
    {
        static readonly JavaScriptSerializer Jss = new JavaScriptSerializer();

        // Example of parameter: new  { Foo = "Foo", Bar = "Bar" }
        internal static string Serialize(object o) => Jss.Serialize(o);

        internal static Dictionary<string, object> Deserialize(string json) => Jss.Deserialize<Dictionary<string, object>>(json);
    }
}