﻿using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts.Email
{
    public interface IEmailConfiguration
    {
        string DefaultFrom
        {
            get;
        }
    }
}
