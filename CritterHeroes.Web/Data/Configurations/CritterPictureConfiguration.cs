using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class CritterPictureConfiguration : EntityTypeConfiguration<CritterPicture>
    {
        public CritterPictureConfiguration()
        {
            HasKey(x => new
            {
                x.CritterID,
                x.PictureID
            });

            HasRequired(x => x.Critter).WithMany(x => x.Pictures).WillCascadeOnDelete();
            HasRequired(x => x.Picture).WithMany().WillCascadeOnDelete();
        }
    }
}
