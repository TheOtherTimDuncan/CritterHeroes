namespace CH.DatabaseMigrator.Migrations
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using CritterHeroes.Web.Common.Identity;
    using CritterHeroes.Web.Data.Models;
    using CritterHeroes.Web.Data.Models.Identity;
    using CritterHeroes.Web.Data.Storage;
    using EntityFramework.DatabaseMigrator.Migrations;

    internal sealed class Configuration : BaseMigrationConfiguration<CH.DatabaseMigrator.Migrations.MigrationsDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MigrationsDataContext context)
        {
            SeedStates(context);
            SeedPhoneTypes(context);

            foreach (string role in UserRole.GetAll())
            {
                AppRole appRole = context.Roles.SingleOrDefault(x => x.Name == role);
                if (appRole == null)
                {
                    appRole = new AppRole()
                    {
                        Name = role
                    };
                    context.Roles.Add(appRole);
                    context.SaveChanges();

                    Logger.Verbose("Added role " + role);
                }
            }

            AppUserManager userManager = new AppUserManager(new AppUserStore(context));
            string seedEmail = ConfigurationManager.AppSettings["SeedEmail"];
            string seedPassword = ConfigurationManager.AppSettings["SeedPassword"];

            AppUser appUser = context.Users.SingleOrDefault(x => x.UserName == seedEmail);
            if (appUser == null)
            {
                appUser = new AppUser(seedEmail)
                {
                    FirstName = "Tim",
                    LastName = "Duncan"
                };
                Task.WaitAll(userManager.CreateAsync(appUser, seedPassword));
                Logger.Verbose("Created user for " + appUser.Email);
            }

            if (!userManager.IsInRoleAsync(appUser.Id, UserRole.MasterAdmin).Result)
            {
                Task.WaitAll(userManager.AddToRoleAsync(appUser.Id, UserRole.MasterAdmin));
                Logger.Verbose("Added " + appUser.Email + " to role " + UserRole.MasterAdmin);
            }

            Species dog = context.Species.SingleOrDefault(x => x.Name == "Dog");
            if (dog == null)
            {
                dog = new Species("Dog", "Dog", "Dogs", "Puppy", "Puppies");
                context.Species.Add(dog);
                context.SaveChanges();
                Logger.Verbose("Added Species Dog");
            }

            Species cat = context.Species.SingleOrDefault(x => x.Name == "Cat");
            if (cat == null)
            {
                cat = new Species("Cat", "Cat", "Cats", "Kitten", "Kittens");
                context.Species.Add(cat);
                context.SaveChanges();
                Logger.Verbose("Added Species Cat");
            }

            Guid fflah = Guid.Parse(ConfigurationManager.AppSettings["fflah"]);
            Organization fflahOrg = context.Organizations.SingleOrDefault(x => x.ID == fflah);
            if (fflahOrg == null)
            {
                fflahOrg = new Organization(fflah)
                {
                    FullName = "Friends For Life Animal Haven",
                    ShortName = "FFLAH",
                    RescueGroupsID = 1211,
                    AzureName = "fflah",
                    LogoFilename = "logo.svg",
                    EmailAddress = "email@email.com"
                };
                context.Organizations.Add(fflahOrg);
                context.SaveChanges();
                Logger.Verbose("Added FFLAH organization");
            }

            if (!fflahOrg.SupportedCritters.Any(x => x.Species.Name == dog.Name))
            {
                context.SaveChanges();
                fflahOrg.AddSupportedCritter(dog);
                Logger.Verbose("Added Dog to Supported Critters for FFLAH");
            }

            if (!fflahOrg.SupportedCritters.Any(x => x.Species.Name == cat.Name))
            {
                context.SaveChanges();
                fflahOrg.AddSupportedCritter(cat);
                Logger.Verbose("Added Cat to Supported Critters for FFLAH");
            }
        }

        private void SeedStates(MigrationsDataContext context)
        {
            string[,] states = {
               { "AL","Alabama" },
               { "AK","Alaska"},
               { "AZ","Arizona"},
               { "AR","Arkansas"},
               { "CA","California"},
               { "CO","Colorado"},
               { "CT","Connecticut"},
               { "DE","Delaware"},
               { "FL","Florida"},
               { "GA","Georgia"},
               { "HI","Hawaii"},
               { "ID","Idaho"},
               { "IL","Illinois"},
               { "IN","Indiana"},
               { "IA","Iowa"},
               { "KS","Kansas"},
               { "KY","Kentucky"},
               { "LA","Louisiana"},
               { "ME","Maine"},
               { "MD","Maryland"},
               { "MA","Massachusetts"},
               { "MI","Michigan"},
               { "MN","Minnesota"},
               { "MS","Mississippi"},
               { "MO","Missouri"},
               { "MT","Montana"},
               { "NE","Nebraska"},
               { "NV","Nevada"},
               { "NH","New Hampshire"},
               { "NJ","New Jersey"},
               { "NM","New Mexico"},
               { "NY","New York"},
               { "NC","North Carolina"},
               { "ND","North Dakota"},
               { "OH","Ohio"},
               { "OK","Oklahoma"},
               { "OR","Oregon"},
               { "PA","Pennsylvania"},
               { "RI","Rhode Island"},
               { "SC","South Carolina"},
               { "SD","South Dakota"},
               { "TN","Tennessee"},
               { "TX","Texas"},
               { "UT","Utah"},
               { "VT","Vermont"},
               { "VA","Virginia"},
               { "WA","Washington"},
               { "WV","West Virginia"},
               { "WI","Wisconsin"},
               { "WY","Wyoming"}
            };

            for (int s = 0; s < states.GetLength(0); s++)
            {
                string abbreviation = states[s, 0];
                string name = states[s, 1];
                State state = context.States.SingleOrDefault(x => x.Abbreviation == abbreviation);
                if (state == null)
                {
                    state = new State(abbreviation, name);
                    context.States.Add(state);
                    context.SaveChanges();
                    Logger.Verbose("Added state " + abbreviation + " - " + name);
                }
            }
        }

        private void SeedPhoneTypes(MigrationsDataContext context)
        {
            string[] seeds = new[] { "Home", "Work", "Cell", "Fax" };

            foreach (string seed in seeds)
            {
                PhoneType phoneType = context.PhoneTypes.SingleOrDefault(x => x.Name == seed);
                if (phoneType == null)
                {
                    phoneType = new PhoneType(seed) ;
                    context.PhoneTypes.Add(phoneType);
                    context.SaveChanges();
                    Logger.Verbose("Added phone type " + seed);
                }
            }
        }
    }
}
