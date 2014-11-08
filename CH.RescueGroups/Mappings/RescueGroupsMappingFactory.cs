using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models;

namespace CH.RescueGroups.Mappings
{
    public static class RescueGroupsMappingFactory
    {
        private static IEnumerable<Type> _mappings = null;

        private static IEnumerable<Type> Mappings
        {
            get
            {
                if (_mappings == null)
                {
                    Type mappingType = typeof(IRescueGroupsMapping<>);
                    _mappings = Assembly.GetExecutingAssembly().GetTypes()
                        .Where
                        (
                                    t => !t.IsAbstract
                                && !t.IsInterface
                                && t.GetInterfaces().Any(g => g.IsGenericType && g.GetGenericTypeDefinition() == mappingType)
                        ).ToList();
                }
                return _mappings;
            }
        }

        public static IRescueGroupsMapping<T> GetMapping<T>() where T : class
        {
            Type implementationType = typeof(IRescueGroupsMapping<T>);
            Type mappingType = Mappings.FirstOrDefault(x => implementationType.IsAssignableFrom(x));

            if (mappingType == null)
            {
                throw new RescueGroupsException("RescueGroups mapping not found for {0}", typeof(T));
            }

            IRescueGroupsMapping<T> result = Activator.CreateInstance(mappingType) as IRescueGroupsMapping<T>;
            if (result == null)
            {
                throw new RescueGroupsException("Unable to create instance of {0}", mappingType);
            }

            return result;
        }
    }
}
