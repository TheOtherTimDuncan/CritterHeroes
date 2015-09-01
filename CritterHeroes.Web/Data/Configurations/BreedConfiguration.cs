using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BreedConfiguration : EntityTypeConfiguration<Breed>
    {
        public BreedConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SpeciesID).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SpeciesBreed", 1)));
            Property(x => x.BreedName).IsRequired().HasMaxLength(100).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SpeciesBreed", 2))); // BreedName is not unique
            Property(x => x.RescueGroupsID).HasMaxLength(6).HasIndex();
        }
    }
}
