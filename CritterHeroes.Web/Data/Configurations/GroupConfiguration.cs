using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            HasKey(x => x.ID);

            HasMany(x => x.Persons).WithRequired(x => x.Group).WillCascadeOnDelete();

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.Name).IsRequired().HasMaxLength(100);
        }
    }
}
