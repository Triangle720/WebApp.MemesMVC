using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;
using System.Net.Http;
using BlobStorageDemo;
using System;

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
        [Route("Management/Index/{roleType}S/page/{pageIndex}")]
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

        [Route("Management/BanManager/page/{pageIndex}/Role/{roleType}/userId/{userId}")]
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

        public IActionResult PictureManager(string errorMessage = "")
        {
            if (errorMessage != "") ViewBag.Error = errorMessage;
            var picture = _context.Pictures.Where(p => !p.IsAccepted).FirstOrDefault();
            return View(picture);
        }
        #endregion

        #region POST SECTION
        [HttpPost]
        public async Task<IActionResult> Ban([Bind("Id, BanExpireIn, BanReason")] UserModel user, int pageIndex)
        {
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            if (tempUser != null && !tempUser.IsBanned) await Ban(tempUser, user);

            return RedirectToAction("Index", "Management", new { roleType = tempUser.Role, pageIndex });
        }

        [HttpPost]
        public async Task<IActionResult> Unban([Bind("Id")] UserModel user, int pageIndex)
        {
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            if (tempUser != null && tempUser.IsBanned) await Unban(tempUser);

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

        [HttpPost]
        public async Task<IActionResult> AcceptPicture(int pictureId)
        {
            var tempPicture = _context.Pictures.Where(p => p.Id == pictureId).FirstOrDefault();

            if (tempPicture == null || tempPicture.IsAccepted) return RedirectToAction("PictureManager");

            using var client = _clientFactory.CreateClient("imgur");
            var content = new StringContent(tempPicture.LocalPath);
            var response = await client.PostAsync("image", content);

            var imgUrl = response.Content.ReadAsStringAsync().Result;
            imgUrl = imgUrl.Split(',').Where(s => s.Contains("link")).FirstOrDefault();
    
            if (response.IsSuccessStatusCode)
            {
                //I did some string operations instead of creating new model for JSON Serialization
                tempPicture.UrlAddress = imgUrl.Substring(imgUrl.IndexOf(':') + 2, imgUrl.LastIndexOf('"') - imgUrl.IndexOf(':') - 2);
                tempPicture.IsAccepted = true;
                _context.Entry(tempPicture).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("PictureManager");
            }

            return RedirectToAction("PictureManager", "Unable to upload, delete image.");
        }

        [HttpPost]
        public async Task<IActionResult> DiscardPicture(int pictureId)
        {
            var tempPicture = await _context.Pictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
            if (tempPicture != null) await DeleteImageAsync(tempPicture);
            return RedirectToAction("PictureManager");
        }

        [HttpPost]
        public async Task<IActionResult> BanUserAndDiscardPicture(int pictureId, [Bind("Id, BanReason, BanExpireIn")] UserModel user)
        {
            var tempPicture = await _context.Pictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
            var tempUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            if (tempPicture != null) await DeleteImageAsync(tempPicture);
            if (tempUser != null && !tempUser.IsBanned) await Ban(tempUser, user);

            return RedirectToAction("PictureManager");
        }
        #endregion

        #region addidional functions
        public async Task Ban(UserModel tempUser, [Bind("Id, BanReason, BanExpireIn")] UserModel UserBanInfos)
        {
            tempUser.BanReason = UserBanInfos.BanReason;
            tempUser.BanExpireIn = UserBanInfos.BanExpireIn;
            tempUser.IsBanned = true;
            _context.Entry(tempUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Unban(UserModel user)
        {
            user.IsBanned = false;
            user.BanReason = string.Empty;
            user.BanExpireIn = DateTime.MinValue;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(PictureModel picture)
        {
            await BlobStorageService.DeleteImageAsync(picture.LocalPath.Substring(picture.LocalPath.LastIndexOf('/') + 1));
            _context.Remove(picture);
            await _context.SaveChangesAsync();       
        }
        #endregion
    }
}