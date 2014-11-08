using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CH.Azure.Storage
{
    public class StorageEntityFactory
    {
        private static IEnumerable<Type> _mappings = null;

        private static IEnumerable<Type> Mappings
        {
            get
            {
                if (_mappings == null)
                {
                    Type mappingType = typeof(StorageEntity<>);
                    _mappings = Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(t =>
                            t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == mappingType
                        )
                        .ToList();
                }
                return _mappings;
            }
        }

        public static StorageEntity<T> GetStorageEntity<T>() where T : class
        {
            Type implementationType = typeof(StorageEntity<T>);
            Type mappingType = Mappings.FirstOrDefault(x => implementationType.IsAssignableFrom(x));

            if (mappingType == null)
            {
                throw new AzureException("Azure storage entity not found for {0}", typeof(T));
            }

            StorageEntity<T> result = Activator.CreateInstance(mappingType) as StorageEntity<T>;
            if (result == null)
            {
                throw new AzureException("Unable to create instance of {0}", mappingType);
            }

            return result;
        }
    }
}
