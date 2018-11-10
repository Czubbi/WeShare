using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Security;

namespace WeShare
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WeShareService : IWeShareService
    {
        DatabaseClassesDataContext db = new DatabaseClassesDataContext();
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
            var userToInsert = new User { Address = user.Address, City = user.City, CPR = user.CPR, Email = user.CPR, LastName = user.LastName, FirstName = user.FirstName, GuidLine = user.GuidLine, ZipCode = user.ZipCode,PassKey=passkey };
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
            }
            catch (Exception e)
            {
                return 0;
            }
            return 1;
        }
    }
}
