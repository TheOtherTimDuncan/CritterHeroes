using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileModel
    {
        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string UnconfirmedEmail
        {
            get;
            set;
        }
    }
}
