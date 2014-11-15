using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Email
{
    public interface IEmailConfiguration
    {
        string DefaultFrom
        {
            get;
        }

        string Username
        {
            get;
        }

        string Password
        {
            get;
        }
    }
}
