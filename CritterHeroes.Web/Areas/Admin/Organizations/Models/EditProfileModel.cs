using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Admin.Organizations.Models
{
    public class EditProfileModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

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
    }
}