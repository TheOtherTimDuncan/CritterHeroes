using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class CritterColorConfiguration : EntityTypeConfiguration<CritterColor>
    {
        public CritterColorConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.Description).IsRequired().HasMaxLength(100);
            Property(x => x.RescueGroupsID).HasMaxLength(6).IsUnicode(false).HasIndex();
        }
    }
}