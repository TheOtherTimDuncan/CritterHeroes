using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts
{
    public interface IStateManager<T>
    {
        T GetContext();
        void SaveContext(T Context);
        void ClearContext();
    }
}
