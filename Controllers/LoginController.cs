using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        private const string SECRET = "agahkasdadluh!@asionm,cjvha!&^#a(wuhddj@nm,!#kjvlkl'l;la'v14125nljash";
        private readonly DatabaseContext _context;

        public LoginController(DatabaseContext context)
        {
            _context = context;
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

            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>();

            switch(userTemp.Role.RoleName)
            {
                case "USER":
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.USER)));
                    break;
                case "MODERATOR":
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.MODERATOR)));
                    break;
                case "ADMIN":
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.ADMIN)));
                    break;
            }

            var token = new JwtSecurityToken(
                issuer: "INO",
                audience: userTemp.Login.ToString(),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials,
                claims: claims
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(tokenString);

            HttpContext.Session.SetString("LOGIN", jwtToken.Audiences.ToArray()[0]);
            HttpContext.Session.SetString("TOKEN", tokenString);
            HttpContext.Session.SetString("ROLE", jwtToken.Claims.First(x => x.Type.ToString().Equals(ClaimTypes.Role)).Value);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
