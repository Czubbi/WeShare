using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            this._proxy = proxy;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
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
                _proxy.AddUser(new UserModel { CPR = user.CPR, FirstName = user.FirstName, LastName = user.LastName, Address = user.Address, ZipCode = user.ZipCode, City = user.City, Email = user.Email, Allergies = user.Allergies, GuidLine = user.Guid });

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
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
    }
}
