using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts.StateManagement
{
    public interface IStateManager<T>
    {
        T GetContext();
        void SaveContext(T Context);
        void ClearContext();
    }
}
