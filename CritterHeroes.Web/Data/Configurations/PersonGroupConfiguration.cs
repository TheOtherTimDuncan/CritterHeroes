using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class PersonGroupConfiguration : EntityTypeConfiguration<PersonGroup>
    {
        public PersonGroupConfiguration()
        {
            HasKey(x => x.ID);
            Property(x => x.PersonID).HasUniqueIndex("IX_PersonGroup", 1);
            Property(x => x.GroupID).HasUniqueIndex("IX_PersonGroup", 2);
        }
    }
}
