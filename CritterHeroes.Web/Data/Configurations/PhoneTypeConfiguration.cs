using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class PhoneTypeConfiguration : EntityTypeConfiguration<PhoneType>
    {
        public PhoneTypeConfiguration()
        {
            HasKey(x => x.ID);
            Property(x => x.Name).IsRequired().HasMaxLength(10);
        }
    }
}
