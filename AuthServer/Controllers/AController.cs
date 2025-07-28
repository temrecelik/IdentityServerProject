using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class AController :Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
