using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IOrganizationLogoService
    {
        string GetLogoUrl();
        void SaveLogo(Stream source, string filename);

        string GetTempLogoUrl();
        void SaveTempLogo(Stream source, string filename);
    }
}
