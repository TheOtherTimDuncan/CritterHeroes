using System;
using System.Collections.Generic;
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

            HasRequired(x => x.Breed).WithMany(x => x.Critters).HasForeignKey(x => x.BreedID).WillCascadeOnDelete(false);
            HasRequired(x => x.Status).WithMany(x => x.Critters).HasForeignKey(x => x.StatusID).WillCascadeOnDelete(false);
            HasRequired(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationID).WillCascadeOnDelete(false);
            HasOptional(x => x.Location).WithMany().HasForeignKey(x => x.LocationID).WillCascadeOnDelete(false);
            HasOptional(x => x.Color).WithMany().HasForeignKey(x => x.ColorID).WillCascadeOnDelete(false);

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasIndex();
            Property(x => x.Sex).IsRequired().HasMaxLength(10);
            Property(x => x.RescueID).HasMaxLength(100).IsUnicode(false);
            Property(x => x.Description).IsMaxLength();
            Property(x => x.SpecialNeedsDescription).IsMaxLength();
            Property(x => x.GeneralAge).IsUnicode(false).HasMaxLength(10);
            Property(x => x.BirthDate).IsDateOnly();
            Property(x => x.EuthanasiaDate).IsDateOnly();
            Property(x => x.EuthanasiaReason).HasMaxLength(15);
        }
    }
}
