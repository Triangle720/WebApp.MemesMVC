using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;

namespace WebApp.MemesMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DatabaseContext _context;

        public RegisterController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Login, Password, Email")] UserModel user)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    if (_context.Users.Any(u => u.Email == user.Email))
                    {
                        ViewBag.EmailAssigned = "Email already assigned";
                        goto case false;
                    }
                    else if (_context.Users.Any(u => u.Email == user.Email || u.Login == user.Login))
                    {
                        ViewBag.UserAlreadyExists = "User already exists";
                        goto case false;
                    }

                    user.Password = Encryptor.EncryptPassword(user.Password);
                    user.AccountCreationTime = DateTime.Now;
                    user.Role = await _context.Roles.Where(r => r.RoleName == Enum.GetName(typeof(RoleTypes), RoleTypes.USER)).FirstAsync();
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Login");

                case false:
                    return View("Index");
            }
        }
    }
}
