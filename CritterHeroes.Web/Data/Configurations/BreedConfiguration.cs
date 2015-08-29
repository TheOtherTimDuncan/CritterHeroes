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

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SpeciesID).IsRequired();
            Property(x => x.BreedName).IsRequired().HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); // BreedName is not unique
            Property(x => x.RescueGroupsID).HasMaxLength(6).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
        }
    }
}
