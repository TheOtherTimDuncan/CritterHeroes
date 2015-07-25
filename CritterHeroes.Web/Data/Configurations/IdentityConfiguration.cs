using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Data.Configurations
{
    public class IdentityConfiguration
    {
        public static void ConfigureIdentity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRole>().ToTable("AppRole");
            modelBuilder.Entity<AppUserClaim>().ToTable("AppUserClaim");
            modelBuilder.Entity<AppUserLogin>().ToTable("AppUserLogin");
            modelBuilder.Entity<AppUserRole>().ToTable("AppUserRole");

            EntityTypeConfiguration<AppUser> userConfiguration = modelBuilder.Entity<AppUser>();
            userConfiguration.ToTable("AppUser");

            userConfiguration.Property(x => x.PreviousEmail).HasMaxLength(256);
            userConfiguration.Property(x => x.FirstName).HasMaxLength(50);
            userConfiguration.Property(x => x.LastName).HasMaxLength(50);
        }
    }
}