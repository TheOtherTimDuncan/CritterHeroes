using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.StateManagement
{
    public interface IStateSerializer
    {
        string Serialize(object data);
        T Deserialize<T>(string data);
    }
}
