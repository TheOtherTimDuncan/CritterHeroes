using System;
using System.Collections.Generic;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Shared.StateManagement
{
    public class SpeciesContext
    {
        public static SpeciesContext FromSpecies(Species species)
        {
            return new SpeciesContext()
            {
                ID = species.ID,
                Name = species.Name
            };
        }

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
