using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Account;
using MyRealEstate.Web.Models.Api;
using MyRealEstate.Web.Services;

namespace MyRealEstate.Web.Controllers
{
public class AccountController : Controller
    {
        private readonly AuthApiClient _auth;

        public AccountController(AuthApiClient auth)
        {
            _auth = auth;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/owner/dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var auth = await _auth.LoginAsync(model.Email, model.Password);

                if (auth is null)
                {
                    ModelState.AddModelError(string.Empty, "بيانات الدخول غير صحيحة.");
                    return View(model);
                }

                // Only owners may access the Owner dashboard
                if (auth.Role != Role.Owner)
                {
                    ModelState.AddModelError(string.Empty, "هذا الحساب غير مخول للوصول إلى لوحة المالك.");
                    return View(model);
                }

                StoreOwnerSession(auth);
                return RedirectToLocal(returnUrl);
            }
            catch (ApiClientException ex)
            {
                ModelState.AddModelError(string.Empty, ex.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "البريد الإلكتروني أو كلمة المرور غير صحيحة."
                    : GetFriendlyError(ex));
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "تعذر الاتصال بالخادم. حاول مرة أخرى لاحقا.");
                return View(model);
            }
        }

[HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/owner/dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.AgreeToTerms)
            {
                ModelState.AddModelError(nameof(model.AgreeToTerms), "يجب الموافقة على الشروط والأحكام.");
                return View(model);
            }

            try
            {
                var request = new RegisterRequest
                {
                    FullName = model.FullName,
                    Mobile = model.Mobile,
                    NationalId = model.NationalId,
                    Email = model.Email,
                    Password = model.Password,
                    Role = Role.Owner
                };

                var auth = await _auth.RegisterAsync(request);
                if (auth is null)
                {
                    ModelState.AddModelError(string.Empty, "تعذر إنشاء الحساب. حاول مرة أخرى.");
                    return View(model);
                }

                StoreOwnerSession(auth);
                TempData["Registered"] = "تم إنشاء حسابك بنجاح. أهلا بك في سكن!";
                return RedirectToLocal(returnUrl ?? "/owner/dashboard");
            }
            catch (ApiClientException ex)
            {
                ModelState.AddModelError(string.Empty, ex.StatusCode == System.Net.HttpStatusCode.Conflict
                    ? "البريد الإلكتروني مسجل مسبقا. جرب تسجيل الدخول."
                    : GetFriendlyError(ex));
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "تعذر الاتصال بالخادم. حاول مرة أخرى لاحقا.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Logout()
        {
            // No auth cookie to clear — signing out only abandons the server-side session.
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

[HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        [HttpGet]
        public IActionResult Error() => View();

        /// <summary>Persist the owner auth data in the server-side session only (cookie-less).</summary>
        private void StoreOwnerSession(AuthResponse auth)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.StoreAuth(auth);
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/owner/dashboard");
        }

        private static string GetFriendlyError(ApiClientException ex)
        {
            var msg = ex.Message;
            if (msg.Contains("Email already registered", System.StringComparison.OrdinalIgnoreCase))
                return "البريد الإلكتروني مسجل مسبقا.";
            if (msg.Contains("Invalid email or password", System.StringComparison.OrdinalIgnoreCase))
                return "البريد الإلكتروني أو كلمة المرور غير صحيحة.";
            return "حدث خطأ أثناء العملية. حاول مرة أخرى.";
        }
    }
}