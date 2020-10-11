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

        #region VIEW SECTION
        public IActionResult Index(RoleTypes roleType, int pageIndex)
        {
            if (pageIndex <= 0) pageIndex = 1;

            ViewBag.RoleType = roleType;
            ViewBag.PageIndex = pageIndex;

            var results = _context.Users.Where(u => u.Role.Equals(roleType))
                                        .Skip((pageIndex - 1) * 10)
                                        .Take(10)
                                        .ToList();
            return View(results);
        }

        public IActionResult BanManager(int userId, int pageIndex, RoleTypes roleType)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user.IsBanned) ViewBag.Action = "Unban";
            else ViewBag.Action = "Ban";

            ViewBag.PageIndex = pageIndex;
            ViewBag.RoleType = roleType;

            return View(user);
        }

        [RoleRequirement("ADMIN")]
        public IActionResult ChangeRole(int userId, int pageIndex)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            ViewBag.PageIndex = pageIndex;

            return View(user);
        }
        #endregion

        #region POST SECTION
        [HttpPost]
        public async Task<IActionResult> Ban([Bind("Id, BanExpireIn, BanReason")] UserModel user, int pageIndex)
        {
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            tempUser.IsBanned = true;
            tempUser.BanExpireIn = user.BanExpireIn;
            tempUser.BanReason = user.BanReason;

            _context.Entry(tempUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Management", new { roleType = tempUser.Role, pageIndex });
        }

        [HttpPost]
        public async Task<IActionResult> Unban([Bind("Id")] UserModel user, int pageIndex)
        {
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            tempUser.IsBanned = false;
            tempUser.BanExpireIn = null;
            tempUser.BanReason = "";

            _context.Entry(tempUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Management", new { roleType = tempUser.Role, pageIndex });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole([Bind("Id, Role")] UserModel user, int pageIndex)
        {
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            var roleHolder = tempUser.Role;

            tempUser.Role = user.Role;

            _context.Entry(tempUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Management", new { roleType = roleHolder, pageIndex });
        }
        #endregion
    }
}
