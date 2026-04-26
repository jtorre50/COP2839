using Microsoft.AspNetCore.Mvc;
using TempManager.Models;

namespace TempManager.Controllers
{
    public class HomeController : Controller
    {
        private TempManagerContext data { get; set; }
        public HomeController(TempManagerContext ctx) => data = ctx;

        public ViewResult Index()
        {
            var temps = data.Temps.OrderBy(t => t.Date).ToList();
            return View(temps);
        }

        [HttpGet]
        public ViewResult Add() => View(new Temp());

        [HttpPost]
        public IActionResult Add(Temp temp)
        {
            if (ModelState.IsValid)
            {
                var existingTemp = data.Temps.FirstOrDefault(t => t.Date == temp.Date);
                if (existingTemp != null)
                {
                    ModelState.AddModelError("Date", "Date already exists in the database.");
                    return View(temp);
                }

                data.Temps.Add(temp);
                data.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please correct all errors");
                return View(temp);
            }
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            var temp = data.Temps.Find(id);
            return View(temp);
        }

        [HttpPost]
        public RedirectToActionResult Delete(Temp temp)
        {
            data.Remove(temp);
            data.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}