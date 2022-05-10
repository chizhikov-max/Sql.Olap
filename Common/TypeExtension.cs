using System;
using System.Reflection;

namespace Sql.Olap.Common
{
    public static class TypeExtension
    {
        public static bool InterfaceFilter(Type type, object filterCriteria)
        {
            var filterInterface = filterCriteria as Type;
            return type.IsAssignableFrom(filterInterface);
        }

        public static bool TypeIsInterface(this Type type, Type interfaceType)
        {
            var typeFilter = new TypeFilter(InterfaceFilter);
            return type.FindInterfaces(typeFilter, interfaceType).Length > 0;
        }
    }
}