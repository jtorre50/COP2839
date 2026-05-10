using Microsoft.AspNetCore.Mvc;
using ClassSchedule.Models;

namespace ClassSchedule.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Class> classes { get; set; }
        private IRepository<Day> days { get; set; }

        public HomeController(IRepository<Class> classes, IRepository<Day> days)
        {
            this.classes = classes;
            this.days = days;
        }

        public ViewResult Index(int id)
        {
            var dayOptions = new QueryOptions<Day> {
                OrderBy = d => d.DayId
            };

            var classOptions = new QueryOptions<Class> {
                Includes = "Teacher, Day"
            };

            if (id == 0) {
                classOptions.OrderBy = c => c.DayId;
                classOptions.ThenOrderBy = c => c.MilitaryTime;
            }
            else {
                classOptions.Where = c => c.DayId == id;
                classOptions.OrderBy = c => c.MilitaryTime;
            }

            var dayList = days.List(dayOptions);
            var classList = classes.List(classOptions);

            ViewBag.Id = id;
            ViewBag.Days = dayList;
            return View(classList);
        }
    }
}