using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web.Security;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net.Mail;

namespace WeShare
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WeShareService : IWeShareService
    {
        DatabaseClassesDataContext db = new DatabaseClassesDataContext();
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "Gmail API .NET Quickstart";

        public int AddFood(FoodModel food,string email)
        {
            db.Connection.Open();
            var foods = db.Foods;
            var user = db.Users.SingleOrDefault(x => x.Email == email);
            var foodToInsert = new Food { Description = food.Description, ExpDate = food.ExpDate, Guid = food.GuidLine, PicPath = food.PhotoPath, UserID=user.ID };
            try
            {
                foods.InsertOnSubmit(foodToInsert);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
            var foodInDB = db.Foods.SingleOrDefault(x => x.Guid == food.GuidLine);
            if (foodInDB == null)
            {
                return 0;
            }
            else
            {
                foreach (var a in food.Allergies)
                {
                    FoodAllergy foodAllergy = new FoodAllergy { AllergyID = a, FoodID = foodInDB.ID };
                    try
                    {
                        db.FoodAllergies.InsertOnSubmit(foodAllergy);
                        db.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        return 0;
                    }
                }
                db.Connection.Close();
                return 1;
            }
        }

        public int AddUser(UserModel user)
        {
            db.Connection.Open();
            var users = db.Users;
            string passkey=Membership.GeneratePassword(user.Password.Length,0);
            var userToInsert = new User { Address = user.Address, City = user.City, CPR = user.CPR, Email = user.Email, LastName = user.LastName, FirstName = user.FirstName, GuidLine = user.GuidLine, ZipCode = user.ZipCode,PassKey=passkey };
            try
            {
                users.InsertOnSubmit(userToInsert);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
            var password = new Password { Password1 = user.Password, UserPassKey = passkey };
            try {
                db.Passwords.InsertOnSubmit(password);
                db.SubmitChanges();
            }
            catch(Exception e)
            {
                return 0;
            }
            foreach (var a in user.Allergies)
            {
                UserAllergy allergy = new UserAllergy { AllergyID = a, UserID = users.Single(x => x.CPR == user.CPR).ID };
                db.UserAllergies.InsertOnSubmit(allergy);
                db.SubmitChanges();
            }
            db.Connection.Close();
            return 1;
        }

        public int DeleteUser(UserModel user)
        {
            db.Connection.Open();
            var userToDelete = db.Users.SingleOrDefault(x => x.CPR == user.CPR);
            if (userToDelete != null)
            {
                db.Users.DeleteOnSubmit(userToDelete);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    return 0;
                }
                return 1;
            }
            else return 0;
        }

        public List<UserModel> GetAllUsers()
        {
            var listToReturn = db.Users.Select(x => new UserModel
            {
                Address = x.Address,
                Allergies = db.UserAllergies.Where(y => y.UserID == x.ID).Select(y => (int)y.AllergyID).ToList(),
                City = x.City,
                CPR = x.CPR,
                Email = x.Email,
                FirstName = x.FirstName,
                GuidLine = x.GuidLine,
                LastName = x.LastName,
                Password = x.PassKey,
                ZipCode = x.ZipCode
            }).ToList();
            return listToReturn;
        }

        public UserModel GetUserByCPR(string cpr)
        {
            var user = GetAllUsers().SingleOrDefault(x => x.CPR == cpr);
            if (user != null)
            {
                return user;
            }
            else throw new Exception("User cannot be found");
        }

        public UserModel GetUserByEmail(string email)
        {
            var user = GetAllUsers().SingleOrDefault(x => x.Email == email);
            if (user != null)
            {
                return user;
            }
            else throw new Exception("User cannot be found");
        }

        public int TakeFood(FoodModel food,string email)
        {

            Food foodTaken = db.Foods.SingleOrDefault(x => x.Guid == food.GuidLine);
            foodTaken.TakenBy = db.Users.SingleOrDefault(x => x.Email == email).ID;
            try
            {
                db.SubmitChanges();

                var msg = new AE.Net.Mail.MailMessage { Subject = "Someone just took your food!", Body = $"Someone just got interested in your offer and took the food. You can contact him/her via this email address: {email}", From = new MailAddress("piotr.gzubicki97@gmail.com") };

                msg.To.Add(new MailAddress(foodTaken.User.Email));
                msg.ReplyTo.Add(msg.From);
                var msgStr = new StringWriter();
                msg.Save(msgStr);
                UserCredential credential;
                using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }
                var gmail = new GmailService(new BaseClientService.Initializer(){ HttpClientInitializer = credential,ApplicationName = ApplicationName });
                var result = gmail.Users.Messages.Send(new Message
                {
                    Raw = Base64UrlEncode(msgStr.ToString())
                }, "me").Execute();
                
            

  
            }
            catch (Exception e)
            {
                return 0;
            }
            return 1;
        }

        private static string Base64UrlEncode(string input) {
    var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
    // Special "url-safe" base64 encode.
    return Convert.ToBase64String(inputBytes)
      .Replace('+', '-')
      .Replace('/', '_')
      .Replace("=", "");
  }
        public List<string> GetAllAllergies()
        {
            return db.Allergies.Select(x => x.Name).ToList();
        }
        public string[] GetPasswordKey(string cpr)
        {
            var user = db.Users.SingleOrDefault(x => x.CPR == cpr);
            string[] passAndKey = new string[2];
            if (user != null)
            {
                var passKeys = db.Passwords.Single(x => x.User.ID == user.ID);
                passAndKey[0] = passKeys.Password1;
                passAndKey[1] = passKeys.UserPassKey; 
            }
            return passAndKey;

        }
    }
}
