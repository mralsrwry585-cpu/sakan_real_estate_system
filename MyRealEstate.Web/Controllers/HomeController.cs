using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyRealEstate.Web.Controllers
{
    /// <summary>
    /// Landing/error pages used by conventional routing and the exception handler.
    /// Fixes the root "/" navigation and the "/Home/Error" exception-handler target
    /// (both previously returned 404 because no Home controller existed).
    /// </summary>
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/owner/dashboard");

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        [AllowAnonymous]
        public IActionResult Error() => View();
    }
}