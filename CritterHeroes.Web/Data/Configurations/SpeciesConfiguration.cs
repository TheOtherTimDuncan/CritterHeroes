using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class SpeciesConfiguration : EntityTypeConfiguration<Species>
    {
        public SpeciesConfiguration()
        {
            HasKey(x => x.ID);

            HasMany(x => x.Breeds).WithRequired(x => x.Species).WillCascadeOnDelete();

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(50).IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(x => x.Singular).HasMaxLength(50);
            Property(x => x.Plural).HasMaxLength(50);
            Property(x => x.YoungSingular).HasMaxLength(50);
            Property(x => x.YoungPlural).HasMaxLength(50);
        }
    }
}