using Microsoft.AspNetCore.Mvc;

namespace CSO_Responsive.Controllers;

public class DashBoardController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
