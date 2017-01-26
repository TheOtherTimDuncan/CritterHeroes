using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts.StateManagement
{
    public interface IStateSerializer
    {
        string Serialize(object data);
        T Deserialize<T>(string data);
    }
}
