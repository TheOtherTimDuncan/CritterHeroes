using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BusinessConfiguration : EntityTypeConfiguration<Business>
    {
        public BusinessConfiguration()
        {
            HasKey(x => x.ID);

            HasMany(x => x.Groups).WithRequired(x => x.Business).WillCascadeOnDelete();
            HasMany(x => x.PhoneNumbers).WithRequired(x => x.Business).WillCascadeOnDelete();

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.Name).HasMaxLength(100);
            Property(x => x.City).HasMaxLength(100);
            Property(x => x.State).HasMaxLength(2).IsUnicode(false);
            Property(x => x.Email).HasMaxLength(256);
            Property(x => x.Address).HasMaxLength(100);
            Property(x => x.Zip).HasMaxLength(10).IsUnicode(false);
            Property(x => x.RescueGroupsID).HasIndex();
        }
    }
}
