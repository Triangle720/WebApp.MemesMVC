using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Login, Password, Nickname, Email")] UserModel user)
        {
            switch (ModelState.IsValid)
            {
                case true:
                    user.Login = user.Login.ToLower();

                    if (_context.Users.Any(u => u.Email == user.Email))
                    {
                        ViewBag.EmailAssigned = "Email already assigned";
                        goto case false;
                    }
                    else if (_context.Users.Any(u => u.Login == user.Login))
                    {
                        ViewBag.UserAlreadyExists = "User already exists";
                        goto case false;
                    }
                    else if (_context.Users.Any(u => u.Nickname == user.Nickname))
                    {
                        ViewBag.NicknameAssigned = "Nickname already assigned";
                        goto case false;
                    }

                    user.Password = Encryptor.EncryptPassword(user.Password);
                    user.AccountCreationTime = DateTime.Now;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Login");

                case false:
                    return View("Index");
            }
        }
    }
}
