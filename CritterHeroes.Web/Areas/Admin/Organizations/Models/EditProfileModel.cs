using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Admin.Organizations.Models
{
    public class EditProfileModel
    {
        [Placeholder("Name")]
        public string Name
        {
            get;
            set;
        }

        [Placeholder("Abbreviated Name")]
        public string ShortName
        {
            get;
            set;
        }

        [DataType( DataType.EmailAddress)]
        [Placeholder("Email")]
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