using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeShareClient2.Models
{
    public class FoodModel
    {
        [Display(Name = "The Display Name"),DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy H:mm:ss tt}"), DataType(DataType.DateTime)]
        public DateTime ExpDate { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string Guid { get; set; }
        public List<string> Allergies { get; set; }
        public int[] SelectedAllergies { get; set; }
        public string Email { get; set; }
        public HttpPostedFileBase[] files { get; set; }
    }
}