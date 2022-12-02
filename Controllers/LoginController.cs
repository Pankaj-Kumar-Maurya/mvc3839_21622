using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc3839_21622.Models;

namespace mvc3839_21622.Controllers
{
    public class LoginController : Controller
    {
        DatabaseContext _db = new DatabaseContext();
        // GET: Login
        public ActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginForm(tblemployee _emp)
        {
            var data = (from a in _db.tblemployees where a.email ==_emp.email && a.password==_emp.password select a).ToList();
            if(data.Count>0)
            {
                return RedirectToAction("UserHome","Home1");
            }
            else
            {
                ViewBag.msg = "Login Fail Please CheK Filled Informatiom";
            return View();
            }

        }
    }
}