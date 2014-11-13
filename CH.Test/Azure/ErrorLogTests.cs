using System;
using System.Collections.Generic;
using System.Configuration;
using CH.Azure.Logging;
using Elmah;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CH.Test.Azure
{
    [TestClass]
    public class ErrorLogTests
    {
        [TestMethod]
        public void CanGetConnectionStringUsingConnectionStringName()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config["connectionStringName"] = "test";
            AzureErrorLog errorLog = new AzureErrorLog(config);
            Assert.AreEqual(ConfigurationManager.ConnectionStrings["test"].ConnectionString, errorLog.ConnectionString);
        }

        [TestMethod]
        public void CanGetConnectionStringFromConfig()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config["connectionString"] = "test";
            AzureErrorLog errorLog = new AzureErrorLog(config);
            Assert.AreEqual(config["connectionString"], errorLog.ConnectionString);
        }

        [TestMethod]
        public void CanGetConnectionStringFromAppSettings()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config["connectionStringAppKey"] = "TestKey";
            AzureErrorLog errorLog = new AzureErrorLog(config);
            Assert.AreEqual(ConfigurationManager.AppSettings["TestKey"], errorLog.ConnectionString);
        }

        [TestMethod]
        public void CanSaveAndRetrieveErrors()
        {
            AzureErrorLog errorLog = new AzureErrorLog(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            Exception exception = new DivideByZeroException("Can't divide by 0");
            Error error = new Error(exception);
            string errorID = errorLog.Log(error);
            Console.WriteLine(errorID);
            ErrorLogEntry entry = errorLog.GetError(errorID);
            Assert.AreEqual(error.HostName, entry.Error.HostName);
            Assert.AreEqual(error.Message, entry.Error.Message);
            Assert.AreEqual(error.Source, entry.Error.Source);
            Assert.AreEqual(error.StatusCode, entry.Error.StatusCode);
            Assert.AreEqual(error.Type, entry.Error.Type);
            Assert.AreEqual(error.User, entry.Error.User);

            List<ErrorLogEntry> entries = new List<ErrorLogEntry>();
            int count = errorLog.GetErrors(0, 5, entries);
            Assert.IsTrue(entries.Any(x => x.Id == errorID));
        }
    }
}
