using System;

namespace CH.Domain.Contracts
{
    public interface IAppDependencyResolver
    {
        T Resolve<T>();
    }
}
