using Microsoft.AspNetCore.Mvc;

namespace IvanVGodDota2.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}