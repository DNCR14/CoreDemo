using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        CommentManager comman = new CommentManager(new EfCommentRepository());
        public IActionResult Index()
        {
            ViewBag.toplamBlog = bm.GetList().Count;
            ViewBag.toplamYazarBlog = bm.GetBlogListByWriter(1).Count;
            ViewBag.toplamYorum = comman.GetList().Count;
            return View();
        }
    }
}
