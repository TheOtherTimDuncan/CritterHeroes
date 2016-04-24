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
            Property(x => x.RescueGroupsID).HasIndex();
        }
    }
}
