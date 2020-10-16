using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;
using System;
using Castle.Core.Internal;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WebApp.MemesMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly string _secret;
        private readonly string _expireTimeInMinutes;
        
        public LoginController(DatabaseContext context, IConfiguration configuration)
        {    
            _context = context;
            _secret = configuration.GetSection("JWT").GetSection("secret").Value;
            _expireTimeInMinutes = configuration.GetSection("JWT").GetSection("expireTimeInMinutes").Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Login, Password")] UserModel user)
        {
            var tempUser = await _context.Users.Where(u => u.Login == user.Login).FirstOrDefaultAsync();
            var results = await ErrorHandler(user, tempUser);

            if (!results.IsNullOrEmpty())
            {
                ViewBag.Error = results;
                return View("Index");
            }

            var token = await JWTManager.AssignToken(tempUser);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(tokenString);

            HttpContext.Session.SetString("LOGIN", jwtToken.Audiences.ToArray()[0]);
            HttpContext.Session.SetString("NICKNAME", tempUser.Nickname);
            HttpContext.Session.SetString("TOKEN", tokenString);
            HttpContext.Session.SetString("ROLE", jwtToken.Claims.First(x => x.Type.ToString().Equals(ClaimTypes.Role)).Value);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public async Task<string> ErrorHandler(UserModel user, UserModel tempUser)
        {
            if (user.Password.IsNullOrEmpty() || user.Login.IsNullOrEmpty()) return "Fill all fields";
            else if (tempUser == null || !Encryptor.IsPasswordCorrect(user.Password, tempUser.Password)) return "Wrong username or password";
            else if (tempUser.IsBanned)
            {
                // QUARZT.net istead of this
                if (tempUser.BanExpireIn <= DateTime.Now)
                {
                    tempUser.IsBanned = false;
                    tempUser.BanExpireIn = null;
                    tempUser.BanReason = "";

                    _context.Entry(tempUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else return "Your account is banned. Ban expires in: " + tempUser.BanExpireIn;
            }

            return "";
        }
    }
}
