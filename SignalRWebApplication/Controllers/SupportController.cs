using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalRWebApplication.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
