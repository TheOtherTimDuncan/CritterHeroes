using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Identity
{
    public class AppUser : IUser
    {
        private List<IdentityRole> _roles = new List<IdentityRole>();
        private string _email;

        public AppUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public AppUser(string userID, string email)
            : this(email)
        {
            this.Id = userID;
        }

        public AppUser(string email)
            : this()
        {
            ThrowIf.Argument.IsNull(email, "email");
            this._email = email;
        }

        public string Id
        {
            get;
            private set;
        }

        public string UserName
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public string NewEmail
        {
            get;
            set;
        }

        public bool IsEmailConfirmed
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public IEnumerable<IdentityRole> Roles
        {
            get
            {
                return _roles;
            }
        }

        public void AddRole(IdentityRole role)
        {
            _roles.Add(role);
        }

        public void RemoveRole(IdentityRole role)
        {
            _roles.Remove(role);
        }
    }
}
