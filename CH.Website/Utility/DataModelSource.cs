using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CH.Domain.Contracts;
using CH.Domain.Handlers.DataStatus;
using TOTD.Utility.Misc;

namespace CH.Website.Utility
{
    public class DataModelSource : Enumeration<DataModelSource>
    {
        public static readonly DataModelSource AnimalStatus = new DataModelSource(0, "Animal Status", () => DataStatusHandlerFactory.AnimalStatus());
        public static readonly DataModelSource AnimalBreed = new DataModelSource(1, "Animal Breeds", () => DataStatusHandlerFactory.AnimalBreed());

        private static class DataStatusHandlerFactory
        {
            public static IDataStatusHandler AnimalStatus()
            {
                return new AnimalStatusStatusHandler();
            }

            public static IDataStatusHandler AnimalBreed()
            {
                return new BreedStatusHandler();
            }
        }

        private DataModelSource()
        {
        }

        private DataModelSource(int value, string displayName, Func<IDataStatusHandler> statusHandler)
            : base(value, displayName)
        {
            this.GetDataStatusHandler = statusHandler;
        }

        private Func<IDataStatusHandler> GetDataStatusHandler;

        public IDataStatusHandler StatusHandler
        {
            get
            {
                return GetDataStatusHandler();
            }
        }

        public string Title
        {
            get
            {
                return DisplayName;
            }
        }
    }
}