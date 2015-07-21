using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Contexts
{
    public class AnimalStatusStorageContext : BaseDbContext<AnimalStatus>
    {
        public override async Task<AnimalStatus> GetAsync(string entityID)
        {
            throw new NotImplementedException();
        }
    }
}