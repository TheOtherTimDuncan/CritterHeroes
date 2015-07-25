using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class SpeciesConfiguration : EntityTypeConfiguration<Species>
    {
        public SpeciesConfiguration()
        {
            HasKey(x => x.Name);

            Property(x => x.Name).HasMaxLength(50).IsRequired();
            Property(x => x.Singular).HasMaxLength(50);
            Property(x => x.Plural).HasMaxLength(50);
            Property(x => x.YoungSingular).HasMaxLength(50);
            Property(x => x.YoungPlural).HasMaxLength(50);
        }
    }
}