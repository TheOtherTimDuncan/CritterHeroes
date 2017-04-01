using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Mailer.Core;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.Email
{
    public class FosterCrittersEmailBuilder : EmailBuilderBase<FosterCrittersEmailCommand>
    {
        private const string _styles = @"
    .container {
      width: 800px !important;
      max-width: 800px !important;
    }
    .content {
           max-width: 800px !important;
    }
";
        public FosterCrittersEmailBuilder(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
            : base(urlGenerator, stateManager, logoService, emailConfiguration)
        {
        }

        protected override EmailBuilder BuildEmail(EmailBuilder builder, FosterCrittersEmailCommand command)
        {
            EmailBuilder result = builder
                .AddStyles(_styles)
                .AddParagraph($"According to {command.OrganizationShortName} records, the following critters are currently in your care. If any of this information is out of date, please respond to this email with what needs changed.")
                .BeginTable(className: "critters-list")
                    .BeginTableRow()
                        .AddTableHeader(String.Empty)
                        .AddTableHeader("Name")
                        .AddTableHeader("Status")
                        .AddTableHeader("Sex")
                        .AddTableHeader("Birthdate")
                        .AddTableHeader("ID")
                        .AddTableHeader(String.Empty)
                    .EndTableRow();

            foreach (var data in command.Critters)
            {
                builder.BeginTableRow();

                if (!data.ThumbnailUrl.IsNullOrEmpty())
                {
                    builder
                        .BeginTableCell()
                        .AddImage(data.ThumbnailUrl, "Thumbnail")
                        .EndTableCell();
                }
                else
                {
                    builder.AddTableCell(String.Empty);
                }

                builder.AddTableCell(data.Name);
                builder.AddTableCell(data.Status);
                builder.AddTableCell(data.Sex);

                //builder.AddTableCell(AgeHelper.GetAge(data.Birthdate, data.IsBirthDateExact));
                builder.BeginTableCell();
                builder.AddText($"{data.Birthdate:MM/dd/yyyy}");
                if (data.Birthdate != null && !data.IsBirthDateExact)
                {
                    builder.AddLineBreak();
                    builder.AddText("(approximate)");
                }
                builder.EndTableCell();

                builder.AddTableCell(data.RescueID);

                builder.
                    BeginTableCell()
                        .AddLink($"http://www.fflah.org/animals/detail?AnimalID={data.RescueGroupsID}", "View")
                    .EndTableCell();

                builder.EndTableRow();
            }

            result = result
                .EndTable()
                .WithSubject($"Critters Update - {command.OrganizationFullName}");

            return result;
        }
    }
}
