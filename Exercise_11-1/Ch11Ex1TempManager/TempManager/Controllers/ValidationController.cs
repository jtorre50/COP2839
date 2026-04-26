using Microsoft.AspNetCore.Mvc;
using TempManager.Models;

namespace TempManager.Controllers
{
    public class ValidationController : Controller
    {
        private TempManagerContext data { get; set; }
        public ValidationController(TempManagerContext ctx) => data = ctx;

        public JsonResult CheckDate(string date)
        {
            DateTime dt;
            bool parsed = DateTime.TryParse(date, out dt);

            if (!parsed)
                return Json(true);

            var temp = data.Temps.FirstOrDefault(t => t.Date == dt);
            return temp == null ? Json(true) : Json("Date already exists in the database.");
        }
    }
}