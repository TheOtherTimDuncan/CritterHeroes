using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Identity
{
    public class IdentityUser : IUser
    {
        private List<IdentityRole> _roles = new List<IdentityRole>();

        public IdentityUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public IdentityUser(string userID, string userName)
            : this(userName)
        {
            this.Id = userID;
        }

        public IdentityUser(string username)
            : this()
        {
            ThrowIf.Argument.IsNull(username, "username");
            this.UserName = username;
        }

        public string Id
        {
            get;
            private set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public string Email
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
