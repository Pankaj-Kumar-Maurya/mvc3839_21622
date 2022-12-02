using mvc3839_21622.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace mvc3839_21622.Controllers
{
    public class UserController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        // GET: User
        public ActionResult AddUser(int id=0)
        {
            tblUser obj = new tblUser();
            ViewBag.BT = "Create";
            if(id>0)
            {
                var data = db.tblUsers.Where(x => x.userid == id).ToList();
                obj.userid = data[0].userid;
                obj.name = data[0].name;
                obj.age = data[0].age;
                obj.img = data[0].img;
                ViewBag.imgg = data[0].img;
                ViewBag.BT = "Update"; 

            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult AddUser(tblUser _usr,HttpPostedFileBase file)
        {
            
            if(_usr.userid>0)
            {
                if(file!=null)
                {
                string FN = Path.GetFileName(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/PIC/"), FN));
                System.IO.File.Delete(Server.MapPath(_usr.img));
                _usr.img = "~/PIC/" + FN;
                }
                db.Entry(_usr).State = System.Data.Entity.EntityState.Modified;

            }
            else
            {
            string FN = Path.GetFileName(file.FileName);
            file.SaveAs(Path.Combine(Server.MapPath("~/PIC/"), FN));
            _usr.img = "~/PIC/" + FN;
            db.tblUsers.Add(_usr);
            }
            db.SaveChanges();
            return RedirectToAction("ShowUser");
        }

        public ActionResult Del(int id=0,string img="")
        {
            var data = db.tblUsers.Find(id);
            db.tblUsers.Remove(data);
            db.SaveChanges();
            System.IO.File.Delete(Server.MapPath(img));
            return RedirectToAction("ShowUser");
        }

        public ActionResult ShowUser()
        {
            var data = db.tblUsers.ToList();
            return View(data);
        }
    }
}