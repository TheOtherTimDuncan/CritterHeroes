using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Configuration
{
    public interface IRescueGroupsConfiguration
    {
        string Url
        {
            get;
        }

        string APIKey
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

        string AccountNumber
        {
            get;
        }
    }
}
