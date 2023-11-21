using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
