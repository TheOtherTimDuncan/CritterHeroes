using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Domain.Contracts.Storage
{
    public interface IOrganizationLogoService
    {
        string GetLogoUrl();
        Task SaveLogo(Stream source, string filename, string contentType);
    }
}
