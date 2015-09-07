using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class CritterPicture
    {
        protected CritterPicture()
        {
        }

        internal CritterPicture(Critter critter, Picture picture)
        {
            ThrowIf.Argument.IsNull(critter, nameof(critter));
            ThrowIf.Argument.IsNull(picture, nameof(picture));

            this.CritterID = critter.ID;
            this.Critter = critter;

            this.PictureID = picture.ID;
            this.Picture = picture;
        }

        public int CritterID
        {
            get;
            private set;
        }

        public virtual Critter Critter
        {
            get;
            private set;
        }

        public int PictureID
        {
            get;
            private set;
        }

        public virtual Picture Picture
        {
            get;
            private set;
        }
    }
}
