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
            var tempUser = new UserModel();

            try 
            {
                tempUser = await _context.Users.Where(u => u.Login == user.Login).FirstAsync(); 
            }
            catch 
            {
                ViewBag.Error = "Wrong username or password";
                return View("Index"); 
            }

            if (!Encryptor.IsPasswordCorrect(user.Password, tempUser.Password))
            {
                ViewBag.Error = "Wrong username or password";
                return View("Index");
            }
            else if(tempUser.IsBanned)
            {
                if (tempUser.BanExpireIn <= DateTime.Now)
                {
                    tempUser.IsBanned = false;
                    tempUser.BanExpireIn = null;
                    tempUser.BanReason = "";

                    _context.Entry(tempUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Error = "Your account is banned. Ban expires in: " + tempUser.BanExpireIn;
                    return View("Index");
                }
            }

            var token = await JWTManager.AssignToken(tempUser, _secret, _expireTimeInMinutes);

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
    }
}
