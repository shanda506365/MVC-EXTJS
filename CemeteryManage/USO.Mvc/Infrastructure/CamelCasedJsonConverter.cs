
namespace USO.Mvc.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using MvcExtensions;

    public class CamelCasedJsonConverter : JavaScriptConverter
    {
        private static readonly Type scriptIgnoreAttributeType = typeof(ScriptIgnoreAttribute);

        private static readonly Func<IEnumerable<Type>> getSupportedTypes = () => DependencyResolver.Current.GetService<IBuildManager>()
                                                                                                .ConcreteTypes
                                                                                                .Where(type => (type.Name.EndsWith("DTO", StringComparison.OrdinalIgnoreCase) || type.Name.EndsWith("Model", StringComparison.OrdinalIgnoreCase)));

        private static IEnumerable<Type> _supportedTypes;

        public override IEnumerable<Type> SupportedTypes
        {
            [DebuggerStepThrough]
            get
            {
                return _supportedTypes ?? (_supportedTypes = getSupportedTypes().ToList());
            }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Func<string, string> camelCase = name => name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + name.Substring(1);

            IDictionary<string, object> result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (obj != null)
            {
                var type = obj.GetType();

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField)
                                                    .Where(field => !field.IsDefined(scriptIgnoreAttributeType, true));

                foreach (var field in fields)
                {
                    var key = camelCase(field.Name);
                    var value = field.GetValue(obj);

                    result.Add(key, value);
                }

                Func<PropertyInfo, bool> shouldInclude = property => !property.IsDefined(scriptIgnoreAttributeType, true) &&
                                                                     (property.GetGetMethod() != null) &&
                                                                     (property.GetGetMethod().GetParameters().Length == 0);

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                                                           .Where(shouldInclude);

                foreach (var property in properties)
                {
                    var key = camelCase(property.Name);
                    var value = property.GetValue(obj, null);

                    result.Add(key, value);
                }
            }

            return result;
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
