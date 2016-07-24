using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class BusinessConfiguration : BaseContactConfiguration<Business>
    {
        public BusinessConfiguration()
        {
            HasMany(x => x.Groups).WithRequired(x => x.Business).WillCascadeOnDelete();
            HasMany(x => x.PhoneNumbers).WithRequired(x => x.Business).WillCascadeOnDelete();

            Property(x => x.Name).HasMaxLength(100);
            Property(x => x.Email).HasMaxLength(256);
        }
    }
}
