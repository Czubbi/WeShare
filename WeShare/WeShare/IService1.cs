﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WeShare
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class User
    {
        [DataMember]
        public string CPR { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PassKey { get; set; }
        [DataMember]
        List<int> Allergies { get; set; }
    }

    [DataContract]
    public class Food
    {
        [DataMember]
        public DateTime ExpDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string PhotoPath { get; set; }
        [DataMember]
        public string GuidLine { get; set; }
        [DataMember]
        List<int> Allergies { get; set; }
    }
}
