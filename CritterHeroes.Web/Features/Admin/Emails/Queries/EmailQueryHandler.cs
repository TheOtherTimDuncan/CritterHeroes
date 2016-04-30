using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Emails.Models;
using TOTD.EntityFramework;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Features.Admin.Emails.Queries
{
    public class EmailQuery : IAsyncQuery<EmailModel>
    {
        public bool SendEmail
        {
            get;
            set;
        }
    }

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
                    GeneralAge = x.GeneralAge,
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

                FosterSummaryEmailCommand emailSummary = new FosterSummaryEmailCommand("tduncan72@gmail.com");
                emailSummary.AddTo("catmamadiane@yahoo.co");

                emailSummary.EmailData.Critters = data
                    .Select(x => new
                    {
                        Foster = x.Foster,
                        Baby = (x.GeneralAge.SafeEquals("Baby") ? 1 : 0),
                        Young = (x.GeneralAge.SafeEquals("Young") ? 1 : 0),
                        Adult = (x.GeneralAge.SafeEquals("Adult") ? 1 : 0),
                        Senior = (x.GeneralAge.SafeEquals("Senior") ? 1 : 0)
                    })
                    .OrderBy(x => x.Foster)
                    .GroupBy(x => x.Foster)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Baby = g.Sum(x => x.Baby),
                        Young = g.Sum(x => x.Young),
                        Adult = g.Sum(x => x.Adult),
                        Senior = g.Sum(x => x.Senior)
                    })
                    .ToList();

                await _emailService.SendEmailAsync(emailSummary);

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
