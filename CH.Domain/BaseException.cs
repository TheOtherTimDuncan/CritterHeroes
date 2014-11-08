using System;
using System.Collections.Generic;

namespace CH.Domain
{
    public class BaseException : Exception
    {
        public BaseException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
