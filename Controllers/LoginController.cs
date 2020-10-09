using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;

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
            var userTemp = new UserModel();

            try 
            { 
                userTemp = await _context.Users.Where(u => u.Login == user.Login).FirstAsync(); 
            }
            catch 
            {
                ViewBag.LoginError = "User not exists";
                return View("Index"); 
            }

            if (!Encryptor.IsPasswordCorrect(user.Password, userTemp.Password))
            {
                ViewBag.PasswordError = "Wrong password";
                return View("Index");
            }

            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>();

            switch(userTemp.Role)
            {
                case RoleTypes.USER:
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.USER)));
                    break;
                case RoleTypes.MODERATOR:
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.MODERATOR)));
                    break;
                case RoleTypes.ADMIN:
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.ADMIN)));
                    break;
            }

            var token = new JwtSecurityToken(
                issuer: "INO",
                audience: userTemp.Login.ToString(),
                expires: DateTime.Now.AddMinutes(int.Parse(_expireTimeInMinutes)),
                signingCredentials: signingCredentials,
                claims: claims
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(tokenString);

            HttpContext.Session.SetString("LOGIN", jwtToken.Audiences.ToArray()[0]);
            HttpContext.Session.SetString("NICKNAME", userTemp.Nickname);
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
