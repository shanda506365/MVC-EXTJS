
namespace USO.Domain.Extensions
{
    using System;
    using System.Web.Script.Serialization;
    using System.Text;
    using System.Runtime.Serialization.Json;

    public static class JsonExtensions
    {
        private static readonly Lazy<JavaScriptSerializer> _serializer = new Lazy<JavaScriptSerializer>();

        public static object ToObject(this string json, Type type)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            var obj = _serializer.Value.Deserialize(json, type);
            return obj;
        }

        public static T ToObject<T>(this string json)
        {

            return string.IsNullOrWhiteSpace(json) ? default(T) : (T)_serializer.Value.Deserialize(json, typeof(T));
        }

        public static string ToJson<T>(this T obj)
        {
            return obj == null ? string.Empty : _serializer.Value.Serialize(obj);
        }

        public static T JsonToEntity<T>(this string entityString)
        {
            if (string.IsNullOrWhiteSpace(entityString))
                return default(T);

            using (var ms = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(entityString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        public static string EntityToJson(this object entity)
        {
            if (entity != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(entity.GetType());
                    serializer.WriteObject(ms, entity);
                    ms.Position = 0;
                    using (var reader = new System.IO.StreamReader(ms))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return null;
        }
    }
}
