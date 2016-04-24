using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BreedConfiguration : EntityTypeConfiguration<Breed>
    {
        public BreedConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.SpeciesID).IsRequired().HasIndex("SpeciesBreed", 1);
            Property(x => x.BreedName).IsRequired().HasMaxLength(100).HasIndex("SpeciesBreed", 2); // BreedName is not unique
            Property(x => x.RescueGroupsID).HasIndex();
        }
    }
}
