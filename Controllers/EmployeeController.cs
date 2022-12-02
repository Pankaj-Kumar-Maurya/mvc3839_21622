using mvc3839_21622.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc3839_21622.Controllers
{
    public class EmployeeController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        public ActionResult AddEmployee(int id=0)
        {
            ViewBag.BT = "Creata";
            TableCollection obj = new TableCollection();
           


            var Hdata = db.tblhobbies.ToList();
            obj.lsthobby1 = Hdata.Select(x => new tblhobby1
            {
                hid = x.hid,
                hname = x.hname,
                ischecked = false
            }).ToList();
            if (id>0)
            {
                var data = db.tblemployees.Where(x=>x.empid==id).ToList();
                obj.empid = data[0].empid;
                obj.name = data[0].name;
                obj.email = data[0].email;
                obj.password = data[0].password;
                obj.gender = data[0].gender;
                obj.country = data[0].country;
                obj.state = data[0].state;
                string[] arr = data[0].hobbies.ToString().Split(',');
                foreach(var a in obj.lsthobby1)
                {
                    foreach(var b in arr)
                    {
                        if(a.hname==b)
                        {
                            a.ischecked = true;
                        }
                    }
                }

                ViewBag.BT = "Update";
            }


            obj.lstcountry = db.tblcountries.ToList();
            obj.lstgender = db.tblgenders.ToList();
            obj.lststate = db.tblstates.Where(x => x.cid == obj.country).ToList();


            return View(obj);
        }
        [HttpPost]
        public ActionResult AddEmployee(TableCollection TL)
        {
            string HOBS = "";
            foreach(var a in TL.lsthobby1)
            {
                if(a.ischecked==true)
                {
                    HOBS += a.hname + ",";
                }
            }
            HOBS = HOBS.TrimEnd(',');

            tblemployee emp = new tblemployee();
            emp.name = TL.name;
            emp.email = TL.email;
            emp.password = TL.password;
            emp.gender = TL.gender;
            emp.country = TL.country;
            emp.state = TL.state;
            emp.hobbies = HOBS;
            if(TL.empid>0)
            {
                emp.empid = TL.empid;
                db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Show");
            }
            else
            {
                var data = (from a in db.tblemployees where a.email==emp.email select a).ToList();
                if(data.Count>0)
                {
                    TableCollection obj = new TableCollection();
                    obj.lstcountry = db.tblcountries.ToList();
                    obj.lstgender = db.tblgenders.ToList();
                    obj.lststate = db.tblstates.Where(x => x.cid == obj.country).ToList();
                    ViewBag.msg = "This Email is Already Enterd";
                    return View(obj);

                }
                else
                {
                   db.tblemployees.Add(emp);
                    db.SaveChanges();
                    return RedirectToAction("Show");
                }
            }
           
        }

        public ActionResult Delete(int id = 0)
        {
            var data = db.tblemployees.Find(id)
;
            db.tblemployees.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        public ActionResult Show()
        {
            var data = (from a in db.tblemployees
                        join b in db.tblgenders on a.gender equals b.gid
                        join c in db.tblcountries on a.country equals c.cid
                        join d in db.tblstates on a.state equals d.sid
                        select new tblemployee1
                        {
                            empid = a.empid,
                            name = a.name,
                            email = a.email,
                            hobbies=a.hobbies,
                            password = a.password,
                            gname = b.gname,
                            cname = c.cname,
                            sname = d.sname
                        }).ToList();
            return View(data);
        }

        public JsonResult GetState(int A)
        {
            var data = (from a in db.tblstates where a.cid == A select a).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}