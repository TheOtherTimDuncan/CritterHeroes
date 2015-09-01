using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class CritterConfiguration : EntityTypeConfiguration<Critter>
    {
        public CritterConfiguration()
        {
            HasKey(x => x.ID);

            HasRequired(x => x.Breed).WithMany(x => x.Critters).WillCascadeOnDelete(false);
            HasRequired(x => x.Status).WithMany(x => x.Critters).WillCascadeOnDelete(false);
            HasRequired(x => x.Organization).WithMany().WillCascadeOnDelete(false);

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasIndex();
            Property(x => x.Sex).IsRequired().HasMaxLength(10);
            Property(x => x.RescueID).HasMaxLength(100).IsUnicode(false);
        }
    }
}
