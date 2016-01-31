using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.Azure.Logging
{
    public class AzureEmailLogger : BaseAzureLogger, IEmailLogger
    {
        private IAzureService _azureService;

        private const string _blobPath = "email";
        private const bool _isPrivate = true;

        public AzureEmailLogger(IAzureService azureService)
            : base(azureService, LogCategory.Email)
        {
            this._azureService = azureService;
        }

        public async Task LogEmailAsync(EmailModel email)
        {
            Guid blobID = Guid.NewGuid();

            Logger
                .ForContext("BlobID", blobID)
                .Information("Sent email from {From} to {To}", email.From, email.To);

            string emailData = JsonConvert.SerializeObject(email.EmailData);

            await _azureService.UploadBlobAsync($"{_blobPath}/{blobID}", _isPrivate, emailData);
        }
    }
}
