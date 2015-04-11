using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;

namespace JinxiaocunApp.Controllers.Tender
{
    public class Tender_ModelManage1Controller : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /Tender_ModelManage1/

        public ActionResult Index()
        {
            var tender_modelmanage1 = db.Tender_ModelManage1.Include(t => t.tenderGonggao);
            return View(tender_modelmanage1.ToList());
        }

        //
        // GET: /Tender_ModelManage1/Details/5

        public ActionResult Details(int id = 0)
        {
            Tender_ModelManage1 tender_modelmanage1 = db.Tender_ModelManage1.Find(id);
            if (tender_modelmanage1 == null)
            {
                return HttpNotFound();
            }
            return View(tender_modelmanage1);
        }

        //
        // GET: /Tender_ModelManage1/Create

        [MultipleResponseFormats]
        public ActionResult Create(int TaskID=1)
        {
            
            if (TaskID != 1)
            {
                Tender_GongGao tg= db.Tender_GongGaos.Find(TaskID);
                if (tg != null)
                {
                    if(tg.IsShenhe!=true)
                    {
                        return Content("尚未进行供应商投标审核,不能投标~！");
                    }

                    Tender_GongGao tgonggao = db.Tender_GongGaos.Find(TaskID);
                    if (tgonggao.TenderModel != null && tgonggao.TenderModel.Trim().Equals("竞价模式"))
                    {
                        return Content("您已选择竞价模式，不能再选择三轮报价模式");
                    }
                    tgonggao.TenderModel = "三轮报价模式";
                    
                    db.Entry(tgonggao).State = EntityState.Modified;
                    db.SaveChanges();

                    int mcount = db.Tender_ModelManage1.Where(m => m.Tid == TaskID).Count();
                    if (mcount >0)
                    {
                        ViewBag.ggM = tg;
                        Tender_ModelManage1 mod2 = db.Tender_ModelManage1.Single(m => m.Tid == TaskID);
                        mod2.Tid = TaskID;
                        return View(mod2);
                        //return Content("已经投标 请不要重复投标");
                    }
                    ViewBag.ggM = tg;
                    Tender_ModelManage1 mod1 = new Tender_ModelManage1();
                    mod1.PriceUnit = "元/标";
                    mod1.Tid = TaskID;
                    return View(mod1);
                    //mod1.tenderGonggao = tg;
                    //ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN");
                    //return View(mod1);
                }else
                {
                    return Content("请求页面异常~！");
                }
            }
            return Content("如有问题 请联系管理员");
            //ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN");
            //return View();
            
        }

        //
        // POST: /Tender_ModelManage1/Create
        [MultipleResponseFormats]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tender_ModelManage1 tender_modelmanage1,string btnAction="")
        {
            Tender_ModelManage1 tender_modelmanage2;
            switch (btnAction)
            { 
                case "btn1":
                    
                    if( db.Tender_ModelManage1.Where(m => m.Tid == tender_modelmanage1.Tid).Count()>0)
                    {
                        tender_modelmanage2 = db.Tender_ModelManage1.Single(m => m.Tid == tender_modelmanage1.Tid);
                        tender_modelmanage2.BeginTime1 = tender_modelmanage1.BeginTime1;
                        tender_modelmanage2.EndTime1 = tender_modelmanage1.EndTime1;
                        tender_modelmanage2.PriceUnit = tender_modelmanage1.PriceUnit;
                        
                        db.Entry(tender_modelmanage2).State = EntityState.Modified;
                        db.SaveChanges();
                        return Content("第一轮时间确认");
                    }

                    var empGs = db.Bemp_GongGaos.Where(m => m.GongGaoId == tender_modelmanage1.Tid);
                    if(empGs.Count()>0)
                    {
                        foreach(Bemp_GongGao bemp in empGs)
                        {
                            bemp.PriceUnit = tender_modelmanage1.PriceUnit;
                            db.Entry(bemp).State = EntityState.Modified;
                           
                        }
                    }

                    tender_modelmanage1.DateToday = System.DateTime.Now.Date;
                    db.Tender_ModelManage1.Add(tender_modelmanage1);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                    return Content("第一轮时间确认");
                    break;
                case "btn2":
                     tender_modelmanage2 = db.Tender_ModelManage1.Single(m => m.Tid == tender_modelmanage1.Tid);
                    tender_modelmanage2.BeginTime2 = tender_modelmanage1.BeginTime2;
                    tender_modelmanage2.EndTime2 = tender_modelmanage1.EndTime2;
                    db.Entry(tender_modelmanage2).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content(" 时间确认");
                default:
                    tender_modelmanage2 = db.Tender_ModelManage1.Single(m => m.Tid == tender_modelmanage1.Tid);
                    tender_modelmanage2.BeginTime3 = tender_modelmanage1.BeginTime3;
                    tender_modelmanage2.EndTime3 = tender_modelmanage1.EndTime3;
                    db.Entry(tender_modelmanage2).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content(" 时间确认");
            }
            //if (ModelState.IsValid)
            //{
            //    db.Tender_ModelManage1.Add(tender_modelmanage1);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN", tender_modelmanage1.Tid);
            //return View(tender_modelmanage1);
        }

        //
        // GET: /Tender_ModelManage1/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Tender_ModelManage1 tender_modelmanage1 = db.Tender_ModelManage1.Find(id);
            if (tender_modelmanage1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN", tender_modelmanage1.Tid);
            return View(tender_modelmanage1);
        }

        //
        // POST: /Tender_ModelManage1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tender_ModelManage1 tender_modelmanage1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tender_modelmanage1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN", tender_modelmanage1.Tid);
            return View(tender_modelmanage1);
        }

        //
        // GET: /Tender_ModelManage1/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Tender_ModelManage1 tender_modelmanage1 = db.Tender_ModelManage1.Find(id);
            if (tender_modelmanage1 == null)
            {
                return HttpNotFound();
            }
            return View(tender_modelmanage1);
        }

        //
        // POST: /Tender_ModelManage1/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tender_ModelManage1 tender_modelmanage1 = db.Tender_ModelManage1.Find(id);
            db.Tender_ModelManage1.Remove(tender_modelmanage1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LoadData(int id = 0)
        {
            Tender_CustomerData data = new Tender_CustomerData();
            data.StrServ = "";
            data.PriceShang = 0;
            data.PriceShangLow = 0;
            //Bemp_GongGao empGong = db.Bemp_GongGaos.Include(m => m.Tender_GongGaos).Single(m => m.BGid == id);
            Tender_ModelManage1 modelM = null;
            try
            {
                modelM = db.Tender_ModelManage1.Single(m => m.Tid == id);
            }
            catch (Exception ex) { }

            if (modelM != null)
            {
                //Tender_ModelCustomer1 cust;
                //cust = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Single(m => m.EmpGonggaoID == empGong.BGid);


                if ((!string.IsNullOrEmpty(modelM.BeginTime1)) && (!string.IsNullOrEmpty(modelM.EndTime1)) && TimeSpan.Parse(modelM.EndTime1) >= System.DateTime.Now.TimeOfDay)
                {
                    data.Lunci = "一 未开始";
                    data.TimeXia = modelM.BeginTime1;
                    if ((TimeSpan.Parse(modelM.BeginTime1) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime1)))
                    {
                        //第一轮进行中
                        data.Lunci = "一";
                        data.EndTime = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime1) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                        data.PriceShang = 0;
                        data.PriceShangLow = 0;
                        data.TimeXia = modelM.BeginTime2;
                    }
                    //第一轮未开始

                }
                else if ((!string.IsNullOrEmpty(modelM.BeginTime2)) && (!string.IsNullOrEmpty(modelM.EndTime2)) && (TimeSpan.Parse(modelM.EndTime1) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime2)))
                {
                    data.Lunci = "二 未开始";
                    //data.PriceShang = cust.PriceOne;
                    //if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Count() > 0)
                    //{
                    //    data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Min(m => m.PriceOne);
                    //    if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne == data.PriceShangLow).Count() > 1)
                    //    {
                    //        data.StrServ = "请注意，此刻有多家供应商报出了相同的最低价。";
                    //    }
                    //}
                    //else
                    //{
                    //    data.PriceShangLow = 0;
                    //}
                    //data.TimeXia = modelM.BeginTime2;
                    if ((TimeSpan.Parse(modelM.BeginTime2) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime2)))
                    {
                        //第二轮进行中
                        data.Lunci = "二";
                        data.EndTime = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime2) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                        data.TimeXia = modelM.BeginTime3;

                    }
                    //第二轮未开始

                }
                else if ((!string.IsNullOrEmpty(modelM.BeginTime3)) && (!string.IsNullOrEmpty(modelM.EndTime3)) && (TimeSpan.Parse(modelM.EndTime2) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime3)))
                {
                    data.Lunci = "三 未开始";
                    //data.PriceShang = cust.PriceTwo;
                    data.TimeXia = modelM.BeginTime3;
                    //if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Count() > 0)
                    //{
                    //    data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Min(m => m.PriceTwo);
                    //    if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo == data.PriceShangLow).Count() > 1)
                    //    {
                    //        data.StrServ = "请注意，此刻有多家供应商报出了相同的最低价。";
                    //    }
                    //}
                    //else
                    //{
                    //    data.PriceShangLow = 0;
                    //}

                    if ((TimeSpan.Parse(modelM.BeginTime3) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime3)))
                    {
                        //第三轮进行中
                        data.Lunci = "三";
                        data.EndTime = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime3) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                        data.TimeXia = "";
                    }

                }
                

            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}