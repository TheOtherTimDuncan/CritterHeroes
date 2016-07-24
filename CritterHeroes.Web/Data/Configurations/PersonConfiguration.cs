using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Data.Configurations
{
    public class PersonConfiguration : BaseContactConfiguration<Person>
    {
        public PersonConfiguration()
        {
            HasMany(x => x.Groups).WithRequired(x => x.Person).WillCascadeOnDelete();
            HasMany(x => x.PhoneNumbers).WithRequired(x => x.Person).WillCascadeOnDelete();
            HasMany(x => x.Critters).WithOptional(x => x.Foster).WillCascadeOnDelete(false);

            Property(x => x.FirstName).HasMaxLength(100);
            Property(x => x.LastName).HasMaxLength(100);
            Property(x => x.Email).HasMaxLength(256);
            Property(x => x.NewEmail).HasMaxLength(256);
        }
    }
}
