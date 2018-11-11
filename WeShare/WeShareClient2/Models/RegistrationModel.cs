using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeShareClient2.Models
{
    public class RegistrationModel
    {
        public string CPR { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FileName { get; set; }
        public string Guid { get; set; }
        public List<string> Allergies { get; set; }
        public int[] SelectedAllergies { get; set; }
    }
}