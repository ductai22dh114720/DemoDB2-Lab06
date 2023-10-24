using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class LoginUserController : Controller
    {
        DBSportStoreEntities database = new DBSportStoreEntities();
        // GET: LoginUser : Form Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterUser()
        {
            return View();
        }    

        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if(ModelState.IsValid)
            {
                var check_ID = database.AdminUsers.Where(s => s.ID == _user.ID).FirstOrDefault();
                if (check_ID == null) // chua co ID
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.AdminUsers.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exixst";
                    return View();
                }
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult LoginAcount(AdminUser _user) 
        {
            var check = database.AdminUsers.
                Where(s => s.NameUser == _user.NameUser && s.PasswordUser ==_user.PasswordUser).FirstOrDefault();

            if(check == null) //login sai thong tin
            {
                ViewBag.ErrorInfo = "SaiInfo";
                return View("Index");
            }    
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["NameUser"] = _user.NameUser;
                Session["PasswordUser"] = _user.PasswordUser;
                return RedirectToAction("Index", "Product");
            }    
        }
        public  ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "LoginUser");
        }
    }
}