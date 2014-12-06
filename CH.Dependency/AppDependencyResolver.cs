using System;
using CH.Domain.Contracts;

namespace CH.Dependency
{
    public class AppDependencyResolver : IAppDependencyResolver
    {
        public T Resolve<T>()
        {
            return DependencyContainer.Using<T>();
        }
    }
}
