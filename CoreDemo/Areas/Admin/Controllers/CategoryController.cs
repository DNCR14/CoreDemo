using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        [Area("Admin")]
        public IActionResult Index(int page=1)
        {
            var values = cm.GetList().ToPagedList(page,5);
            return View(values);
        }
    }
}
