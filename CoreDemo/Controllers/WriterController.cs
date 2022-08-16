using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace CoreDemo.Controllers
{
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());
        public IActionResult Index()
        {
            var userMail = User.Identity.Name;
            ViewBag.v = userMail;
            Context c = new Context();
            var userName = c.Writers.FirstOrDefault(x => x.WriterMail == userMail).WriterName;
            ViewBag.v2 = userName;
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult WriterNavbarPartial()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult WriterFooterPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public IActionResult WriterEditProfile()
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            int writerId = c.Writers.FirstOrDefault(x => x.WriterMail == userMail).WriterID;

            var writerValues = wm.TGetById(writerId);
            return View(writerValues);
        }
        [HttpPost]
        public IActionResult WriterEditProfile(Writer writer)
        {
            WriterValidator wl = new WriterValidator();
            ValidationResult results = wl.Validate(writer);
            if (results.IsValid)
            {
                wm.TUpdate(writer);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult WriterAdd()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult WriterAdd(AddProfileImage p)
        {
            Writer writer = new Writer();
            if (p != null)
            {
                var extension = Path.GetExtension(p.WriterImage.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", newImageName);
                var stream = new FileStream(location, FileMode.Create);
                p.WriterImage.CopyTo(stream);
                writer.WriterImage = newImageName;

            }
            writer.WriterMail = p.WriterMail;
            writer.WriterName = p.WriterName;
            writer.WriterPassword = p.WriterPassword;
            writer.WriterStatus = true;
            writer.WriterAbout = p.WriterAbout;
            wm.TAdd(writer);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
