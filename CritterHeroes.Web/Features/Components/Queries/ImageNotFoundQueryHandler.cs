using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Domain.Contracts.Storage;

namespace CritterHeroes.Web.Features.Components.Queries
{
    public class ImageNotFoundQuery : IQuery<string>
    {
    }

    public class ImageNotFoundQueryHandler : IQueryHandler<ImageNotFoundQuery, string>
    {
        private IAzureService _azureService;

        public ImageNotFoundQueryHandler(IAzureService azureService)
        {
            this._azureService = azureService;
        }

        public string Execute(ImageNotFoundQuery query)
        {
            return _azureService.GetNotFoundUrl();
        }
    }
}
