using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Emails.Queries;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using TOTD.EntityFramework;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Admin.Emails.QueryHandlers
{
    public class EmailQueryHandler : IAsyncQueryHandler<EmailQuery, Models.EmailModel>
    {
        private ISqlStorageContext<Critter> _critterStorage;
        private IEmailService _emailService;

        public EmailQueryHandler(ISqlStorageContext<Critter> critterStorage, IEmailService emailService)
        {
            this._critterStorage = critterStorage;
            this._emailService = emailService;
        }

        public async Task<Models.EmailModel> ExecuteAsync(EmailQuery query)
        {
            var data = await _critterStorage.Entities
                .OrderBy(x => x.Name)
                .Where(x => new[] { "Available", "Not Available", "Sponsorship" }.Contains(x.Status.Name))
                .SelectToListAsync(x => new
                {
                    ID = x.ID,
                    Name = x.Name,
                    Status = x.Status.Name,
                    Sex = x.Sex,
                    RescueGroupsID = x.RescueGroupsID,
                    RescueID = x.RescueID,
                    Foster = x.Foster.FirstName + " " + x.Foster.LastName,
                    FosterEmail = x.Foster.Email,
                    LocationName = x.Location.Name,
                    PictureFilename = x.Pictures.FirstOrDefault(p => p.Picture.DisplayOrder == 1).Picture.Filename
                });

            if (query.SendEmail)
            {
                CrittersEmailCommand emailCommandCritters = new CrittersEmailCommand("tduncan72@gmail.com");
                emailCommandCritters.AddTo("nan3176165@aol.com");
                emailCommandCritters.AddTo("swonger66@gmail.com");
                emailCommandCritters.AddTo("uponastar9@yahoo.com");
                emailCommandCritters.AddTo("pchapman72@yahoo.com");

                emailCommandCritters.EmailData.Critters = data.Select(x =>
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
                        RescueGroupsID = x.RescueGroupsID,
                        Location = x.LocationName,
                        FosterName = x.Foster,
                        FosterEmail = x.FosterEmail
                    };
                });

                await _emailService.SendEmailAsync(emailCommandCritters);

                IEnumerable<string> fosterEmails = data.Where(x => !x.FosterEmail.IsNullOrEmpty()).Select(x => x.FosterEmail).Distinct();
                foreach (string email in fosterEmails)
                {

                    FosterCrittersEmailCommand emailcommandFosterCritters = new FosterCrittersEmailCommand(email);
                    emailcommandFosterCritters.EmailData.Critters = data.Where(x => x.FosterEmail == email).Select(x =>
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
                    });

                    await _emailService.SendEmailAsync(emailcommandFosterCritters);
                }
            }

            Models.EmailModel model = new Models.EmailModel();
            return model;
        }
    }
}
