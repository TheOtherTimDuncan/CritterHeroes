using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BaseContactConfiguration<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : BaseContact
    {
        public BaseContactConfiguration()
        {
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.City).HasMaxLength(100);
            Property(x => x.State).HasMaxLength(2).IsUnicode(false);
            Property(x => x.Address).HasMaxLength(100);
            Property(x => x.Zip).HasMaxLength(10).IsUnicode(false);
            Property(x => x.RescueGroupsID).HasIndex();
        }
    }
}
