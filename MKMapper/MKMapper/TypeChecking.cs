using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKMapper
{
    public static class TypeChecking
    {
        public static bool CheckIsList(Type property)
        {
           return property.IsInterface ||
                    (property.IsGenericType &&
                    property.GetGenericTypeDefinition() == typeof(List<>));
        }
        public static bool CheckIsObject(Type property)
        {
            return property.IsClass &&
                        !property.Name.Contains("String");
        }
    }
}
