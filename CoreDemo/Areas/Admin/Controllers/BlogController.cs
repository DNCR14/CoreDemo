using ClosedXML.Excel;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class BlogController : Controller
    {

        public IActionResult ExportStaticExcelBlogList()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Blog Listesi");
                worksheet.Cell(1, 1).Value = "Blog ID";
                worksheet.Cell(1, 2).Value = "Blog Adı";

                int BlogRowCount = 2;
                foreach (var item in GetBlogList())
                {
                    worksheet.Cell(BlogRowCount, 1).Value = item.ID;
                    worksheet.Cell(BlogRowCount, 2).Value = item.BlogName;
                    BlogRowCount++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel.xlsx");
                }
            }
        }

        public List<BlogModel> GetBlogList()
        {
            List<BlogModel> blogList = new List<BlogModel>
            {
                new BlogModel
            {
                ID = 1,
                BlogName="Entity Framework Nedir",
            },
                new BlogModel{
                ID=2,
                BlogName ="C# ile Firebase Kullanımı",
            },
             new BlogModel{
                ID=3,
                BlogName ="SQL Örnekleri",
            }};
            return blogList;

        }

        public IActionResult BlogListExcel()
        {
            return View();
        }

        public IActionResult ExportDinamikExcelBlogList()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Blog Listesi");
                worksheet.Cell(1, 1).Value = "Blog ID";
                worksheet.Cell(1, 2).Value = "Blog Adı";

                int BlogRowCount = 2;
                foreach (var item in GetBlogList2())
                {
                    worksheet.Cell(BlogRowCount, 1).Value = item.ID;
                    worksheet.Cell(BlogRowCount, 2).Value = item.BlogName;
                    BlogRowCount++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel.xlsx");
                }
            }
        }
        public List<BlogModel> GetBlogList2()
        {
            List<BlogModel> blogList = new List<BlogModel>();
            using(var c = new Context())
            {
                blogList = c.Blogs.Select(x => new BlogModel
                {
                    ID = x.BlogID,
                    BlogName = x.BlogTitle
                }).ToList();
            }
            return blogList;

        }

    }
}
