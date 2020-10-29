using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlobStorageDemo;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.MemesMVC.Data;
using WebApp.MemesMVC.Models;
using WebApp.MemesMVC.Security;

namespace WebApp.MemesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region VIEW REGION
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        [Route("Home/Index/page/{pageIndex?}")]
        [AllowAnonymous]
        public IActionResult Index(int pageIndex)
        {
            if (pageIndex <= 0) pageIndex = 1;
            ViewBag.PageIndex = pageIndex;

            var pictures = _context.Pictures.Where(p => p.IsAccepted)
                                            .OrderByDescending(p => p.UploadTime)
                                            .Skip((pageIndex - 1) * 10)
                                            .Take(10)
                                            .ToList();

            List<DisplayedPictureModel> pictureList = new List<DisplayedPictureModel>();

            foreach (PictureModel pic in pictures)
            {
                pictureList.Add(new DisplayedPictureModel(
                    pic.Title,
                    pic.UrlAddress,
                    _context.Users.Where(u => u.Id == pic.UserModelId).SingleOrDefault().Nickname,
                    pic.UploadTime));
            }

            return View(pictureList);
        }

        [AllowAnonymous]
        public IActionResult Random()
        {
            var random = _context.Pictures.OrderBy(o => Guid.NewGuid()).Where(p => p.IsAccepted).FirstOrDefault();

            var picture = new DisplayedPictureModel(
                    random.Title,
                    random.UrlAddress,
                    _context.Users.Where(u => u.Id == random.UserModelId).SingleOrDefault().Nickname,
                    random.UploadTime);

            return View(picture);
        }

        [RoleRequirement("ADMIN,MODERATOR")]
        public IActionResult Management()
        {
            return View();
        }

        [RoleRequirement("ADMIN,MODERATOR,USER")]
        public IActionResult Profile()
        {
            var user = _context.Users.Where(u => u.Login == HttpContext.Session.GetString("LOGIN")).FirstOrDefault();
            return View(user);
        }

        [RoleRequirement("ADMIN,MODERATOR,USER")]
        public IActionResult AddMeme(bool isSucceed = false)
        {
            return View(isSucceed);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region POST REGION
        [RoleRequirement("ADMIN,MODERATOR,USER")]
        [HttpPost]
        public async Task<IActionResult> ProfilePasswordChange(int userId, string password, string newPassword)
        {
            var tempUser = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (!Encryptor.IsPasswordCorrect(password, tempUser.Password))
            {
                ViewBag.Error = "Wrong password";
                return View("Profile", tempUser);
            }
            else if(password == newPassword)
            {
                ViewBag.Error = "Passwords are not different";
                return View("Profile", tempUser);
            }

            tempUser.Password = Encryptor.EncryptPassword(newPassword);
            _context.Entry(tempUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction("Logout", "Login");
        }

        [RoleRequirement("ADMIN,MODERATOR,USER")]
        [HttpPost]
        public async Task<IActionResult> UploadPicture(IFormFile file, string description = "")
        {
            if (file != null && file.Length > 0)
            {
                var fileExtenstion = Enum.GetNames(typeof(ImageExtensions)).Where(e => file.FileName.ToUpper().Contains(e)).FirstOrDefault();
                if (fileExtenstion != null)
                {
                    try
                    {
                        var imgPath = await BlobStorageService.UploadImageAsync(file);

                        if (!imgPath.IsNullOrEmpty())
                        {
                            var temp = new PictureModel
                            {
                                UserModelId = _context.Users.Where(u => u.Login == HttpContext.Session.GetString("LOGIN")).FirstOrDefault().Id,
                                LocalPath = imgPath, //kinda 'local' path. 
                                Title = description
                            };

                            _context.Pictures.Add(temp);
                            await _context.SaveChangesAsync();

                            return RedirectToAction("AddMeme", new { isSucceed = true });
                        }

                        throw new ArgumentNullException();
                    }
                    catch
                    {
                        ViewBag.Message = "An error occured during upload";
                    }
                }
                else ViewBag.Message = "Bad file extension.";
            }        
            else ViewBag.Message = "You have not specified a file.";

            return View("AddMeme");
        }
        #endregion
    }
}
