using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Configurations
{
    public class OrganizationSupportedCritterConfiguration : EntityTypeConfiguration<OrganizationSupportedCritter>
    {
        public OrganizationSupportedCritterConfiguration()
        {
            HasKey(x => x.ID);

            HasRequired(x => x.Species).WithMany(x => x.OrganizationSupportedCritters).WillCascadeOnDelete(false);

            Property(x => x.ID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.OrganizationID)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, new IndexAnnotation(
                        new IndexAttribute("OrganizationSpecies", order: 1) { IsUnique = true }
                    )
                );

            Property(x => x.SpeciesID)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, new IndexAnnotation(
                        new IndexAttribute("OrganizationSpecies", order: 2) { IsUnique = true }
                    )
                );
        }
    }
}
