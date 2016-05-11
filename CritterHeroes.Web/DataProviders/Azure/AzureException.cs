using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared;

namespace CritterHeroes.Web.DataProviders.Azure
{
    public class AzureException : BaseException
    {
        public AzureException(string message)
            : base(message)
        {
        }

        public AzureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AzureException(string format, params object[] args)
            : base(format, args)
        {
        }
    }
}
