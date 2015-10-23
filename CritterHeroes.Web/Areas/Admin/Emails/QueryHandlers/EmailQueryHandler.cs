using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Emails.Models;
using CritterHeroes.Web.Areas.Admin.Emails.Queries;
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
        private IEmailClient _emailClient;

        public EmailQueryHandler(ISqlStorageContext<Critter> critterStorage, IEmailClient emailClient)
        {
            this._critterStorage = critterStorage;
            this._emailClient = emailClient;
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

            EmailMessage msg = new EmailMessage();
            EmailBuilder
                .Begin(msg)
                    .StartTable()
                        .StartTableRow()
                            .AddTableHeader("&nbsp;")
                            .AddTableHeader("Name")
                            .AddTableHeader("Status")
                            .AddTableHeader("ID")
                            .AddTableHeader("Sex")
                            .AddTableHeader("Foster Name")
                            .AddTableHeader("Foster Email")
                            .AddTableHeader("&nbsp;")
                            .AddTableHeader("&nbsp;")
                        .EndTableRow()
                        .Repeat(data, (element, builder) =>
                        {
                            builder.StartTableRow();

                            if (!element.PictureFilename.IsNullOrEmpty())
                            {
                                builder.AddTableCell("<img height='50' src='https://s3.amazonaws.com/filestore.rescuegroups.org/1211/pictures/animals/" + element.RescueGroupsID.ToString().Substring(0, 4) + "/" + element.RescueGroupsID + "/" + element.PictureFilename + "'/>");
                                //builder.AddTableCell("<img height='50' src='/debug/image/critter/" + element.ID + "/" + element.PictureFilename + "?height=50'/>");
                            }
                            else
                            {
                                builder.AddTableCell("&nbsp;");
                            }

                            builder.AddTableCell(element.Name);
                            builder.AddTableCell(element.Status);
                            builder.AddTableCell(element.RescueID);
                            builder.AddTableCell(element.Sex);
                            builder.AddTableCell(element.Foster);
                            builder.AddTableCell(element.FosterEmail);
                            builder.AddTableCell("<a href='http://www.fflah.org/animals/detail?AnimalID=" + element.RescueGroupsID + "'>View<a/>");
                            builder.AddTableCell("<a href='https://manage.rescuegroups.org/animal?animalID=" + element.RescueGroupsID + "'>Edit<a/>");
                            builder.EndTableRow();
                        })
                    .EndTable()
                .End();

            if (query.SendEmail)
            {
                msg.To.Add("tduncan72@gmail.com");
                msg.Subject = "FFLAH Critters Update";
                await _emailClient.SendAsync(msg);
            }

            EmailModel model = new EmailModel()
            {
                HtmlBody = msg.HtmlBody
            };

            return model;
        }
    }
}
