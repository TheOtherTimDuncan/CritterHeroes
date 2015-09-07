using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class PictureConfiguration : EntityTypeConfiguration<Picture>
    {
        public PictureConfiguration()
        {
            HasKey(x => x.ID);

            HasMany(x => x.ChildPictures).WithRequired(x => x.Parent).WillCascadeOnDelete();

            Property(x => x.ID).IsRequired().IsIdentity();

            Property(x => x.Filename).IsRequired().IsUnicode(false).HasMaxLength(256);
            Property(x => x.ContentType).IsRequired().IsUnicode(false).HasMaxLength(256);
            Property(x => x.RescueGroupsID).HasMaxLength(8).IsUnicode(false).HasIndex();
        }
    }
}
