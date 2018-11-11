using System;
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
    public interface IWeShareService
    {

        [OperationContract]
        List<UserModel> GetAllUsers();
        [OperationContract]
        UserModel GetUserByCPR(string cpr);
        [OperationContract]
        UserModel GetUserByEmail(string email);
        [OperationContract]
        int AddUser(UserModel user);
        [OperationContract]
        int DeleteUser(UserModel user);
        [OperationContract]
        int AddFood(FoodModel food,string email);
        [OperationContract]
        int TakeFood(FoodModel food, string email);
        [OperationContract]
        List<string> GetAllAllergies();
        [OperationContract]
        string[] GetPasswordKey(string cpr);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class UserModel
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
        public string Password { get; set; }
        [DataMember]
        public string GuidLine { get; set; }
        [DataMember]
        public List<int> Allergies { get; set; }
    }

    [DataContract]
    public class FoodModel
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
        public List<int> Allergies { get; set; }


    }
}
