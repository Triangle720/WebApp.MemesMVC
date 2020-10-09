using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;

namespace WebApp.MemesMVC.Controllers
{
    [RoleRequirement("ADMIN,MODERATOR")]
    public class ManagementController : Controller
    {
        private readonly DatabaseContext _context;

        public ManagementController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.PageIndex = 1;
            return View();
        }

        public IActionResult UsersManager(RoleTypes roleType, int pageIndex)
        {
            if (pageIndex <= 0) pageIndex = 1;

            ViewBag.PageIndex = pageIndex;
            var results = _context.Users.Where(u => u.Role.Equals(roleType)).Skip((pageIndex - 1) * 10).Take(10).ToList();
            return View(results);
        }

        public IActionResult BanUser(int userId)
        {
            return View();
        }
    }
}
