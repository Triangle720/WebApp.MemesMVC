using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;

namespace WebApp.MemesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region VIEW REGION
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [RoleRequirement("ADMIN,MODERATOR")]
        public IActionResult Management()
        {
            return View();
        }

        [RoleRequirement("ADMIN,MODERATOR,USER")]
        public IActionResult Profile()
        {
            var user = _context.Users.Where(u => u.Login == HttpContext.Session.GetString("LOGIN")).FirstOrDefault();
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region POST REGION
        [RoleRequirement("ADMIN,MODERATOR,USER")]
        [HttpPost]
        public async Task<IActionResult> ProfilePasswordChange(int userId, string password, string newPassword)
        {
            var tempUser = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (!Encryptor.IsPasswordCorrect(password, tempUser.Password))
            {
                ViewBag.Error = "Wrong password";
                return View("Profile", tempUser);
            }
            else if(password == newPassword)
            {
                ViewBag.Error = "Passwords are not different";
                return View("Profile", tempUser);
            }

            tempUser.Password = Encryptor.EncryptPassword(newPassword);
            _context.Entry(tempUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction("Logout", "Login");
        }
        #endregion
    }
}
