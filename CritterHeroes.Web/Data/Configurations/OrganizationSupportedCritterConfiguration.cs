using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class OrganizationSupportedCritterConfiguration : EntityTypeConfiguration<OrganizationSupportedCritter>
    {
        public OrganizationSupportedCritterConfiguration()
        {
            HasKey(x => x.ID);

            HasRequired(x => x.Species).WithMany(x => x.OrganizationSupportedCritters).WillCascadeOnDelete(false);

            Property(x => x.ID).IsRequired().IsIdentity();
            Property(x => x.OrganizationID).IsRequired().HasUniqueIndex("IX_OrganizationSpecies", 1);
            Property(x => x.SpeciesID).IsRequired().HasUniqueIndex("IX_OrganizationSpecies", 2);
        }
    }
}
