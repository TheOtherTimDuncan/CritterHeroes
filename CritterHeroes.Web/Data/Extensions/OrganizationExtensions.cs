using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Extensions
{
    public static class OrganizationExtensions
    {
        public static IQueryable<Organization> MatchingID(this IQueryable<Organization> source, Guid organizationID)
        {
            return source.Where(x => x.ID == organizationID);
        }

        public async static Task<Organization> FindByIDAsync(this IQueryable<Organization> source, Guid organizationID)
        {
            return await source.MatchingID(organizationID).SingleOrDefaultAsync();
        }
    }
}