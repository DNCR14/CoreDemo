using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection.Metadata;
using X.PagedList;
using FluentValidation.Results;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        
        public IActionResult Index(int page=1)
        {
            var values = cm.GetList().ToPagedList(page,5);
            return View(values);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
          
            CategoryValidator cv = new CategoryValidator();
            ValidationResult result = cv.Validate(category);
            if (result.IsValid)
            {
                category.CategoryStatus = true;
                cm.TAdd(category);
                return RedirectToAction("Index", "Category");
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

        public IActionResult CategoryDelete(int id)
        {
            var value = cm.TGetById(id);
            cm.TDelete(value);

            return RedirectToAction("Index");
        }
    }
}
