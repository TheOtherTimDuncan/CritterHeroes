using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Shared
{
    public static class AgeHelper
    {
        public static string GetLifeStage(string speciesName, DateTime? birthDate, string generalAge)
        {
            if (birthDate == null)
            {
                return generalAge;
            }

            int ageMonths = (DateTime.Now.Month - birthDate.Value.Month) + 12 * (DateTime.Now.Year - birthDate.Value.Year);

            switch (speciesName)
            {
                case "Cat":
                    if (ageMonths <= 6)
                    {
                        return "Kitten";
                    }
                    else if (ageMonths <= 24)
                    {
                        return "Young";
                    }
                    else if (ageMonths < 120)
                    {
                        return "Adult";
                    }
                    return "Senior";

                case "Dog":
                    if (ageMonths <= 6)
                    {
                        return "Puppy";
                    }
                    else if (ageMonths <= 18)
                    {
                        return "Young";
                    }
                    else if (ageMonths < 72)
                    {
                        return "Adult";
                    }
                    return "Senior";

                default:
                    throw new ArgumentOutOfRangeException(nameof(speciesName), speciesName);
            }
        }
    }
}
