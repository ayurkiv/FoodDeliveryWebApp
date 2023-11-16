using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
