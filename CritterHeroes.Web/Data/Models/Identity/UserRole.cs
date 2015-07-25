using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Data.Models.Identity
{
    public class UserRole
    {
        public const string MasterAdmin = "MasterAdmin";
        public const string Admin = "Admin";
        public const string Volunteer = "Volunteer";
        public const string Foster = "Foster";
        public const string Applicant = "Applicant";

        public static IEnumerable<string> GetAll()
        {
            yield return MasterAdmin;
            yield return Admin;
            yield return Volunteer;
            yield return Foster;
            yield return Applicant;
        }
    }
}