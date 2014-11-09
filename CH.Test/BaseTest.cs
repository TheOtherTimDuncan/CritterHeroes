using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Dependency;

namespace CH.Test
{
    public class BaseTest
    {
        public T Using<T>()
        {
            return DependencyContainer.Using<T>();
        }
    }
}
