using System;

namespace CritterHeroes.Web.Contracts.Queries
{
    public interface IQuery<out T>
    {
    }

    public interface IAsyncQuery<out T>
    {
    }
}
