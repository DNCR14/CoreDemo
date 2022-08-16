using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class BlogController : Controller
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        public IActionResult Index()
        {
            var values = bm.GetBlogListWithCategory();
            return View(values);
        }

        public IActionResult BlogReadAll(int id)
        {
            ViewBag.id = id;
            var values = bm.GetBlogById(id);
            return View(values);
        }

        public IActionResult BlogListByWriter()
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            int writerId = c.Writers.FirstOrDefault(x => x.WriterMail == userMail).WriterID;
            var values = bm.GetListWithCategoryByWriterBm(writerId);
            return View(values);
        }

        [HttpGet]
        public IActionResult BlogAdd()
        {
            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.cv = categoryValues;
            return View();
        }

        [HttpPost]
        public IActionResult BlogAdd(Blog blog)
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            int writerId = c.Writers.FirstOrDefault(x => x.WriterMail == userMail).WriterID;

            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.cv = categoryValues;

            BlogValidator bv = new BlogValidator();
            ValidationResult result = bv.Validate(blog);
            if (result.IsValid)
            {
                blog.BlogStatus = true;
                blog.BlogCreateDate = DateTime.Now;
                blog.WriterID = writerId;
                bm.TAdd(blog);
                return RedirectToAction("BlogListByWriter", "Blog");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        public IActionResult DeleteBlog(int id)
        {
            var blogValue = bm.TGetById(id);
            bm.TDelete(blogValue);

            return RedirectToAction("BlogListByWriter");
        }

        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            List<SelectListItem> categoryValues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.cv = categoryValues;

            var blogValue = bm.TGetById(id);

            return View(blogValue);
        }

        [HttpPost]
        public IActionResult EditBlog(Blog blog)
        {
            var userMail = User.Identity.Name;
            Context c = new Context();
            int writerId = c.Writers.FirstOrDefault(x => x.WriterMail == userMail).WriterID;

            blog.WriterID = writerId;
            blog.BlogCreateDate = DateTime.Now;
            blog.BlogStatus = true;
            bm.TUpdate(blog);
            return RedirectToAction("BlogListByWriter","Blog");
        }

    }
}
