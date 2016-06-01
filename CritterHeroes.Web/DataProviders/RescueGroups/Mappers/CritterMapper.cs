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
                    fieldName: "animalID",
                    fromAction: context =>
                    {
                        context.Source.ID = context.Target.RescueGroupsID.Value;
                    }
                )
                .ForField
                (
                    fieldName: "animalBirthdate",
                    toAction: context =>
                    {
                        if (context.Source.BirthDate != null)
                        {
                            context.Target.BirthDate = context.Source.BirthDate;
                            context.Target.IsBirthDateExact = context.Source.IsBirthDateExact;
                        }
                    },
                    fromAction: context =>
                    {
                        if (context.Target.BirthDate != null)
                        {
                            context.Source.BirthDate = context.Target.BirthDate;
                            context.Source.IsBirthDateExact = context.Target.IsBirthDateExact;
                        }
                        else
                        {
                            context.Source.BirthDate = null;
                            context.Source.IsBirthDateExact = null;
                        }
                    }
                )
                .ForField
                (
                    fieldName: "animalCourtesy",
                    toAction: context => context.Target.IsCourtesy = context.Source.IsCourtesy ?? false,
                    fromAction: context => context.Source.IsCourtesy = context.Target.IsCourtesy
                )
                .ForField
                (
                    fieldName: "animalDescription",
                    toAction: context => context.Target.Description = context.Source.Description,
                    fromAction: context => context.Source.Description = context.Target.Description
                )
                .ForField
                (
                    fieldName: "animalKillDate",
                    toAction: context => context.Target.EuthanasiaDate = context.Source.EuthanasiaDate,
                    fromAction: context => context.Source.EuthanasiaDate = context.Target.EuthanasiaDate
                )
                .ForField
                (
                    fieldName: "animalKillReason",
                    toAction: context => context.Target.EuthanasiaReason = context.Source.EuthanasiaReason,
                    fromAction: context => context.Source.EuthanasiaReason = context.Target.EuthanasiaReason
                )
                .ForField
                (
                    fieldName: "animalMicrochipped",
                    toAction: context => context.Target.IsMicrochipped = context.Source.IsMicrochipped,
                    fromAction: context => context.Source.IsMicrochipped = context.Target.IsMicrochipped
                )
                .ForField
                (
                    fieldName: "animalName",
                    toAction: context => context.Target.Name = context.Source.Name,
                    fromAction: context => context.Source.Name = context.Target.Name
                )
                .ForField
                (
                    fieldName: "animalOKWithCats",
                    toAction: context => context.Target.IsOkWithCats = context.Source.IsOkWithCats,
                    fromAction: context => context.Source.IsOkWithCats = context.Target.IsOkWithCats
                )
                .ForField
                (
                    fieldName: "animalOKWithDogs",
                    toAction: context => context.Target.IsOkWithDogs = context.Source.IsOkWithDogs,
                    fromAction: context => context.Source.IsOkWithDogs = context.Target.IsOkWithDogs
                )
                .ForField
                (
                    fieldName: "animalOKWithKids",
                    toAction: context => context.Target.IsOkWithKids = context.Source.IsOkWithKids,
                    fromAction: context => context.Source.IsOkWithKids = context.Target.IsOkWithKids
                )
                .ForField
                (
                    fieldName: "animalOlderKidsOnly",
                    toAction: context => context.Target.OlderKidsOnly = context.Source.OlderKidsOnly,
                    fromAction: context => context.Source.OlderKidsOnly = context.Target.OlderKidsOnly
                )
                .ForField
                (
                    fieldName: "animalNotes",
                    toAction: context => context.Target.Notes = context.Source.Notes,
                    fromAction: context => context.Source.Notes = context.Target.Notes
                )
                .ForField
                (
                    fieldName: "animalSex",
                    toAction: context => context.Target.Sex = context.Source.Sex,
                    fromAction: context => context.Source.Sex = context.Target.Sex
                )
                .ForField
                (
                    fieldName: "animalSpecialDiet",
                    toAction: context => context.Target.HasSpecialDiet = context.Source.HasSpecialDiet ?? false,
                    fromAction: context => context.Source.HasSpecialDiet = context.Target.HasSpecialDiet
                )
                .ForField
                (
                    fieldName: "animalSpecialneeds",
                    toAction: context => context.Target.HasSpecialNeeds = context.Source.HasSpecialNeeds ?? false,
                    fromAction: context => context.Source.HasSpecialNeeds = context.Target.HasSpecialNeeds
                )
                .ForField
                (
                    fieldName: "animalSpecialneedsDescription",
                    toAction: context => context.Target.SpecialNeedsDescription = context.Source.SpecialNeedsDescription,
                    fromAction: context => context.Source.SpecialNeedsDescription = context.Target.SpecialNeedsDescription
                )
                .ForField
                (
                    fieldName: "animalRescueID",
                    toAction: context => context.Target.RescueID = context.Source.RescueID,
                    fromAction: context => context.Source.RescueID = context.Target.RescueID
                )
                .ForField
                (
                    fieldName: "animalUpdatedDate",
                    toAction: context => context.Target.RescueGroupsLastUpdated = DateTimeToDateTimeOffset(context.Source.LastUpdated)
                )
                .ForField
                (
                    fieldName: "animalReceivedDate",
                    toAction: context => context.Target.ReceivedDate = context.Source.ReceivedDate
                )
                .ForField
                (
                    fieldName: "animalCreatedDate",
                    toAction: context => context.Target.RescueGroupsCreated = DateTimeToDateTimeOffset(context.Source.Created)
                )
                .ForField
                (
                    fieldName: "animalGeneralAge",
                    toAction: context => context.Target.GeneralAge = context.Source.GeneralAge,
                    fromAction: context => context.Source.GeneralAge = context.Target.GeneralAge
                )
                .ForField
                (
                    fieldName: "animalFosterID",
                    toAction: context =>
                    {
                        if (context.Source.FosterContactID != null)
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
                    fromAction: context =>
                    {
                        if (context.Target.FosterID != null)
                        {
                            context.Source.FosterContactID = context.Target.Foster.RescueGroupsID;
                        }
                        else
                        {
                            context.Source.FosterContactID = null;
                        }
                    }
                )
                .ForField
                (
                    fieldName: "animalSpeciesID",
                    fromAction: context =>
                    {
                        context.Source.Species = context.Target.Breed.Species.Name;
                    }
                )
                .ForField
                (
                    fieldName: "animalPrimaryBreedID",
                    toAction: context =>
                    {
                        if (context.Target.BreedID != context.Breed.ID)
                        {
                            context.Publisher.Publish(CritterLogEvent.Action("Changed breed from {OldBreed} to {NewBreed} for {CritterID} - {CritterName}", context.Target.Breed.IfNotNull(x => x.BreedName, "not set"), context.Breed.BreedName, context.Source.ID, context.Source.Name));
                            context.Target.ChangeBreed(context.Breed);
                        }
                    },
                    fromAction: context =>
                    {
                        context.Source.PrimaryBreedID = context.Target.Breed.RescueGroupsID;
                    }
                )
                .ForField
                (
                    fieldName: "animalColorID",
                    toAction: context =>
                    {
                        if (context.Color != null && context.Target.ColorID != context.Color.ID)
                        {
                            context.Publisher.Publish(CritterLogEvent.Action("Changed color from {OldColor} to {NewColor} for {CritterID} - {CritterName}", context.Target.Color.IfNotNull(x => x.Description, "not set"), context.Color.Description, context.Source.ID, context.Source.Name));
                            context.Target.ChangeBreed(context.Breed);
                        }
                    },
                    fromAction: context =>
                    {
                        context.Source.ColorID = context.Target.Color.IfNotNull(x => x.RescueGroupsID);
                    }
                )
                .ForField
                (
                    fieldName: "animalStatusID",
                    toAction: context =>
                    {
                        if (context.Target.StatusID != context.Status.ID)
                        {
                            context.Publisher.Publish(CritterLogEvent.Action("Changed status from {OldStatus} to {NewStatus} for {CritterID} - {CritterName}", context.Target.Status.IfNotNull(x => x.Name, "not set"), context.Status.Name, context.Source.ID, context.Source.Name));
                            context.Target.ChangeStatus(context.Status);
                        }
                    },
                    fromAction: context =>
                    {
                        context.Source.StatusID = context.Target.Status.RescueGroupsID;
                    }
                )
                .ForField
                (
                    fieldName: "animalLocationID",
                    toAction: context =>
                    {

                        if (context.Source.LocationID != null)
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
                    fromAction: context =>
                    {
                        if (context.Target.LocationID != null)
                        {
                            context.Source.LocationID = context.Target.Location.RescueGroupsID;
                        }
                        else
                        {
                            context.Source.LocationID = null;
                        }
                    }
                )
                .ForField
                (
                    fieldName: "`ColorID",
                    toAction: context =>
                    {
                        if (context.Source.ColorID != null)
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
                    fromAction: context =>
                    {
                    }
                );
        }
    }
}
