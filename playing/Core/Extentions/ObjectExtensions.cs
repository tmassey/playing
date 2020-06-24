using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace playing.Core.Extentions
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in value.GetType().GetTypeInfo().DeclaredProperties)
            {
                expando.Add(property.Name, property.GetValue(value));
            }

            return (ExpandoObject)expando;
        }
    }
}