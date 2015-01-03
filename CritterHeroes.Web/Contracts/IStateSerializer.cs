using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts
{
    public interface IStateSerializer
    {
        string Serialize(object data);
        T Deserialize<T>(string data);
    }
}
