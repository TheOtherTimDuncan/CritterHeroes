using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Queries;

namespace CH.Website.Services.Queries
{
    public class EditProfileQuery : IQuery
    {
        public string Username
        {
            get;
            set;
        }
    }
}