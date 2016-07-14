using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        public LocationConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsIdentity();
            Property(x => x.Name).HasMaxLength(50);
            Property(x => x.Address).HasMaxLength(50);
            Property(x => x.City).HasMaxLength(20);
            Property(x => x.State).IsUnicode(false).HasMaxLength(2);
            Property(x => x.Zip).IsUnicode(false).HasMaxLength(10);
            Property(x => x.Phone).IsUnicode(false).HasMaxLength(14);
            Property(x => x.Website).IsUnicode(false).HasMaxLength(200);
            Property(x => x.RescueGroupsID).HasIndex();
        }
    }
}
