using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WeShareClient2.Models;
using WeShareClient2.ServiceReference1;

namespace WeShareClient2.Controllers
{
    public class UserController : Controller
    {

        private readonly IWeShareService _proxy;

        public UserController(IWeShareService proxy)
        {
            _proxy = proxy;
        }

        // GET: User
        public ActionResult Index()
        {

            if (Request.Cookies.Get("login") == null)
            {
                return View();
            }
            else
            {
                string userName = Request.Cookies.Get("login").Values["feketePorzeczka"];
                UsernameModel login = new UsernameModel { Username = userName };
                return RedirectToAction("LoggedIn", login);

            }

            
        }

        [HttpPost]
        public ActionResult Index(LoginModel userCred)
        {
            if (!ModelState.IsValid)
                return View(userCred);

            
            var dbUser = _proxy.GetAllUsers().SingleOrDefault(x => x.Email == userCred.Username);

            if (dbUser == null)
            {
                return View(userCred);
            }

            
            var passAndKey = _proxy.GetPasswordKey(dbUser.CPR);
            string hashString = Hash(passAndKey[1]);
            if (passAndKey[0] == userCred.PassKey)
            {
                UsernameModel userToPass = new UsernameModel { Username = userCred.Username };

                HttpCookie cookie = new HttpCookie("login");
                cookie.Values.Add("feketePorzeczka", userToPass.Username);
                cookie.Values.Add("pirosPorzeczka", hashString);
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(cookie);
                return RedirectToAction("LoggedIn", userToPass);
            }
            else return View(userCred);

        }

        public ActionResult LoggedIn(UsernameModel user)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var dbUser = _proxy.GetAllUsers().SingleOrDefault(x => x.Email == user.Username);
            RegistrationModel fullUser = new RegistrationModel { CPR = dbUser.CPR, FirstName = dbUser.FirstName, LastName = dbUser.LastName, Address = dbUser.Address, ZipCode = dbUser.ZipCode, City = dbUser.City, Email = dbUser.Email, SelectedAllergies = dbUser.Allergies, Password = dbUser.Password };

            if (Request.Cookies.Get("login") != null)
            {
                if (Hash(fullUser.Password) == Request.Cookies.Get("login").Values["pirosPorzeczka"])
                {
                    return View("LoggedIn", fullUser);
                }
                else return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");

        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            RegistrationModel regModel = new RegistrationModel { Allergies = _proxy.GetAllAllergies().ToList() };
            return View(regModel);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(RegistrationModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Create", user);
                // TODO : Uploading files to webserver 
                var guidHandler = Guid.NewGuid();
                string guid = guidHandler.ToString();
                user.Address = $"{user.AddressStreet} {user.AddressNumber}";
                _proxy.AddUser(new UserModel { CPR = user.CPR, FirstName = user.FirstName, LastName = user.LastName, Address = user.Address, ZipCode = user.ZipCode, City = user.City, Email = user.Email, Allergies = user.SelectedAllergies, GuidLine = guid,Password=user.Password});

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("login");
            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public string Hash(string input)
        {
            MD5 md = MD5.Create();

            byte[] asciiBytes = Encoding.ASCII.GetBytes(input);

            byte[] hashString = md.ComputeHash(asciiBytes);

            StringBuilder sb = new StringBuilder();

            for(int i=0; i< hashString.Length; i++)
            { 
                 sb.Append(hashString[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
