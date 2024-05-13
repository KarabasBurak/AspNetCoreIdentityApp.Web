using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            message = "Bu sayfaya erişim yetkiniz yoktur";
            ViewBag.message=message;
            return View();
        }
    }
}
