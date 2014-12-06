using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts
{
    public interface IStateSerializer
    {
        string Serialize(object data);
        T Deserialize<T>(string data);
    }
}
