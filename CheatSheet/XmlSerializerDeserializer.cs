using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CheatSheet
{
    static class XmlSerializerDeserializer<T>
    {
        static readonly XmlSerializer Xs = new XmlSerializer(typeof(T));
        static readonly XmlSerializerNamespaces Xsn = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

        static readonly XmlWriterSettings Xws = new XmlWriterSettings
        {
            // Replace the default encoding "UTF8 with BOM" with "UTF8 without BOM".
            Encoding = new UTF8Encoding(false),
            Indent = true,
            OmitXmlDeclaration = true
        };

        internal static T ReadXml(string path)
        {
            using (var sr = new StreamReader(path))
            {
                return (T)Xs.Deserialize(sr);
            }
        }

        internal static void WriteXml(string path, object o)
        {
            using (var xw = XmlWriter.Create(path, Xws))
            {
                Xs.Serialize(xw, o, Xsn);
            }
        }
    }
}