using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Features.Admin.Emails.Models;
using CritterHeroes.Web.Shared;
using CritterHeroes.Web.Shared.Commands;
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
        private ISqlQueryStorageContext<Critter> _critterStorage;
        private IEmailService<CrittersEmailCommand> _crittersEmailService;
        private IEmailService<FosterCrittersEmailCommand> _fostersEmailService;
        private IEmailService<FosterSummaryEmailCommand> _summaryEmailService;

        public EmailQueryHandler(ISqlQueryStorageContext<Critter> critterStorage, IEmailService<CrittersEmailCommand> emailService, IEmailService<FosterCrittersEmailCommand> fostersEmailService, IEmailService<FosterSummaryEmailCommand> summaryEmailService)
        {
            this._critterStorage = critterStorage;
            this._crittersEmailService = emailService;
            this._fostersEmailService = fostersEmailService;
            this._summaryEmailService = summaryEmailService;
        }

        public async Task<EmailModel> ExecuteAsync(EmailQuery query)
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
                    BirthDate = x.BirthDate,
                    IsBirthDateExact = x.IsBirthDateExact,
                    SpeciesName = x.Breed.Species.Name,
                    PictureFilename = x.Pictures.FirstOrDefault(p => p.Picture.DisplayOrder == 1).Picture.Filename
                });

            if (query.SendEmail)
            {
                CrittersEmailCommand emailCommandCritters = new CrittersEmailCommand("tduncan72@gmail.com");
                emailCommandCritters.AddTo("nan3176165@aol.com");
                emailCommandCritters.AddTo("swonger66@gmail.com");
                emailCommandCritters.AddTo("uponastar9@yahoo.com");
                emailCommandCritters.AddTo("pchapman72@yahoo.com");

                emailCommandCritters.Critters = data.Select(x =>
                {

                    string urlThumbnail = null;
                    if (!x.PictureFilename.IsNullOrEmpty())
                    {
                        urlThumbnail = "https://s3.amazonaws.com/filestore.rescuegroups.org/1211/pictures/animals/" + x.RescueGroupsID.ToString().Substring(0, 4) + "/" + x.RescueGroupsID + "/" + x.PictureFilename;
                    }

                    return new CrittersEmailCommand.Critter()
                    {
                        ThumbnailUrl = urlThumbnail,
                        Name = x.Name,
                        Status = x.Status,
                        RescueID = x.RescueID,
                        Sex = x.Sex,
                        RescueGroupsID = x.RescueGroupsID,
                        Location = x.LocationName,
                        FosterName = x.Foster,
                        FosterEmail = x.FosterEmail,
                        IsBirthDateExact = x.IsBirthDateExact ?? false,
                        Birthdate = x.BirthDate
                    };
                });

                await _crittersEmailService.SendEmailAsync(emailCommandCritters);

                FosterSummaryEmailCommand emailSummary = new FosterSummaryEmailCommand("catmamadiane@yahoo.com");
                emailSummary.EmailBcc = "tduncan72@gmail.com";

                emailSummary.Summaries = data
                    .Select(x =>
                    {
                        string lifeStage = AgeHelper.GetLifeStage(x.SpeciesName, x.BirthDate, x.GeneralAge);
                        return new FosterSummaryEmailCommand.Summary()
                        {
                            FosterName = x.Foster,
                            BabyCount = (lifeStage.SafeEquals("Kitten") || lifeStage.SafeEquals("Puppy") ? 1 : 0),
                            YoungCount = (lifeStage.SafeEquals("Young") ? 1 : 0),
                            AdultCount = (lifeStage.SafeEquals("Adult") ? 1 : 0),
                            SeniorCount = (lifeStage.SafeEquals("Senior") ? 1 : 0)
                        };
                    })
                    .OrderBy(x => x.FosterName)
                    .GroupBy(x => x.FosterName)
                    .Select(g => new FosterSummaryEmailCommand.Summary()
                    {
                        FosterName = g.Key,
                        BabyCount = g.Sum(x => x.BabyCount),
                        YoungCount = g.Sum(x => x.YoungCount),
                        AdultCount = g.Sum(x => x.AdultCount),
                        SeniorCount = g.Sum(x => x.SeniorCount)
                    })
                    .ToList();

                await _summaryEmailService.SendEmailAsync(emailSummary);

                IEnumerable<string> fosterEmails = data.Where(x => !x.FosterEmail.IsNullOrEmpty()).Select(x => x.FosterEmail).Distinct();
                foreach (string email in fosterEmails)
                {

                    FosterCrittersEmailCommand emailcommandFosterCritters = new FosterCrittersEmailCommand(email);
                    emailcommandFosterCritters.EmailBcc = "nan3176165@aol.com";

                    emailcommandFosterCritters.Critters = data.Where(x => x.FosterEmail == email).Select(x =>
                    {

                        string urlThumbnail = null;
                        if (!x.PictureFilename.IsNullOrEmpty())
                        {
                            urlThumbnail = "https://s3.amazonaws.com/filestore.rescuegroups.org/1211/pictures/animals/" + x.RescueGroupsID.ToString().Substring(0, 4) + "/" + x.RescueGroupsID + "/" + x.PictureFilename;
                        }

                        return new FosterCrittersEmailCommand.Critter()
                        {
                            ThumbnailUrl = urlThumbnail,
                            Name = x.Name,
                            Status = x.Status,
                            RescueID = x.RescueID,
                            Sex = x.Sex,
                            RescueGroupsID = x.RescueGroupsID,
                            IsBirthDateExact = x.IsBirthDateExact ?? false,
                            Birthdate = x.BirthDate
                        };
                    });

                    await _fostersEmailService.SendEmailAsync(emailcommandFosterCritters);
                }
            }

            EmailModel model = new EmailModel();
            return model;
        }
    }
}
