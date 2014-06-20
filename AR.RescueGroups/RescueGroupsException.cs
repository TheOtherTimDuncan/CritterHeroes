using System;
using AR.Domain;

namespace AR.RescueGroups
{
    public class RescueGroupsException : BaseException
    {
        public RescueGroupsException(string message)
            : base(message)
        {
        }

        public RescueGroupsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RescueGroupsException(string format, params object[] args)
            : base(format, args)
        {
        }
    }
}
