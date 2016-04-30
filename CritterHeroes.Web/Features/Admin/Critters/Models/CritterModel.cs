using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common;

namespace CritterHeroes.Web.Features.Admin.Critters.Models
{
    public class CritterModel
    {
        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string Breed
        {
            get;
            set;
        }

        public string Sex
        {
            get;
            set;
        }

        public string SiteID
        {
            get;
            set;
        }

        public string SexName
        {
            get
            {
                return SexHelper.GetSexName(Sex);
            }
        }

        public string FosterName
        {
            get;
            set;
        }

        public string PictureFilename
        {
            get;
            set;
        }
    }
}
