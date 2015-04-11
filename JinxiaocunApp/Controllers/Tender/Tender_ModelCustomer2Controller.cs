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
    public class Tender_ModelCustomer2Controller : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        public bool isSmall = true;
        public bool isSmallByid(int Gonggaoid)
        {
            Tender_GongGao tGonggao = db.Tender_GongGaos.Single(m => m.TaskID == Gonggaoid);
            string typeStr = tGonggao.Types;
            if (typeStr.Trim().ToLower().Equals("e"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //
        // GET: /Tender_ModelCustomer2/

        [MultipleResponseFormats]
        public ActionResult Index(int id=0)
        {
            var tender_mcust = db.Tender_ModelCustomer2.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id);
            isSmall = isSmallByid(id);
            //第一次加载 生成客户初始数据
            if (tender_mcust.Count() <= 0)
            {
                List<Bemp_GongGao> list = db.Bemp_GongGaos.Include(b => b.bemplyees).Where(m => m.GongGaoId == id&&m.IsPassShen!=null&&m.IsPassShen==true).ToList();
                if (list.Count > 0)
                {
                    lock (this)
                    {
                        foreach (Bemp_GongGao bemp in list)
                        {
                            Tender_ModelCustomer2 cust = new Tender_ModelCustomer2();
                            cust.UserName = bemp.bemplyees.NumberEmp;
                            cust.LinkMan = bemp.bemplyees.LinkMan;
                            cust.LinkType = bemp.bemplyees.LinkType;
                            cust.EmpGonggaoID = bemp.BGid;
                            cust.PriceLost = 0;

                            db.Tender_ModelCustomer2.Add(cust);
                            //li.Add(cust);
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            db.Configuration.ValidateOnSaveEnabled = true;
                        }
                    }
                    


                }
            }

            //找每轮最低价
            tender_mcust = db.Tender_ModelCustomer2.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id);
            ViewBag.PriceD = 0;

            tender_mcust = db.Tender_ModelCustomer2.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id && mc.PriceLost > 0);
            if (tender_mcust.Count() > 0)
            {
                if(isSmall)
                {
                    ViewBag.PriceD = tender_mcust.Min(m => m.PriceLost);
                }else
                {
                    ViewBag.PriceD = tender_mcust.Max(m => m.PriceLost);
                }
                
            }

            if(isSmall)
            {
                foreach (Tender_ModelCustomer2 custVar in tender_mcust.ToList<Tender_ModelCustomer2>())
                {
                    Bemp_GongGao bgong = db.Bemp_GongGaos.Find(custVar.EmpGonggaoID);
                    if (bgong.LostPrice == null || (custVar.PriceLost != 0 && bgong.LostPrice > custVar.PriceLost))
                    {
                        bgong.LostPrice = custVar.PriceLost;
                        db.Entry(bgong).State = EntityState.Modified;

                        db.SaveChanges();
                    }

                }
            }else
            {
                foreach (Tender_ModelCustomer2 custVar in tender_mcust.ToList<Tender_ModelCustomer2>())
                {
                    Bemp_GongGao bgong = db.Bemp_GongGaos.Find(custVar.EmpGonggaoID);
                    if (bgong.LostPrice == null || (custVar.PriceLost != 0 && bgong.LostPrice < custVar.PriceLost))
                    {
                        bgong.LostPrice = custVar.PriceLost;
                        db.Entry(bgong).State = EntityState.Modified;

                        db.SaveChanges();
                    }

                }
            }
            ViewBag.IsSmall = isSmall;
            tender_mcust = db.Tender_ModelCustomer2.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id);
            return View(tender_mcust.ToList());
        }

        //
        // GET: /Tender_ModelCustomer2/Details/5

        public ActionResult Details(int id = 0)
        {
            Tender_ModelCustomer2 tender_modelcustomer2 = db.Tender_ModelCustomer2.Find(id);
            if (tender_modelcustomer2 == null)
            {
                return HttpNotFound();
            }
            return View(tender_modelcustomer2);
        }

        //
        // GET: /Tender_ModelCustomer2/Create

        [MultipleResponseFormats]
        public ActionResult Create(int id=0)
        {
            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid");

            if(id!=0)
            {
                Bemp_GongGao empGong=null;
                try
                {
                    empGong = db.Bemp_GongGaos.Include(m => m.Tender_GongGaos).Single(m => m.BGid == id&&m.IsPassShen!=null&&m.IsPassShen==true);
                    isSmall = isSmallByid(empGong.GongGaoId);
                    ViewBag.IsSmall = isSmall;
                }catch(Exception ex)
                {
                    return Content("不能访问，可能审核未通过");
                }
                

                Tender_ModelManage2 modelM = null;
                if (db.Tender_ModelManage2.Where(m => m.Tid == empGong.GongGaoId).Count() > 0)
                {
                    modelM = db.Tender_ModelManage2.Single(m => m.Tid == empGong.GongGaoId);
                }
                

                if (modelM == null)
                { return Content("投标未开始"); }

                

                if (modelM.DateToday.Date < System.DateTime.Now.Date)
                {
                    return Content("投标已结束~!");
                }
                if (modelM.DateToday.Date > System.DateTime.Now.Date)
                {
                    return Content("投标未开始~!");
                }

                TimeSpan tsEnd;
                if (TimeSpan.TryParse(modelM.EndTime, out tsEnd))
                {
                    if (tsEnd <= System.DateTime.Now.TimeOfDay)
                    {
                        return Content("投标已结束");
                    }
                }
                else
                {
                    return Content("投标未开始");
                }

                TimeSpan tsBegin;
                if(TimeSpan.TryParse(modelM.BeginTime,out tsBegin))
                {}
                else
                { return Content("投标未开始"); }
                if (empGong != null)
                {
                    Tender_ModelCustomer2 cust;
                    cust = db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Single(m => m.EmpGonggaoID == empGong.BGid);
                    if (cust == null)
                    {
                        return Content("请联系管理");
                        
                    }

                    cust.PriceUnit = modelM.PriceUnit;
                    db.Entry(cust).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;

                    ViewBag.TimeXia = modelM.BeginTime;
                    if(tsBegin<=System.DateTime.Now.TimeOfDay&&System.DateTime.Now.TimeOfDay<=tsEnd)
                    {
                        ViewBag.Jieshu = Convert.ToDateTime((tsEnd - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");

                        if (db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Count() > 0)
                        {
                            if(isSmall)
                            {
                                ViewBag.ShangDi = db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Min(m => m.PriceLost);
                            }
                            else
                            {
                                ViewBag.ShangDi = db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Max(m => m.PriceLost);
                            }
                            
                        }
                    }

                    return View(cust);
                }
            }
            return View();
        }

        //
        // POST: /Tender_ModelCustomer2/Create

        [HttpPost]
        [ValidateAntiForgeryToken][MultipleResponseFormats]
        public ActionResult Create(Tender_ModelCustomer2 tender_modelcustomer21)
        {
            if (ModelState.IsValid)
            {
                int ttid = db.Bemp_GongGaos.Single(n => n.BGid == tender_modelcustomer21.EmpGonggaoID).GongGaoId;
                isSmall = isSmallByid(ttid);

                Tender_ModelManage2 modelM = db.Tender_ModelManage2.Single(m => m.Tid == ttid);
                Tender_ModelCustomer2 tender_modelcustomer2 = db.Tender_ModelCustomer2.Single(m => m.EmpGonggaoID == tender_modelcustomer21.EmpGonggaoID);

                if(TimeSpan.Parse(modelM.BeginTime)<=System.DateTime.Now.TimeOfDay&&TimeSpan.Parse(modelM.EndTime)>=System.DateTime.Now.TimeOfDay)
                {
                    if(isSmall)
                    {
                        if (tender_modelcustomer2.PriceLost!=0&&tender_modelcustomer2.PriceLost <= tender_modelcustomer21.PriceLost)
                        {
                            return Content("价格必须小于上次报价~!");
                        }
                    }
                    else
                    {
                        if (tender_modelcustomer2.PriceLost != 0 && tender_modelcustomer2.PriceLost >= tender_modelcustomer21.PriceLost)
                        {
                            return Content("价格必须大于上次报价~!");
                        }
                    }
                    
                        tender_modelcustomer2.PriceLost = tender_modelcustomer21.PriceLost;
                        tender_modelcustomer2.bemp_Gonggao.PayType = tender_modelcustomer21.bemp_Gonggao.PayType;
                        tender_modelcustomer2.bemp_Gonggao.HandDate = tender_modelcustomer21.bemp_Gonggao.HandDate;
                        Bemp_GongGao bemp = tender_modelcustomer2.bemp_Gonggao;

                        db.Entry(tender_modelcustomer2).State = EntityState.Modified;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        db.Configuration.ValidateOnSaveEnabled = true;
                        return Content("投标完成！");
                    
                }else if(TimeSpan.Parse(modelM.BeginTime)>System.DateTime.Now.TimeOfDay)
                {
                    return Content("投标未开始");
                }else
                { return Content("投标已结束"); }
            }


            return Content("投标未开始");
        }

        public ActionResult LoadData(int id=0)
        {
            Tender_CustomerData data = new Tender_CustomerData();
            Bemp_GongGao empGong = db.Bemp_GongGaos.Include(m => m.Tender_GongGaos).Single(m => m.BGid == id);
            isSmall = isSmallByid(empGong.GongGaoId);
            ViewBag.IsSmall = isSmall;

            Tender_ModelManage2 modelM = db.Tender_ModelManage2.Single(m => m.Tid == empGong.GongGaoId);
            data.StrServ = "";
            data.PriceShangLow = 0;
            if ((TimeSpan.Parse(modelM.BeginTime) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime)))
            {
                data.EndTime = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                try
                {
                    if (db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Count()>0)
                    {
                        if (isSmall)
                        {
                            data.PriceShangLow = db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Min(m => m.PriceLost);
                        }
                        else
                        {
                            data.PriceShangLow = db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Max(m => m.PriceLost);
                        }

                        if (db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost == data.PriceShangLow).Count() > 1)
                        {
                            data.StrServ = "请注意，此刻有多家供应商报出了相同的最低价。";
                        }
                    }
                    
                }catch(Exception ex)
                { }
                
                

                
                data.TimeXia = modelM.BeginTime;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Tender_ModelCustomer2/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Tender_ModelCustomer2 tender_modelcustomer2 = db.Tender_ModelCustomer2.Find(id);
            if (tender_modelcustomer2 == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid", tender_modelcustomer2.EmpGonggaoID);
            return View(tender_modelcustomer2);
        }

        //
        // POST: /Tender_ModelCustomer2/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tender_ModelCustomer2 tender_modelcustomer2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tender_modelcustomer2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid", tender_modelcustomer2.EmpGonggaoID);
            return View(tender_modelcustomer2);
        }

        //
        // GET: /Tender_ModelCustomer2/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Tender_ModelCustomer2 tender_modelcustomer2 = db.Tender_ModelCustomer2.Find(id);
            if (tender_modelcustomer2 == null)
            {
                return HttpNotFound();
            }
            return View(tender_modelcustomer2);
        }

        //
        // POST: /Tender_ModelCustomer2/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tender_ModelCustomer2 tender_modelcustomer2 = db.Tender_ModelCustomer2.Find(id);
            db.Tender_ModelCustomer2.Remove(tender_modelcustomer2);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}