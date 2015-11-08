using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Emails.Models;
using CritterHeroes.Web.Areas.Admin.Emails.Queries;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Email;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Models;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Admin.Emails.QueryHandlers
{
    public class EmailQueryHandler : IAsyncQueryHandler<EmailQuery, EmailModel>
    {
        private ISqlStorageContext<Critter> _critterStorage;
        private IEmailService _emailService;

        public EmailQueryHandler(ISqlStorageContext<Critter> critterStorage, IEmailService emailService)
        {
            this._critterStorage = critterStorage;
            this._emailService = emailService;
        }

        public async Task<EmailModel> RetrieveAsync(EmailQuery query)
        {
            var data = await _critterStorage.Entities.OrderBy(x => x.Name).Where(x => new[] { "Available", "Not Available", "Sponsorship" }.Contains(x.Status.Name)).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                Status = x.Status.Name,
                Sex = x.Sex,
                RescueGroupsID = x.RescueGroupsID,
                RescueID = x.RescueID,
                Foster = x.Foster.FirstName + " " + x.Foster.LastName,
                FosterEmail = x.Foster.Email,
                PictureFilename = x.Pictures.FirstOrDefault(p => p.Picture.DisplayOrder == 1).Picture.Filename
            }).ToListAsync();

            FosterCrittersEmailCommand emailCommand = new FosterCrittersEmailCommand("tduncan72@gmail.com")
            {
                OrganizationFullName = "Friends For Life Animal Haven",
                OrganizationShortName = "FFLAH",
                HomeUrl = "http://www.fflah.org/",
                LogoUrl = "https://fflah.blob.core.windows.net/fflah/logo.svg",
                Critters = data.Select(x =>
                {

                    string urlThumbnail = null;
                    if (!x.PictureFilename.IsNullOrEmpty())
                    {
                        urlThumbnail = "https://s3.amazonaws.com/filestore.rescuegroups.org/1211/pictures/animals/" + x.RescueGroupsID.ToString().Substring(0, 4) + "/" + x.RescueGroupsID + "/" + x.PictureFilename;
                    }

                    return new
                    {
                        UrlThumbnail = urlThumbnail,
                        Name = x.Name,
                        Status = x.Status,
                        RescueID = x.RescueID,
                        Sex = x.Sex,
                        RescueGroupsID = x.RescueGroupsID
                    };
                })
            };

            if (query.SendEmail)
            {
                await _emailService.SendEmailAsync(emailCommand);
            }

            EmailModel model = new EmailModel()
            {
                //HtmlBody = msg.HtmlBody
            };

            return model;
        }
    }
}
