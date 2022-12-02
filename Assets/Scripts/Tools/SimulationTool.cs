
using System.Collections;
using System.Linq;

namespace Simulation.Tool
{
    public static class EnumerableExtension
    {
        public static T Find<T>(this IEnumerable e)
        {
            return e.OfType<T>().FirstOrDefault();
        }
        public static bool Has<T>(this IEnumerable e)
        {
            return e.OfType<T>().Count() > 0;
        }
    }
}