using System;
using System.Collections.Generic;
using System.IO;
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
            Models.FoodModel foodModel = new Models.FoodModel{Allergies = _proxy.GetAllAllergies().ToList()};
            return View(foodModel);
        }

        // POST: Food/Create
        [HttpPost]
        public ActionResult Create(Models.FoodModel food)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Create", food);
                string fileName = food.files[0].FileName;
                string guid = Guid.NewGuid().ToString();
                System.IO.Directory.CreateDirectory(Server.MapPath($"~/App_Data/{guid}"));
                food.files[0].SaveAs(Server.MapPath($"~/App_Data/{guid}/{fileName}"));
                _proxy.AddFood(new ServiceReference1.FoodModel { ExpDate = food.ExpDate, Description = food.Description, PhotoPath = fileName, GuidLine = guid, Allergies = food.SelectedAllergies }, Request.Cookies.Get("login").Values["feketePorzeczka"]);
               

                return RedirectToAction("Index","User","");
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
        
        //GET
        public ActionResult Take()
        {
            var foodModels = _proxy.GetAllFoods().Select(x=>new Models.FoodModel{
                SelectedAllergies=x.Allergies,
                Description=x.Description,
                ExpDate=x.ExpDate,
                Guid=x.GuidLine,
                PhotoPath=x.PhotoPath
            });
            return View(foodModels);
        }


        public ActionResult TakeFood(Models.FoodModel model)
        {
            var model2 = new ServiceReference1.FoodModel { GuidLine = model.Guid };
            var email = Request.Cookies.Get("login").Values["feketePorzeczka"];
            _proxy.TakeFood(model2,email);
            return RedirectToAction("Take");
        }

        public ActionResult Download(string guid, string file)
        {
            string path = Server.MapPath($"~/App_Data/{guid}/{file}");
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] fileBytes = new byte[fileStream.Length];
            fileStream.CopyTo(mem);
            fileBytes = mem.ToArray();
            string fileName = file;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
