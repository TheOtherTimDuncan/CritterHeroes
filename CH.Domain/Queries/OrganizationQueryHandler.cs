using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Queries;
using CH.Domain.Models;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Queries
{
    public class OrganizationQueryHandler : IQueryHandler<OrganizationQuery, Organization>
    {
        private IStorageContext<Organization> _storageContext;

        public OrganizationQueryHandler( IStorageContext<Organization> storageContext)
        {
            this._storageContext = storageContext;
        }

        public async Task<Organization> Retrieve(OrganizationQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");
            return await _storageContext.GetAsync(query.OrganizationID.ToString());
        }
    }
}
