using System;
using System.Collections.Generic;
using System.Web;

namespace CritterHeroes.Web.Areas.Admin.Organizations.Models
{
    public class EditProfileModel
    {
        public string Name
        {
            get;
            set;
        }

        public string ShortName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string LogoUrl
        {
            get;
            set;
        }

        public HttpPostedFileBase LogoFile
        {
            get;
            set;
        }
    }
}
