using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Models.LogEvents;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public class CritterMapperContext : MapperContext<CritterSource, Critter>
    {
        public CritterMapperContext(CritterSource source, Critter target, IAppEventPublisher publisher)
            : base(source, target, publisher)
        {
        }

        public Breed Breed
        {
            get;
            set;
        }

        public CritterStatus Status
        {
            get;
            set;
        }

        public Person Foster
        {
            get;
            set;
        }

        public Location Location
        {
            get;
            set;
        }

        public CritterColor Color
        {
            get;
            set;
        }
    }

    public class CritterMapper : Mapper<CritterSource, Critter, CritterMapperContext>
    {
        protected override void CreateConfiguration(MapperConfiguration<CritterSource, Critter, CritterMapperContext> configuration)
        {
            configuration
                .ForField
                (
                    fieldName: "animalBirthdate",
                    toAction: (CritterMapperContext context) =>
                    {
                        if (context.Source.BirthDate != null)
                        {
                            context.Target.BirthDate = context.Source.BirthDate;
                            context.Target.IsBirthDateExact = context.Source.IsBirthDateExact;
                        }
                    },
                    fromAction: (CritterMapperContext context) =>
                    {
                    }
                )
                .ForField
                (
                    fieldName: "animalCourtesy",
                    toAction: (CritterMapperContext context) => context.Target.IsCourtesy = context.Source.IsCourtesy ?? false,
                    fromAction: (CritterMapperContext context) => context.Source.IsCourtesy = context.Target.IsCourtesy
                )
                .ForField
                (
                    fieldName: "animalDescription",
                    toAction: (CritterMapperContext context) => context.Target.Description = context.Source.Description,
                    fromAction: (CritterMapperContext context) => context.Source.Description = context.Target.Description
                )
                .ForField
                (
                    fieldName: "animalKillDate",
                    toAction: (CritterMapperContext context) => context.Target.EuthanasiaDate = context.Source.EuthanasiaDate,
                    fromAction: (CritterMapperContext context) => context.Source.EuthanasiaDate = context.Target.EuthanasiaDate
                )
                .ForField
                (
                    fieldName: "animalKillReason",
                    toAction: (CritterMapperContext context) => context.Target.EuthanasiaReason = context.Source.EuthanasiaReason,
                    fromAction: (CritterMapperContext context) => context.Source.EuthanasiaReason = context.Target.EuthanasiaReason
                )
                .ForField
                (
                    fieldName: "animalMicrochipped",
                    toAction: (CritterMapperContext context) => context.Target.IsMicrochipped = context.Source.IsMicrochipped,
                    fromAction: (CritterMapperContext context) => context.Source.IsMicrochipped = context.Target.IsMicrochipped
                )
                .ForField
                (
                    fieldName: "animalName",
                    toAction: (CritterMapperContext context) => context.Target.Name = context.Source.Name,
                    fromAction: (CritterMapperContext context) => context.Source.Name = context.Target.Name
                )
                .ForField
                (
                    fieldName: "animalOKWithCats",
                    toAction: (CritterMapperContext context) => context.Target.IsOkWithCats = context.Source.IsOkWithCats,
                    fromAction: (CritterMapperContext context) => context.Source.IsOkWithCats = context.Target.IsOkWithCats
                )
                .ForField
                (
                    fieldName: "animalOKWithDogs",
                    toAction: (CritterMapperContext context) => context.Target.IsOkWithDogs = context.Source.IsOkWithDogs,
                    fromAction: (CritterMapperContext context) => context.Source.IsOkWithDogs = context.Target.IsOkWithDogs
                )
                .ForField
                (
                    fieldName: "animalOKWithKids",
                    toAction: (CritterMapperContext context) => context.Target.IsOkWithKids = context.Source.IsOkWithKids,
                    fromAction: (CritterMapperContext context) => context.Source.IsOkWithKids = context.Target.IsOkWithKids
                )
                .ForField
                (
                    fieldName: "animalOlderKidsOnly",
                    toAction: (CritterMapperContext context) => context.Target.OlderKidsOnly = context.Source.OlderKidsOnly,
                    fromAction: (CritterMapperContext context) => context.Source.OlderKidsOnly = context.Target.OlderKidsOnly
                )
                .ForField
                (
                    fieldName: "animalNotes",
                    toAction: (CritterMapperContext context) => context.Target.Notes = context.Source.Notes,
                    fromAction: (CritterMapperContext context) => context.Source.Notes = context.Target.Notes
                )
                .ForField
                (
                    fieldName: "animalSex",
                    toAction: (CritterMapperContext context) => context.Target.Sex = context.Source.Sex,
                    fromAction: (CritterMapperContext context) => context.Source.Sex = context.Target.Sex
                )
                .ForField
                (
                    fieldName: "animalSpecialDiet",
                    toAction: (CritterMapperContext context) => context.Target.HasSpecialDiet = context.Source.HasSpecialDiet ?? false,
                    fromAction: (CritterMapperContext context) => context.Source.HasSpecialDiet = context.Target.HasSpecialDiet
                )
                .ForField
                (
                    fieldName: "animalSpecialneeds",
                    toAction: (CritterMapperContext context) => context.Target.HasSpecialNeeds = context.Source.HasSpecialNeeds ?? false,
                    fromAction: (CritterMapperContext context) => context.Source.HasSpecialNeeds = context.Target.HasSpecialNeeds
                )
                .ForField
                (
                    fieldName: "animalSpecialneedsDescription",
                    toAction: (CritterMapperContext context) => context.Target.SpecialNeedsDescription = context.Source.SpecialNeedsDescription,
                    fromAction: (CritterMapperContext context) => context.Source.SpecialNeedsDescription = context.Target.SpecialNeedsDescription
                )
                .ForField
                (
                    fieldName: "animalRescueID",
                    toAction: (CritterMapperContext context) => context.Target.RescueID = context.Source.RescueID,
                    fromAction: (CritterMapperContext context) => context.Source.RescueID = context.Target.RescueID
                )
                .ForField
                (
                    fieldName: "animalUpdatedDate",
                    toAction: (CritterMapperContext context) => context.Target.RescueGroupsLastUpdated = context.Source.LastUpdated
                )
                .ForField
                (
                    fieldName: "animalReceivedDate",
                    toAction: (CritterMapperContext context) => context.Target.ReceivedDate = context.Source.ReceivedDate
                )
                .ForField
                (
                    fieldName: "animalCreatedDate",
                    toAction: (CritterMapperContext context) => context.Target.RescueGroupsCreated = context.Source.Created
                )
                .ForField
                (
                    fieldName: "animalGeneralAge",
                    toAction: (CritterMapperContext context) => context.Target.GeneralAge = context.Source.GeneralAge
                )
                .ForField
                (
                    fieldName: "animalFosterID",
                    toAction: (CritterMapperContext context) =>
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
                    },
                    fromAction: (CritterMapperContext context) =>
                    {
                    }
                )
                .ForField
                (
                    fieldName: "animalPrimaryBreedID",
                    toAction: (CritterMapperContext context) =>
                    {
                        if (context.Target.BreedID != context.Breed.ID)
                        {
                            context.Publisher.Publish(CritterLogEvent.Action("Changed breed from {OldBreed} to {NewBreed} for {CritterID} - {CritterName}", context.Target.Breed.IfNotNull(x => x.BreedName, "not set"), context.Breed.BreedName, context.Source.ID, context.Source.Name));
                            context.Target.ChangeBreed(context.Breed);
                        }
                    },
                    fromAction: (CritterMapperContext context) =>
                    {
                    }
                )
                .ForField
                (
                    fieldName: "animalStatusID",
                    toAction: (CritterMapperContext context) =>
                    {
                        if (context.Target.StatusID != context.Status.ID)
                        {
                            context.Publisher.Publish(CritterLogEvent.Action("Changed status from {OldStatus} to {NewStatus} for {CritterID} - {CritterName}", context.Target.Status.IfNotNull(x => x.Name, "not set"), context.Status.Name, context.Source.ID, context.Source.Name));
                            context.Target.ChangeStatus(context.Status);
                        }
                    },
                    fromAction: (CritterMapperContext context) =>
                    {
                    }
                )
                .ForField
                (
                    fieldName: "animalLocationID",
                    toAction: (CritterMapperContext context) =>
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
                    },
                    fromAction: (CritterMapperContext context) =>
                    {
                    }
                )
                .ForField
                (
                    fieldName: "`ColorID",
                    toAction: (CritterMapperContext context) =>
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
                    },
                    fromAction: (CritterMapperContext context) =>
                    {
                    }
                );
        }
    }
}
