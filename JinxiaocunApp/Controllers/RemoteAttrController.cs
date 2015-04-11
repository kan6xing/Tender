using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;

namespace JinxiaocunApp.Controllers
{
    public class RemoteAttrController : Controller
    {
        //
        // GET: /RemoteAttr/

        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult IsExist(string BNumber = null,int BID=0, string BFullName = null,
            string NumberEmp = null, int EmpID = 0, RegisterModel reg = null)
        {
            if (BNumber != null)
            {
                if (BID > 0)
                {
                    return Json(db.BDepartments.Where(m => m.BID!=BID&&m.BNumber.Equals(BNumber)).Count() <= 0, JsonRequestBehavior.AllowGet);
                }
                return Json(db.BDepartments.Where(m => m.BNumber.Equals(BNumber)).Count() <= 0, JsonRequestBehavior.AllowGet);
            }
            else if (BFullName != null)
            {//BFullName  BID用来判断是否Edit
                if (db.BDepartments.Where(m => m.BID == BID).Count() > 0)
                {
                    //判断是否有重复
                    if (db.BDepartments.Where(m => m.BFullName.Equals(BFullName)).Count() > 0)
                    {
                        //判断是否自身
                        return Json(db.BDepartments.Where(m => m.BFullName.Equals(BFullName) && m.BID == BID).Count() > 0, JsonRequestBehavior.AllowGet);

                    }
                    //没重复返回true
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                //CREATE验证
                return Json(db.BDepartments.Where(m => m.BFullName.Equals(BFullName)).Count() <= 0, JsonRequestBehavior.AllowGet);
            }
                
                
                
                //**************BEmplyee***********
            else
            {
                //if (db.BEmplyees.Where(m => m.EmpID == EmpID).Count()>0)
                //{
                //    return Json(true, JsonRequestBehavior.AllowGet);
                //}
                if (reg != null&&reg.bemplyee!=null)
                { 
                   NumberEmp = reg.bemplyee.NumberEmp;  
                }
                   

                if (EmpID > 0)
                {
                    return Json(db.BEmplyees.Where(m => m.EmpID != EmpID && m.NumberEmp.Equals(NumberEmp)).Count() <= 0, JsonRequestBehavior.AllowGet);
                }
                return Json(db.BEmplyees.Where(m=>m.NumberEmp.Equals(NumberEmp)).Count()<=0,JsonRequestBehavior.AllowGet);
            }
            
        }

    }
}
