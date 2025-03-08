using Microsoft.AspNetCore.Mvc;

namespace aspMVCpr.Controllers
{
    public class LabController : Controller
    {
        [HttpGet]
        public IActionResult Info()
        {
            var labInfo = new
            {
                LabNumber = "1",
                Theme = "Вступ до ASP.NET Core",
                Goal = "Ознайомитися з основними принципами роботи .NET, навчитися налаштовувати середовище розробки та встановлювати необхідні компоненти, набути навичок створення рішень та проектів різних типів, набути навичок обробки запитів з використанням middleware.",
                Name = "Богдан",
                Surname = "Сапацінський"
            };

            return View(labInfo);
        }
    }
}
