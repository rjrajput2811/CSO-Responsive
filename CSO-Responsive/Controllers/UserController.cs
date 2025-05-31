using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class UserController : Controller
    {
        public IActionResult User()
        {
            return View();
        }

        public IActionResult UserDetail()
        {
            return View();
        }
    }
}
