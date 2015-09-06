using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BusinessGroupConfiguration : EntityTypeConfiguration<BusinessGroup>
    {
        public BusinessGroupConfiguration()
        {
            HasKey(x => new
            {
                x.BusinessID,
                x.GroupID
            });

            HasRequired(x => x.Group).WithMany().WillCascadeOnDelete();
        }
    }
}
