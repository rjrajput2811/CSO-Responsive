using CSO.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var model = new UserViewModel
            {
                RoleList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Admin", Value = "1" },
                    new SelectListItem { Text = "User", Value = "2" }
                },

                UserTypeList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Internal", Value = "1" },
                    new SelectListItem { Text = "Vendor", Value = "2" }
                },
            };

            return View(model);
        }
    }
}
