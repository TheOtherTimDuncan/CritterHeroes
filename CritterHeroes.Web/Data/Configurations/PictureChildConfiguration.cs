using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class PictureChildConfiguration : EntityTypeConfiguration<PictureChild>
    {
        public PictureChildConfiguration()
        {
            // Include ParentID in the primary key so EF knows to delete this entity when removed from parent
            HasKey(x => new
            {
                x.ID,
                x.ParentID
            });

            Property(x => x.ID).IsRequired().IsIdentity();

            Property(x => x.Filename).IsRequired().IsUnicode(false).HasMaxLength(256);
        }
    }
}
