
namespace USO.Domain.Extensions
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml.Serialization;

    public static class XmlExtensions
    {
        public static T ConvertTo<T>(this string xml)
        {
            return (T)ConvertToObject(xml, typeof(T));
        }

        private static object ConvertToObject(string xml, Type objectType)
        {
            object obj = null;
            if (!string.IsNullOrEmpty(xml))
            {
                using (var reader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(objectType);
                    try
                    {
                        obj = serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    reader.Close();
                }
            }
            return obj;
        }

        public static string ConvertToString(this object objectToConvert)
        {
            string xml = null;
            if (objectToConvert != null)
            {
                var serializer = new XmlSerializer(objectToConvert.GetType());
                using (var writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    serializer.Serialize((TextWriter)writer, objectToConvert);
                    xml = writer.ToString();
                    writer.Close();
                }
            }
            return xml;
        }
    }
}
