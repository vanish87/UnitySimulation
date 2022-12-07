
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Tool
{
    public static class EnumerableExtension
    {
        public static T Find<T>(this IEnumerable e)
        {
            return e.OfType<T>().FirstOrDefault();
        }
        public static T Find<T>(this IEnumerable e, string name) where T : ISerializable
        {
            return e.OfType<T>().Where(s => s.Identifier == name).FirstOrDefault();
        }
        public static IEnumerable<T> FindAll<T>(this IEnumerable e, string name) where T : ISerializable
        {
            return e.OfType<T>().Where(s => s.Identifier == name);
        }
        public static IEnumerable<T> FindAll<T>(this IEnumerable e)
        {
            return e.OfType<T>();
        }
        public static bool Has<T>(this IEnumerable e)
        {
            return e.OfType<T>().Count() > 0;
        }
    }
}