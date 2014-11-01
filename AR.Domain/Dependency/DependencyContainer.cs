using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace AR.Domain.Dependency
{
    public class DependencyContainer
    {
        private static IKernel _kernel;

        public static T Using<T>()
        {
            return Kernel.Get<T>();
        }

        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    _kernel = GetStandardKernel();
                }
                return _kernel;
            }
        }

        private static IKernel GetStandardKernel()
        {
            return new StandardKernel
            (
                new DependencyModule()
            );
        }
    }
}
