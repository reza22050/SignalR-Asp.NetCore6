using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SignalRWebApplication.Context;
using SignalRWebApplication.Models;
using SignalRWebApplication.Models.Entities;
using System.Diagnostics;
using System.Security.Claims;

namespace SignalRWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;

        public HomeController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var loginUser = _context.Users.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if (loginUser != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties()
                {
                    RedirectUri = Url.Content("/support")
                };

                return SignIn(new ClaimsPrincipal(identity), properties, CookieAuthenticationDefaults.AuthenticationScheme);

            }
            else
            {
                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}