using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class AnimalStatusConfiguration : EntityTypeConfiguration<AnimalStatus>
    {
        public AnimalStatusConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasMaxLength(25).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            })); ;
            Property(x => x.Description).HasMaxLength(100);
        }
    }
}