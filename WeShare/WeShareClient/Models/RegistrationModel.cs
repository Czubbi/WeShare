using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeShareClient.Models
{
    public class RegistrationModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string FileName { get; set; }

        public string Guid { get; set; }
    }
}