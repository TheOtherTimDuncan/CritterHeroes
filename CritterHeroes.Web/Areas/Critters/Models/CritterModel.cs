using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Areas.Critters.Models
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
