using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class BusinessPhone
    {
        protected BusinessPhone()
        {
        }

        internal BusinessPhone(Business business, int phoneTypeID, string phoneNumber, string phoneExtension)
            : this(business, phoneNumber, phoneExtension)
        {
            this.PhoneType = null;
            this.PhoneTypeID = phoneTypeID;
        }

        internal BusinessPhone(Business business, PhoneType phoneType, string phoneNumber, string phoneExtension)
            : this(business, phoneNumber, phoneExtension)
        {
            if (phoneType != null)
            {
                this.PhoneType = phoneType;
                this.PhoneTypeID = phoneType.ID;
            }
        }

        private BusinessPhone(Business business, string phoneNumber, string phoneExtension)
        {
            ThrowIf.Argument.IsNull(business, nameof(business));
            ThrowIf.Argument.IsNullOrEmpty(phoneNumber, nameof(phoneNumber));

            this.Business = business;
            this.BusinessID = business.ID;

            this.PhoneNumber = phoneNumber;
            this.PhoneExtension = phoneExtension;
        }

        public int ID
        {
            get;
            private set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        public string PhoneExtension
        {
            get;
            set;
        }

        public int? PhoneTypeID
        {
            get;
            private set;
        }

        public virtual PhoneType PhoneType
        {
            get;
            private set;
        }

        public int BusinessID
        {
            get;
            private set;
        }

        public virtual Business Business
        {
            get;
            private set;
        }

        public void ChangePhoneType(PhoneType phoneType)
        {
            ThrowIf.Argument.IsNull(phoneType, nameof(phoneType));

            this.PhoneType = phoneType;
            this.PhoneTypeID = phoneType.ID;
        }
    }
}
