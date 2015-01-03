using System;
using System.Configuration;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Configuration
{
    /// <summary>
    /// Represents the repository section from an app.config or web.config
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/en-us/library/2tw134k3%28v=vs.100%29.aspx
    /// http://www.codeproject.com/Articles/16466/Unraveling-the-Mysteries-of-NET-2-0-Configuration
    /// </remarks>
    public class RescueGroupsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
        }

        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string APIKey
        {
            get
            {
                return (string)this["apiKey"];
            }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
        }

        [ConfigurationProperty("accountNumber", IsRequired = true)]
        public string AccountNumber
        {
            get
            {
                return (string)this["accountNumber"];
            }
        }
    }
}
