using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common
{
    public class BaseException : Exception
    {
        public BaseException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
