using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class StateConfiguration : EntityTypeConfiguration<State>
    {
        public StateConfiguration()
        {
            HasKey(x => x.Abbreviation);

            Property(x => x.Abbreviation).IsRequired().HasMaxLength(2).IsUnicode(false);
            Property(x => x.Name).IsRequired().HasMaxLength(14).IsUnicode(false);
        }
    }
}
