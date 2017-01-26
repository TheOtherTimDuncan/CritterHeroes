using System;

namespace CritterHeroes.Web.Domain.Contracts.Queries
{
    public interface IQuery<out T>
    {
    }

    public interface IAsyncQuery<out T>
    {
    }
}
