using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain;

namespace AR.Azure
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
