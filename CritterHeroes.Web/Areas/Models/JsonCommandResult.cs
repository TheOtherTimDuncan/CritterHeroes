using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Areas.Models
{
    public class JsonCommandResult
    {
        public static JsonCommandResult Success()
        {
            return new JsonCommandResult()
            {
                Succeeded = true
            };
        }

        public static JsonCommandResult Error(string message)
        {
            return new JsonCommandResult()
            {
                Succeeded = false,
                Message = message
            };
        }

        public bool Succeeded
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }
}