using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class ProductController : Controller
    {
        DBSportStoreEntities database = new DBSportStoreEntities();
        // GET: Product
       
        public ActionResult Index()
        {
            return View(database.Products.ToList());
        }


        public ActionResult Create()
        {
            List<Category> list = database.Categories.ToList();
            ViewBag.listCategory = new SelectList(list, "IDCate", "NameCate");
            ProductController pro = new ProductController();
            return View(pro);
        }
       

        public ActionResult SelectCate() 
        {
            Category se_cate = new Category();
            se_cate.ListCate = database.Categories.ToList<Category>();
            return PartialView(se_cate);
        }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
        [HttpPost]
        public ActionResult Create(Product pro)
        {
         
            try
            {
                if (pro.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(pro.UploadImage.FileName);
                    string extent = Path.GetExtension(pro.UploadImage.FileName);
                    filename = filename + extent;
                    pro.ImagePro = "~/Content/images/" + filename;
                    pro.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                }   
               
                database.Products.Add(pro);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                
                return View();
            }
        }
        public ActionResult Index(string category)
        {
            if(category==null)
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro);
                return View(productList);
            }   
            else
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro)
                    .Where(x => x.Category == category);
                return View(productList);
            }    
        }

    }
    

}