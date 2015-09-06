using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BusinessPhoneConfiguration : EntityTypeConfiguration<BusinessPhone>
    {
        public BusinessPhoneConfiguration()
        {
            // Include PersonID in the primary key so EF knows to delete this entity when removed from parent
            HasKey(x => new
            {
                x.ID,
                x.BusinessID
            });

            HasRequired(x => x.PhoneType).WithMany().WillCascadeOnDelete(false);

            Property(x => x.ID).IsIdentity();
            Property(x => x.PhoneNumber).IsRequired().IsUnicode(false).HasMaxLength(10);
            Property(x => x.PhoneExtension).IsUnicode(false).HasMaxLength(6);
        }
    }
}
