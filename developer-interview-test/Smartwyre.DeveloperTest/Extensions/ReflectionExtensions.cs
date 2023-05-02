using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest
{
    public static class ReflectionExtensions
    {
        public static List<Type> InheritedInterfaces(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return
                type.Assembly
                    .GetTypes()
                    .Where(t => t.IsInterface && type.IsAssignableFrom(t) && type != t)
                    .ToList();
        }

        public static List<(Type ServiceType, Type ImplementationType)> InterfaceMappings(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            var interfaces = type.InheritedInterfaces();

            return
                type.Assembly
                    .GetTypes()
                    .Where(t => t.IsInterface == false && t.IsAbstract == false && interfaces.Any(i => i.IsAssignableFrom(t)))
                    .Select(t => (interfaces.First(i => i.IsAssignableFrom(t)), t))
                    .ToList();
        }
    }
}