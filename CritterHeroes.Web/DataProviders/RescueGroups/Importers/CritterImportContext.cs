using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Importers
{
    public class CritterImportContext
    {
        public CritterImportContext(CritterSearchResult source, Critter target, IAppEventPublisher publisher)
        {
            this.Source = source;
            this.Target = target;
            this.Publisher = publisher;
        }

        public CritterSearchResult Source
        {
            get;
        }

        public Critter Target
        {
            get;
            set;
        }

        public IAppEventPublisher Publisher
        {
            get;
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
}
