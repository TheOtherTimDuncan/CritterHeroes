using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class OrganizationConfiguration : EntityTypeConfiguration<Organization>
    {
        public OrganizationConfiguration()
        {
            HasKey(x => x.ID);

            HasMany(x => x.SupportedCritters).WithRequired().HasForeignKey(x => x.OrganizationID).WillCascadeOnDelete(true);

            Property(x => x.ID).IsRequired();
            Property(x => x.FullName).HasMaxLength(100).IsRequired();
            Property(x => x.ShortName).HasMaxLength(50);
            Property(x => x.AzureName).HasMaxLength(25).IsRequired().IsUnicode(false);
            Property(x => x.LogoFilename).HasMaxLength(255).IsUnicode(false);
            Property(x => x.EmailAddress).HasMaxLength(255).IsRequired();
            Property(x => x.TimeZoneID).HasMaxLength(50).IsUnicode(false);
        }
    }
}
