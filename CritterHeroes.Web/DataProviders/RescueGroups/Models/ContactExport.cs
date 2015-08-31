using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class ContactExport
    {
        public int ID
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Contact_Active")]
        public string Active
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Contact_Firstname")]
        public string FirstName
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Contact_Firstname")]
        public string LastName
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Contact_Email")]
        public string Email
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Contact_City")]
        public string City
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Contact_State")]
        public string State
        {
            get;
            set;
        }
    }
}
