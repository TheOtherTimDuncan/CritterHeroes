using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts
{
    public interface IStateManager<T>
    {
        T GetContext();
        void SaveContext(T Context);
        void ClearContext();
    }
}
