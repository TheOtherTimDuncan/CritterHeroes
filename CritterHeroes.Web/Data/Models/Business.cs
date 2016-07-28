using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Data.Models
{
    public class Business : BaseContact
    {
        public Business()
            : base()
        {
            Groups = new HashSet<BusinessGroup>();
            PhoneNumbers = new HashSet<BusinessPhone>();
        }

        public string Name
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public virtual ICollection<BusinessGroup> Groups
        {
            get;
            private set;
        }

        public virtual ICollection<BusinessPhone> PhoneNumbers
        {
            get;
            private set;
        }

        public void AddGroup(int groupID)
        {
            BusinessGroup businessGroup = new BusinessGroup(this, groupID);
            Groups.Add(businessGroup);
        }

        public void AddGroup(Group group)
        {
            BusinessGroup businessGroup = new BusinessGroup(this, group);
            Groups.Add(businessGroup);
        }

        public BusinessPhone AddPhoneNumber(string phoneNumber, string phoneExtension, int phoneTypeID)
        {
            BusinessPhone personPhone = new BusinessPhone(this, phoneTypeID, phoneNumber, phoneExtension);
            PhoneNumbers.Add(personPhone);
            return personPhone;
        }

        public BusinessPhone AddPhoneNumber(string phoneNumber, string phoneExtension, PhoneType phoneType)
        {
            BusinessPhone personPhone = new BusinessPhone(this, phoneType, phoneNumber, phoneExtension);
            PhoneNumbers.Add(personPhone);
            return personPhone;
        }
    }
}
