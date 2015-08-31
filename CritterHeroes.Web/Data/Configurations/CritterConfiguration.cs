using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class CritterConfiguration : EntityTypeConfiguration<Critter>
    {
        public CritterConfiguration()
        {
            HasKey(x => x.ID);

            HasRequired(x => x.Breed).WithMany(x => x.Critters).WillCascadeOnDelete(false);
            HasRequired(x => x.Status).WithMany(x => x.Critters).WillCascadeOnDelete(false);
            HasRequired(x => x.Organization).WithMany().WillCascadeOnDelete(false);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasIndex();
            Property(x => x.Sex).IsRequired().HasMaxLength(10);
        }
    }
}
