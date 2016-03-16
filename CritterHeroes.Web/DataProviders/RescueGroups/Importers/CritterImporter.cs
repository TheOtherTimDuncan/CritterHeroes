using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.LogEvents;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Importers
{
    public class CritterImporter
    {
        public Dictionary<string, Action<CritterImportContext>> _importers;

        public CritterImporter()
        {
            this._importers = new Dictionary<string, Action<CritterImportContext>>()
            {
                { "animalBirthdate",                    ImportBirthDate },
                { "animalColorID",                      ImportColor },
                { "animalCourtesy",                     ImportCourtesy },
                { "animalCreatedDate",                  ImportCreated },
                { "animalDescription",                  ImportDescription },
                { "animalFosterID",                     ImportFoster },
                { "animalGeneralAge",                   ImportGeneralAge },
                { "animalKillDate",                     ImportEuthanasiaDate },
                { "animalKillReason",                   ImportEuthanasiaReason },
                { "animalLocationID",                   ImportLocation },
                { "animalMicrochipped",                 ImportIsMicrochipped },
                { "animalName",                         ImportName },
                { "animalNotes",                        ImportNotes },
                { "animalOKWithCats",                   ImportIsOkWithCats },
                { "animalOKWithDogs",                   ImportIsOkWithDogs },
                { "animalOKWithKids",                   ImportIsOkWithKids },
                { "animalOlderKidsOnly",                ImportOlderKidsOnly },
                { "animalPrimaryBreedID",               ImportBreed },
                { "animalReceivedDate",                 ImportReceivedDate },
                { "animalRescueID",                     ImportRescueID },
                { "animalSex",                          ImportSex },
                { "animalSpecialDiet",                  ImportSpecialDiet },
                { "animalSpecialneeds",                 ImportSpecialNeeds },
                { "animalSpecialneedsDescription",      ImportSpecialNeedsDescription },
                { "animalStatusID",                     ImportStatus },
                { "animalUpdatedDate",                  ImportLastUpdated }
            };
        }

        public void Import(CritterImportContext context, params string[] properties)
        {
            foreach (var importer in _importers.Where(x => properties.IsNullOrEmpty() || properties.Contains(x.Key)))
            {
                importer.Value(context);
            }
        }

        public void ImportReceivedDate(CritterImportContext context)
        {
            context.Target.ReceivedDate = context.Source.ReceivedDate;
        }

        public void ImportSpecialDiet(CritterImportContext context)
        {
            context.Target.HasSpecialDiet = context.Source.HasSpecialDiet ?? false;
        }

        public void ImportSpecialNeeds(CritterImportContext context)
        {
            context.Target.HasSpecialNeeds = context.Source.HasSpecialNeeds ?? false;
        }

        public void ImportSpecialNeedsDescription(CritterImportContext context)
        {
            context.Target.SpecialNeedsDescription = context.Source.SpecialNeedsDescription;
        }

        public void ImportGeneralAge(CritterImportContext context)
        {
            context.Target.GeneralAge = context.Source.GeneralAge;
        }

        public void ImportDescription(CritterImportContext context)
        {
            context.Target.Description = context.Source.Description;
        }

        public void ImportCourtesy(CritterImportContext context)
        {
            context.Target.IsCourtesy = context.Source.IsCourtesy ?? false;
        }

        public void ImportName(CritterImportContext context)
        {
            context.Target.Name = context.Source.Name;
        }

        public void ImportSex(CritterImportContext context)
        {
            context.Target.Sex = context.Source.Sex;
        }

        public void ImportRescueID(CritterImportContext context)
        {
            context.Target.RescueID = context.Source.RescueID;
        }

        public void ImportBreed(CritterImportContext context)
        {
            if (context.Target.BreedID != context.Breed.ID)
            {
                context.Publisher.Publish(CritterLogEvent.Action("Changed breed from {OldBreed} to {NewBreed} for {CritterID} - {CritterName}", context.Target.Breed.IfNotNull(x => x.BreedName, "not set"), context.Breed.BreedName, context.Source.ID, context.Source.Name));
                context.Target.ChangeBreed(context.Breed);
            }
        }

        public void ImportStatus(CritterImportContext context)
        {
            if (context.Target.StatusID != context.Status.ID)
            {
                context.Publisher.Publish(CritterLogEvent.Action("Changed status from {OldStatus} to {NewStatus} for {CritterID} - {CritterName}", context.Target.Status.IfNotNull(x => x.Name, "not set"), context.Status.Name, context.Source.ID, context.Source.Name));
                context.Target.ChangeStatus(context.Status);
            }
        }

        public void ImportFoster(CritterImportContext context)
        {
            if (!context.Source.FosterContactID.IsNullOrEmpty())
            {
                if (context.Target.FosterID != context.Foster.ID)
                {
                    context.Publisher.Publish(CritterLogEvent.Action("Changed foster from {OldFoster} to {NewFoster} for {CritterID} - {CritterName}", context.Target.Foster.IfNotNull(x => x.FirstName, "not set"), context.Foster.FirstName, context.Source.ID, context.Source.Name));
                    context.Target.ChangeFoster(context.Foster);
                }
            }
            else if (context.Target.FosterID != null)
            {
                context.Publisher.Publish(CritterLogEvent.Action("Removed foster {OldFoster} for {CritterID} - {CritterName}", context.Target.Foster.IfNotNull(x => x.FirstName, "not set"), context.Source.ID, context.Source.Name));
                context.Target.RemoveFoster();
            }
        }

        public void ImportLocation(CritterImportContext context)
        {
            if (!context.Source.LocationID.IsNullOrEmpty())
            {
                if (context.Target.LocationID != context.Location.ID)
                {
                    context.Publisher.Publish(CritterLogEvent.Action("Changed location from {OldLocation} to {NewLocation} for {CritterID} - {CritterName}", context.Target.Location.IfNotNull(x => x.Name, "not set"), context.Location.Name, context.Source.ID, context.Source.Name));
                    context.Target.ChangeLocation(context.Location);
                }
            }
            else if (context.Target.LocationID != null)
            {
                context.Publisher.Publish(CritterLogEvent.Action("Removed location {OldLocation} for {CritterID} - {CritterName}", context.Target.Location.IfNotNull(x => x.Name, "not set"), context.Source.ID, context.Source.Name));
                context.Target.RemoveLocation();
            }
        }

        public void ImportLastUpdated(CritterImportContext context)
        {
            context.Target.RescueGroupsLastUpdated = context.Source.LastUpdated;
        }

        public void ImportCreated(CritterImportContext context)
        {
            context.Target.RescueGroupsCreated = context.Source.Created;
        }

        public void ImportBirthDate(CritterImportContext context)
        {
            if (context.Source.BirthDate != null)
            {
                context.Target.BirthDate = context.Source.BirthDate;
                context.Target.IsBirthDateExact = context.Source.IsBirthDateExact;
            }
        }

        public void ImportColor(CritterImportContext context)
        {
            if (!context.Source.ColorID.IsNullOrEmpty())
            {
                if (context.Target.ColorID != context.Color.ID)
                {
                    context.Publisher.Publish(CritterLogEvent.Action("Changed color from {OldColor} to {NewColor} for {CritterID} - {CritterName}", context.Target.Color.IfNotNull(x => x.Description, "not set"), context.Color.Description, context.Source.ID, context.Source.Name));
                    context.Target.ChangeColor(context.Color);
                }
            }
            else if (context.Target.Color != null)
            {
                context.Publisher.Publish(CritterLogEvent.Action("Removed color {OldColor} for {CritterID} - {CritterName}", context.Target.Color.IfNotNull(x => x.Description, "not set"), context.Source.ID, context.Source.Name));
                context.Target.RemoveColor();
            }
        }

        public void ImportEuthanasiaDate(CritterImportContext context)
        {
            context.Target.EuthanasiaDate = context.Source.EuthanasiaDate;
        }

        public void ImportEuthanasiaReason(CritterImportContext context)
        {
            context.Target.EuthanasiaReason = context.Source.EuthanasiaReason;
        }

        public void ImportNotes(CritterImportContext context)
        {
            context.Target.Notes = context.Source.Notes;
        }

        public void ImportIsMicrochipped(CritterImportContext context)
        {
            context.Target.IsMicrochipped = context.Source.IsMicrochipped;
        }

        public void ImportIsOkWithDogs(CritterImportContext context)
        {
            context.Target.IsOkWithDogs = context.Source.IsOkWithDogs;
        }

        public void ImportIsOkWithCats(CritterImportContext context)
        {
            context.Target.IsOkWithCats = context.Source.IsOkWithCats;
        }

        public void ImportIsOkWithKids(CritterImportContext context)
        {
            context.Target.IsOkWithKids = context.Source.IsOkWithKids;
        }

        public void ImportOlderKidsOnly(CritterImportContext context)
        {
            context.Target.OlderKidsOnly = context.Source.OlderKidsOnly;
        }
    }
}
