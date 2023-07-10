using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PostItDemo.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PostItDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostItContext _context;

        public HomeController(ILogger<HomeController> logger, PostItContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("Id,Handle,Passwd")] Author author)
        {
            if(ModelState.IsValid) {
                var user = AuthenticateUser(author.Handle, author.Passwd);

                if (user is null)
                {
                    _logger.LogInformation($"Login attempt with Author {author.Handle} failed");
                    return RedirectToAction(nameof(Index));
                }

                //create authentication cookie, courtesy https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-7.0
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Handle),
                    new Claim("Handle", user.Handle),
                    new Claim(ClaimTypes.Role, "Author"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(6),
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    RedirectUri = Url.Action("Index", "Home")
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"Author {user.Handle} logged in at {DateTime.UtcNow}");
            }
            return RedirectToAction("Index", "PostIts");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

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

        private Author? AuthenticateUser(string handle, string pw)
        {
            //This is of course purely for demo purposes
            return _context.Authors.Where(a => a.Handle == handle && a.Passwd == pw).FirstOrDefault();
        }
    }
}