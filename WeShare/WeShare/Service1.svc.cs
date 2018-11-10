﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WeShare
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WeShareService : IWeShareService
    {
        DatabaseClassesDataContext db = new DatabaseClassesDataContext();
        public int AddFood(FoodModel food,string cpr)
        {
            var foods = db.Foods;
            var user = db.Users.SingleOrDefault(x => x.CPR == cpr);
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
                return 1;
            }
        }

        public int AddUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public int DeleteUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserByCPR(string cpr)
        {
            throw new NotImplementedException();
        }

        public int TakeFood(FoodModel food)
        {
            throw new NotImplementedException();
        }
    }
}
