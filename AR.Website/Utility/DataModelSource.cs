using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AR.Domain.Contracts;
using AR.Domain.Handlers.DataStatus;
using TOTD.Utility.Misc;

namespace AR.Website.Utility
{
    public class DataModelSource : Enumeration<DataModelSource>
    {
        public static readonly DataModelSource AnimalStatus = new DataModelSource(0, "Animal Status", () => DataStatusHandlerFactory.AnimalStatus());

        private static class DataStatusHandlerFactory
        {
            public static IDataStatusHandler AnimalStatus()
            {
                return new AnimalStatusStatusHandler();
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