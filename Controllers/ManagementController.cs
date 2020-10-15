using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;
using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.MemesMVC.Controllers
{
    [RoleRequirement("ADMIN,MODERATOR")]
    public class ManagementController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly DatabaseContext _context;

        public ManagementController(DatabaseContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        #region VIEW SECTION
        [AllowAnonymous]
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

        [RoleRequirement("ADMIN, MODERATOR")]
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

        [RoleRequirement("ADMIN,MODERATOR")]
        public IActionResult PictureManager(string errorMessage = "")
        {
            if (errorMessage != "") ViewBag.Error = errorMessage;
            var picture = _context.Pictures.Where(p => p.LocalPath.Contains("imgs")).FirstOrDefault();
            return View(picture);
        }
        #endregion

        #region POST SECTION
        [RoleRequirement("ADMIN,MODERATOR")]
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
        [RoleRequirement("ADMIN,MODERATOR")]
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
        [RoleRequirement("ADMIN,MODERATOR")]
        public async Task<IActionResult> ChangeRole([Bind("Id, Role")] UserModel user, int pageIndex)
        {
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            var roleHolder = tempUser.Role;

            tempUser.Role = user.Role;

            _context.Entry(tempUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Management", new { roleType = roleHolder, pageIndex });
        }

        [HttpPost]
        [RoleRequirement("ADMIN,MODERATOR")]
        public async Task<IActionResult> AcceptPicture(int pictureId)
        {
            var tempPicture = _context.Pictures.Where(p => p.Id == pictureId).FirstOrDefault();
            if (tempPicture == null || tempPicture.LocalPath.IsNullOrEmpty()) return RedirectToAction("PictureManager"); //already accepted

            using var client = _clientFactory.CreateClient("imgur");
            var byteContent = new ByteArrayContent(await System.IO.File.ReadAllBytesAsync(Path.Combine("C:\\home\\data\\Pics", tempPicture.LocalPath)));
            var response = await client.PostAsync("image", byteContent);

            var imgUrl = response.Content.ReadAsStringAsync().Result
                                                             .Split(',')
                                                             .Where(s => s.Contains("link"))
                                                             .FirstOrDefault();

            if (response.IsSuccessStatusCode)
            {   
                tempPicture.UrlAddress = imgUrl.Substring(imgUrl.IndexOf(':') + 2, imgUrl.LastIndexOf('"') - imgUrl.IndexOf(':') - 2);
                if (System.IO.File.Exists(Path.Combine("C:\\home\\data\\Pics", tempPicture.LocalPath))) System.IO.File.Delete(Path.Combine("wwwroot", tempPicture.LocalPath));
                tempPicture.LocalPath = "";
                _context.Entry(tempPicture).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("PictureManager");
            }

            return RedirectToAction("PictureManager", "Unable to upload, delete image.");
        }

        [HttpPost]
        [RoleRequirement("ADMIN,MODERATOR")]
        public async Task<IActionResult> DiscardPicture(int pictureId)
        {
            var temp = await _context.Pictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();

            if (temp == null || temp.LocalPath.IsNullOrEmpty()) return RedirectToAction("PictureManager"); //already discarded

            var path = Path.Combine("C:\\home\\data\\Pics", temp.LocalPath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                _context.Remove(temp);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("PictureManager");
        }

        [HttpPost]
        [RoleRequirement("ADMIN,MODERATOR")]
        public async Task<IActionResult> BanUserAndDiscardPicture(int pictureId, [Bind("Id, BanReason, BanExpireIn")] UserModel user)
        {
            var tempPicture = await _context.Pictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            if (tempPicture != null && !tempPicture.LocalPath.IsNullOrEmpty())
            {

                var path = Path.Combine("C:\\home\\data\\Pics", tempPicture.LocalPath);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    _context.Remove(tempPicture);
                }
            }

            if (tempUser != null && !tempUser.IsBanned)
            {
                tempUser.BanReason = user.BanReason;
                tempUser.BanExpireIn = user.BanExpireIn;
                tempUser.IsBanned = true;
                _context.Entry(tempUser).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("PictureManager");
        }
        #endregion
    }
}
