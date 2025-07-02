using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
