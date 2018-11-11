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
    public class FoodController : Controller
    {
        private readonly IWeShareService _proxy;

        public FoodController(IWeShareService proxy)
        {
            this._proxy = proxy;
        }
        // GET: Food
        public ActionResult Index()
        {
            return View();
        }

        // GET: Food/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Food/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Food/Create
        [HttpPost]
        public ActionResult Create(Models.FoodModel food)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Create", food);

                _proxy.AddFood(new ServiceReference1.FoodModel { ExpDate = food.ExpDate, Description = food.Description, PhotoPath = food.PhotoPath, GuidLine = food.Guid, Allergies = food.Allergies }, food.Email);
               

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Food/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Food/Edit/5
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

        // GET: Food/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Food/Delete/5
        [HttpPost]
        public ActionResult Delete(Models.FoodModel food)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index");

                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
