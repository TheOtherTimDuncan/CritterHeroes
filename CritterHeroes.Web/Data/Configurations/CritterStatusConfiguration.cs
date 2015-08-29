using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class CritterStatusConfiguration : EntityTypeConfiguration<CritterStatus>
    {
        public CritterStatusConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(25).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()
            {
                IsUnique = true
            }));
            Property(x => x.Description).HasMaxLength(100);
            Property(x => x.RescueGroupsID).HasMaxLength(6).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
        }
    }
}
