using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BreedConfiguration : EntityTypeConfiguration<Breed>
    {
        public BreedConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Species).HasMaxLength(20).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            Property(x => x.BreedName).HasMaxLength(50);
        }
    }
}