using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
    }
}
