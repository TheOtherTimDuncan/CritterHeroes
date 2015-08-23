using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class CritterConfiguration : EntityTypeConfiguration<Critter>
    {
        public CritterConfiguration()
        {
            HasKey(x => x.ID);

            HasRequired(x => x.Breed).WithMany().WillCascadeOnDelete(false);
            HasRequired(x => x.Status).WithMany().WillCascadeOnDelete(false);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            Property(x => x.Sex).IsRequired().HasMaxLength(10);
        }
    }
}
