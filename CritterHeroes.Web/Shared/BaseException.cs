using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared
{
    public class BaseException : Exception
    {
        public BaseException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
