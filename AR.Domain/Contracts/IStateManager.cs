using System;
using System.Collections.Generic;

namespace AR.Domain.Contracts
{
    public interface IStateManager<T>
    {
        T GetContext();
        void SaveContext(T Context);
        void ClearContext();
    }
}
