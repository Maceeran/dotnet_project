using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PhotosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
