using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public IActionResult Index(HomePageDTO? author = null)
        {
            if (author is null) author = new() { Handle = "" };
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Handle", "Passwd")] HomePageDTO author)
        {
            if (ModelState.IsValid)
            {
                if (Utils.HandleIsIllegal(author.Handle))
                {
                    //Handle is invalid
                    _logger.LogInformation($"Register attempt with {author.Handle}, illegal name");

                    author.Error = true;
                    author.ErrorMessage = $"Handle {author.Handle} is not allowed";

                    return View("Index", author);
                }

                if (_context.Authors.Where(a => a.Handle == author.Handle).ToList().Count > 0)
                {
                    //Handle already exists
                    _logger.LogInformation($"Register attempt with {author.Handle} failed, taken");

                    author.Error = true;
                    author.ErrorMessage = $"Handle {author.Handle} is taken";

                    return View("Index", author);
                }

                //if checking password integrity, check here
                if (author.Passwd == "")
                {
                    //Handle already exists
                    _logger.LogInformation($"Register attempt with blank password, invalid");

                    author.Error = true;
                    author.ErrorMessage = $"Blank passwords are invalid.";

                    return View("Index", author);
                }
                author.Passwd = Utils.HashPasswd(author.Passwd);

                //register new user
                var newUserEntry = _context.Authors.Add(author.ToAuthor());
                await _context.SaveChangesAsync();

                //refresh data so that the newUser has the auto incremented ID
                await newUserEntry.ReloadAsync();

                var newUser = new HomePageDTO(newUserEntry.Entity) { NewlyRegistered = true };

                return View("Index", newUser);
            }

            return View("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Clear the current context's User (otherwise logout doesn't happen until a refresh)
            HttpContext.User = new ClaimsPrincipal();

            var blankUser = new HomePageDTO();
            return View("Index", blankUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("Handle,Passwd")] HomePageDTO author)
        {
            if (ModelState.IsValid)
            {
                Author? user = AuthenticateUser(author.Handle, author.Passwd);

                if (user is null)
                {
                    _logger.LogInformation($"Login attempt with Author {author.Handle} failed");

                    author.Error = true;
                    author.ErrorMessage = "Handle or Password mistaken.";
                    return View("Index", author);
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
                    IsPersistent = false,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    RedirectUri = Url.Action("Index", "PostIts")
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"Author {user.Handle} logged in at {DateTime.UtcNow}");

                return RedirectToAction("Index", "PostIts");
            }

            return View("Index", author);
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
            string pwHash = Utils.HashPasswd(pw);
            return _context.Authors.Where(a => a.Handle == handle && a.Passwd == pwHash).FirstOrDefault();
        }
    }
}