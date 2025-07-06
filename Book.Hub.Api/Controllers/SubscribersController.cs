using Microsoft.AspNetCore.Mvc;

namespace Books.Hub.Api.Controllers
{
    public class SubscribersController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
