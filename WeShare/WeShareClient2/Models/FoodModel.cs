using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeShareClient2.Models
{
    public class FoodModel
    {
        public DateTime ExpDate { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string Guid { get; set; }
        public int[] Allergies { get; set; }
        public string Email { get; set; }
    }
}