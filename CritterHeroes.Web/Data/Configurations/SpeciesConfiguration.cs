using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class SpeciesConfiguration : EntityTypeConfiguration<Species>
    {
        public SpeciesConfiguration()
        {
            HasKey(x => x.ID);

            HasMany(x => x.Breeds).WithRequired(x => x.Species).WillCascadeOnDelete();

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.Name).HasMaxLength(50).IsRequired().HasUniqueIndex();
            Property(x => x.Singular).HasMaxLength(50);
            Property(x => x.Plural).HasMaxLength(50);
            Property(x => x.YoungSingular).HasMaxLength(50);
            Property(x => x.YoungPlural).HasMaxLength(50);
        }
    }
}
