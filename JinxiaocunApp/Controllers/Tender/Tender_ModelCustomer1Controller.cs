using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;
using System.IO;

namespace JinxiaocunApp.Controllers.Tender
{
    public class Tender_ModelCustomer1Controller : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        public bool isSmall = true;
        //
        // GET: /Tender_ModelCustomer1/
        [MultipleResponseFormats]
        public ActionResult Index(int id=0)
        {
            isSmall = isSmallByid(id);
            var tender_mcust = db.Tender_ModelCustomer1.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id);
            //第一次加载 生成客户初始数据
            if (tender_mcust.Count() <= 0)
            {
                List<Bemp_GongGao> list = db.Bemp_GongGaos.Include(b => b.bemplyees).Where(m => m.GongGaoId == id&&m.IsPassShen!=null&&m.IsPassShen==true).ToList();
                if (list.Count > 0)
                {
                    //List<Tender_ModelCustomer1> li = new List<Tender_ModelCustomer1>();
                    lock (this)
                    {
                        foreach (Bemp_GongGao bemp in list)
                        {
                            Tender_ModelCustomer1 cust = new Tender_ModelCustomer1();
                            cust.UserName = bemp.bemplyees.NumberEmp;
                            cust.LinkMan = bemp.bemplyees.LinkMan;
                            cust.LinkType = bemp.bemplyees.LinkType;
                            cust.EmpGonggaoID = bemp.BGid;
                            cust.PriceOne = 0;
                            cust.PriceTwo = 0;
                            cust.PriceThree = 0;
                            if (db.Tender_ModelCustomer1.Where(m => m.EmpGonggaoID == bemp.BGid).Count() <= 0)
                                db.Tender_ModelCustomer1.Add(cust);
                            //li.Add(cust);
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            db.Configuration.ValidateOnSaveEnabled = true;
                        }
                    }
                    


                }
            }

            
            //找每轮最低价
            //tender_mcust = db.Tender_ModelCustomer1.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id);

            ViewBag.PriceD1 = ViewBag.PriceD2=ViewBag.PriceD3 = 0;

            tender_mcust = db.Tender_ModelCustomer1.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id&&mc.PriceOne>0);
            if(tender_mcust.Count()>0)
            {
                if(isSmall)
                {
                    ViewBag.PriceD1 = tender_mcust.Min(m => m.PriceOne);
                }else
                {
                    ViewBag.PriceD1 = tender_mcust.Max(m => m.PriceOne);
                }
            }
            

            tender_mcust = db.Tender_ModelCustomer1.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id && mc.PriceTwo > 0);
            if (tender_mcust.Count() > 0)
            {
                if(isSmall)
                {
                    ViewBag.PriceD2 = tender_mcust.Min(m => m.PriceTwo);
                }else
                {
                    ViewBag.PriceD2 = tender_mcust.Max(m => m.PriceTwo);
                }
            }
            

            tender_mcust = db.Tender_ModelCustomer1.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id && mc.PriceThree > 0);
            if (tender_mcust.Count() > 0)
            {
                if(isSmall)
                {
                    ViewBag.PriceD3 = tender_mcust.Min(m => m.PriceThree);
                }else
                {
                    ViewBag.PriceD3 = tender_mcust.Max(m => m.PriceThree);
                }
            }
            

           
            //重新取数
            tender_mcust = db.Tender_ModelCustomer1.Include(t => t.bemp_Gonggao).Where(mc => mc.bemp_Gonggao.GongGaoId == id);

            if(isSmall)
            {
                //循环找最低价
                foreach (Tender_ModelCustomer1 custVar in tender_mcust.ToList<Tender_ModelCustomer1>())
                {
                    Bemp_GongGao bgong = db.Bemp_GongGaos.Find(custVar.EmpGonggaoID);
                    var v1 = custVar.PriceOne != 0 ? custVar.PriceOne : 9999999999;
                    var v2 = custVar.PriceTwo != 0 ? custVar.PriceTwo : 9999999999;
                    var v3 = custVar.PriceThree != 0 ? custVar.PriceThree : 9999999999;
                    bgong.LostPrice = (v1 < v2 ? v1 : v2) < v3 ? (v1 < v2 ? v1 : v2) : v3;
                    if (bgong.LostPrice != 9999999999)
                    {

                        db.Entry(bgong).State = EntityState.Modified;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        db.Configuration.ValidateOnSaveEnabled = true;
                    }
                    else
                    {
                        bgong.LostPrice = 0;
                    }

                }
            }else
            {
                //循环找最高价
                foreach (Tender_ModelCustomer1 custVar in tender_mcust.ToList<Tender_ModelCustomer1>())
                {
                    Bemp_GongGao bgong = db.Bemp_GongGaos.Find(custVar.EmpGonggaoID);
                    var v1 = custVar.PriceOne;
                    var v2 = custVar.PriceTwo;
                    var v3 = custVar.PriceThree;
                    bgong.LostPrice = (v1 > v2 ? v1 : v2) > v3 ? (v1 > v2 ? v1 : v2) : v3;
                    if (bgong.LostPrice != 0)
                    {

                        db.Entry(bgong).State = EntityState.Modified;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        db.Configuration.ValidateOnSaveEnabled = true;
                    }
                    else
                    {
                        bgong.LostPrice = 0;
                    }

                }
            }

            ViewBag.IsSmall = isSmall;
            
            return View(tender_mcust);
        }

        //
        // GET: /Tender_ModelCustomer1/Details/5

        public ActionResult Details(int id = 0)
        {
            Tender_ModelCustomer1 tender_modelcustomer1 = db.Tender_ModelCustomer1.Find(id);
            if (tender_modelcustomer1 == null)
            {
                return HttpNotFound();
            }
            return View(tender_modelcustomer1);
        }

        //
        // GET: /Tender_ModelCustomer1/Create

        [MultipleResponseFormats]
        public ActionResult Create(int id=0)
        {
           
            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid");
            if (id != 0)
            {
                
                Bemp_GongGao empGong =null;
                try
                {
                    empGong = db.Bemp_GongGaos.Include(m => m.Tender_GongGaos).Single(m => m.BGid == id && m.IsPassShen != null && m.IsPassShen == true);

                    isSmall = isSmallByid(empGong.GongGaoId);
                    ViewBag.IsSmall = isSmall;
                }catch(Exception ex)
                {
                    return Content("不能投标，您可能审核未通过。");
                }
                
                Tender_ModelManage1 modelM = db.Tender_ModelManage1.Single(m => m.Tid == empGong.GongGaoId);
               
                if(modelM==null)
                { return Content("投标未开始"); }

                TimeSpan tsEnd3 ;
                if(TimeSpan.TryParse(modelM.EndTime3,out tsEnd3))
                {
                    if(tsEnd3<=System.DateTime.Now.TimeOfDay)
                    {
                        return Content("投标已结束");
                    }
                }

                if (modelM.DateToday.Date < System.DateTime.Now.Date)
                {
                    return Content("投标已结束~!");
                }
                if (modelM.DateToday.Date > System.DateTime.Now.Date)
                {
                    return Content("投标未开始~!");
                }
                if (empGong != null)
                {
                    Tender_ModelCustomer1 cust;
                    cust = db.Tender_ModelCustomer1.Include(m=>m.bemp_Gonggao.Tender_GongGaos).Single(m => m.EmpGonggaoID == empGong.BGid);
                    if(cust==null)
                    {
                        return Content("请联系管理");
                        //cust=new Tender_ModelCustomer1();
                        //cust.EmpGonggaoID=id;
                        //cust.PriceUnit = modelM.PriceUnit;
                        //cust.LinkMan=empGong
                        //db.Tender_ModelCustomer1.Add(cust);
                        
                        //db.SaveChanges();
                    }
                    cust.PriceUnit = modelM.PriceUnit;
                    db.Entry(cust).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;

                    if ((!string.IsNullOrEmpty(modelM.BeginTime1)) && (!string.IsNullOrEmpty(modelM.EndTime1)) && TimeSpan.Parse(modelM.EndTime1)>=System.DateTime.Now.TimeOfDay)
                    {
                        ViewBag.Lunci = "一 未开始";
                        ViewBag.TimeXia = modelM.BeginTime1;
                        if((TimeSpan.Parse(modelM.BeginTime1) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime1)))
                        {
                            //第一轮进行中
                            ViewBag.Lunci = "一";
                            ViewBag.Jieshu =Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime1)-System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                            ViewBag.Shanglun = "";
                            ViewBag.ShangDi = "";
                            ViewBag.TimeXia = modelM.BeginTime2;
                        }
                        //第一轮未开始
                        ViewBag.TimeXia = modelM.BeginTime1;
                    }else if((!string.IsNullOrEmpty(modelM.BeginTime2)) && (!string.IsNullOrEmpty(modelM.EndTime2))&&(TimeSpan.Parse(modelM.EndTime1) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime2)))
                    {
                        ViewBag.Lunci = "二 未开始";
                        ViewBag.Shanglun = cust.PriceOne;
                        if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Count() > 0)
                        {
                            if(isSmall)
                            {
                                ViewBag.ShangDi = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Min(m => m.PriceOne);
                            }else
                            {
                                ViewBag.ShangDi = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Max(m => m.PriceOne);
                            }
                            
                        }
                        else
                        {
                            ViewBag.ShangDi = "";
                        }
                        ViewBag.TimeXia = modelM.BeginTime2;
                        if((TimeSpan.Parse(modelM.BeginTime2) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime2)))
                        {
                            //第二轮进行中
                            ViewBag.Lunci = "二";
                            ViewBag.Jieshu = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime2) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");

                            ViewBag.TimeXia = modelM.BeginTime3;
                        }
                        //第二轮未开始
                       
                    }else if((!string.IsNullOrEmpty(modelM.BeginTime3)) && (!string.IsNullOrEmpty(modelM.EndTime3))&&(TimeSpan.Parse(modelM.EndTime2) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime3)))
                    {
                        ViewBag.Lunci = "三 未开始";
                        ViewBag.Shanglun = cust.PriceTwo;
                        ViewBag.TimeXia = modelM.BeginTime3;
                        if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Count() > 0)
                        {
                            if(isSmall)
                            {
                                ViewBag.ShangDi = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Min(m => m.PriceTwo);
                            }else
                            {
                                ViewBag.ShangDi = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Max(m => m.PriceTwo);
                            }
                            
                        }
                        else
                        {
                            ViewBag.ShangDi = "";
                        }

                        if((TimeSpan.Parse(modelM.BeginTime3) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime3)))
                        {
                            //第三轮进行中
                            ViewBag.Lunci = "三";
                            ViewBag.Jieshu = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime3) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                            ViewBag.TimeXia ="";
                        }

                    }else
                    {
                        return Content("投标已结束。。。");
                    }
                    //if (qv.Count()>0)
                    //{
                    //    cust = qv.ElementAt(0);


                    //    if (cust.PriceThree != null && cust.PriceThree > 0)
                    //    {
                    //        return Content("此项目已投标~！");
                    //    }
                    //    else if (cust.PriceTwo != null && cust.PriceTwo > 0)
                    //    {
                    //        ViewBag.Lunci = "三";
                    //        ViewBag.Jieshu = System.DateTime.Now.TimeOfDay.ToString();
                    //        ViewBag.Shanglun = cust.PriceTwo;
                    //        ViewBag.ShangDi = "";
                    //    }
                    //    else if (cust.PriceOne != null && cust.PriceOne > 0)
                    //    {
                    //        ViewBag.Lunci = "二";
                    //        ViewBag.Jieshu = System.DateTime.Now.TimeOfDay.ToString();
                    //        ViewBag.Shanglun = cust.PriceTwo;
                    //        ViewBag.ShangDi = "";
                    //    }
                    //    else
                    //    {
                            
                    //        ViewBag.Lunci = "一";
                    //        ViewBag.Jieshu = System.DateTime.Now.TimeOfDay.ToString();
                    //        ViewBag.Shanglun = cust.PriceTwo;
                    //        ViewBag.ShangDi = "";
                    //    }
                    //}
                    //else
                    //{

                    //    if (modeM != null)
                    //    {
                    //        try
                    //        {
                    //            TimeSpan tsp = TimeSpan.Parse("10:35:00");
                    //            ViewBag.Lunci = "一";
                    //            ViewBag.Jieshu = tsp;
                    //            ViewBag.Shanglun = "";
                    //            ViewBag.ShangDi = "";
                    //        }
                    //        catch(Exception ex)
                    //        { return Content(ex.Message +" "+ modeM.BeginTime1+" "+System.DateTime.Now.TimeOfDay+" "+System.DateTime.Now.ToShortTimeString()); }

                    //        cust = new Tender_ModelCustomer1();
                    //        cust.bemp_Gonggao = empGong;
                    //        cust.EmpGonggaoID = empGong.BGid;
                    //    }
                    //    else
                    //    {

                    //        ViewBag.Lunci = "未开始";
                    //        cust = new Tender_ModelCustomer1();
                    //        cust.bemp_Gonggao = empGong;
                    //    }
                        
                    //}
                    
                    return View(cust);
                }
            }
            
            return View();
        }



        public bool isSmallByid(int Gonggaoid)
        {
            Tender_GongGao tGonggao = db.Tender_GongGaos.Single(m => m.TaskID == Gonggaoid);
            string typeStr = tGonggao.Types;
            if (typeStr.Trim().ToLower().Equals("e"))
            {
                return false;
            }else
            {
                return true;
            }
        }
        //
        // POST: /Tender_ModelCustomer1/Create

        [HttpPost]
        [ValidateAntiForgeryToken][MultipleResponseFormats]
        public ActionResult Create(Tender_ModelCustomer1 tender_modelcustomer1,string LunciLab="")
        {
            
            if (ModelState.IsValid)
            {
                
                int ttid = db.Bemp_GongGaos.Single(n => n.BGid == tender_modelcustomer1.EmpGonggaoID).GongGaoId;
                //**2014/4/1**
                isSmall = isSmallByid(ttid);
                
                //********** end **********

                Tender_ModelManage1 modelM = db.Tender_ModelManage1.Single(m => m.Tid == ttid);
                Tender_ModelCustomer1 tender_modelcustomer2 = db.Tender_ModelCustomer1.Single(m => m.EmpGonggaoID == tender_modelcustomer1.EmpGonggaoID);
                switch (LunciLab)
                { 
                    case "":
                        break;
                    case "一":
                        
                        if (TimeSpan.Parse(modelM.BeginTime1) <= System.DateTime.Now.TimeOfDay&&System.DateTime.Now.TimeOfDay  <= TimeSpan.Parse(modelM.EndTime1))
                        {
                            if (tender_modelcustomer2.PriceOne > 0)
                            {
                                return Content("每轮只能报价一次！");
                            }
                            //Bemp_GongGao bempG1 = db.Bemp_GongGaos.Single(n => n.BGid == tender_modelcustomer1.EmpGonggaoID);
                            tender_modelcustomer2.bemp_Gonggao.PayType = tender_modelcustomer1.bemp_Gonggao.PayType;
                            tender_modelcustomer2.bemp_Gonggao.HandDate = tender_modelcustomer1.bemp_Gonggao.HandDate;
                            tender_modelcustomer2.PriceOne = tender_modelcustomer1.PriceThree;
                            db.Entry(tender_modelcustomer2).State = EntityState.Modified;
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            db.Configuration.ValidateOnSaveEnabled =true;
                            return Content("投标完成！");
                        }
                        
                        
                        break;
                    case "二":
                        if (TimeSpan.Parse(modelM.BeginTime2) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime2))
                        {
                            if (tender_modelcustomer2.PriceTwo > 0)
                            {
                                //ceshi  要删掉
                                //tender_modelcustomer2.bemp_Gonggao.PayType = tender_modelcustomer1.bemp_Gonggao.PayType;
                                //db.Entry(tender_modelcustomer2).State = EntityState.Modified;
                                //db.Configuration.ValidateOnSaveEnabled = false;
                                //db.SaveChanges();
                                //db.Configuration.ValidateOnSaveEnabled = true;
                                return Content("每轮只能报价一次！");
                            }
                            if(isSmall)
                            {
                                if (tender_modelcustomer2.PriceOne != 0 && tender_modelcustomer1.PriceThree >= tender_modelcustomer2.PriceOne)
                                {
                                    return Content("投标价格必须小于上一轮");
                                }
                            }else
                            {
                                if (tender_modelcustomer2.PriceOne != 0 && tender_modelcustomer1.PriceThree <= tender_modelcustomer2.PriceOne)
                                {
                                    return Content("投标价格必须大于上一轮");
                                }
                            }
                            
                            tender_modelcustomer2.PriceTwo = tender_modelcustomer1.PriceThree;
                            tender_modelcustomer2.bemp_Gonggao.PayType = tender_modelcustomer1.bemp_Gonggao.PayType;
                            tender_modelcustomer2.bemp_Gonggao.HandDate = tender_modelcustomer1.bemp_Gonggao.HandDate;
                            db.Entry(tender_modelcustomer2).State = EntityState.Modified;
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            db.Configuration.ValidateOnSaveEnabled = true;
                            return Content("投标完成！");
                        }
                       
                        
                        break;

                    case "三":
                        if (TimeSpan.Parse(modelM.BeginTime3) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime3))
                        {
                            if (tender_modelcustomer2.PriceThree > 0)
                            {
                                return Content("每轮只能报价一次！");
                            }
                            if(isSmall)
                            {
                                if ((tender_modelcustomer2.PriceTwo != 0 && tender_modelcustomer1.PriceThree >= tender_modelcustomer2.PriceTwo) || (tender_modelcustomer2.PriceOne != 0 && tender_modelcustomer1.PriceThree >= tender_modelcustomer2.PriceOne))
                                {
                                    return Content("投标价格必须小于上一轮");
                                }
                            }else
                            {
                                if ((tender_modelcustomer2.PriceTwo != 0 && tender_modelcustomer1.PriceThree <= tender_modelcustomer2.PriceTwo) || (tender_modelcustomer2.PriceOne != 0 && tender_modelcustomer1.PriceThree <= tender_modelcustomer2.PriceOne))
                                {
                                    return Content("投标价格必须大于上一轮");
                                }
                            }
                            
                            tender_modelcustomer2.PriceThree = tender_modelcustomer1.PriceThree;
                            tender_modelcustomer2.bemp_Gonggao.PayType = tender_modelcustomer1.bemp_Gonggao.PayType;
                            tender_modelcustomer2.bemp_Gonggao.HandDate = tender_modelcustomer1.bemp_Gonggao.HandDate;
                            db.Entry(tender_modelcustomer2).State = EntityState.Modified;
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            db.Configuration.ValidateOnSaveEnabled = true;
                            return Content("投标完成！");
                        }
                        else
                        {
                            return Content("投标未开始");
                        }
                        //db.Entry(tender_modelcustomer1).State = EntityState.Modified;
                        break;
                }

                return Content("投标未开始~！");
                //db.SaveChanges();
             
            }

            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid", tender_modelcustomer1.EmpGonggaoID);
            return View(tender_modelcustomer1);
        }

        public ActionResult LoadData(int id=0)
        {
            Tender_CustomerData data = new Tender_CustomerData();
            data.StrServ = "";
            data.PriceShang = 0;
            data.PriceShangLow = 0;
            Bemp_GongGao empGong = db.Bemp_GongGaos.Include(m => m.Tender_GongGaos).Single(m => m.BGid == id);
            isSmall = isSmallByid(empGong.GongGaoId);
            ViewBag.IsSmall = isSmall;
            Tender_ModelManage1 modelM = db.Tender_ModelManage1.Single(m => m.Tid == empGong.GongGaoId);

            if (empGong != null)
            {
                Tender_ModelCustomer1 cust;
                cust = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Single(m => m.EmpGonggaoID == empGong.BGid);
                

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
                    data.PriceShang = cust.PriceOne;
                    if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Count() > 0)
                    {
                        if(isSmall)
                        {
                            data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Min(m => m.PriceOne);
                        }else
                        {
                            data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne > 0).Max(m => m.PriceOne);
                        }
                        
                        if(db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceOne==data.PriceShangLow).Count()>1)
                        {
                            data.StrServ = "请注意，此刻有多家供应商报出了相同的最低价。";
                        }
                    }
                    else
                    {
                        data.PriceShangLow = 0;
                    }
                    data.TimeXia = modelM.BeginTime2;
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
                    data.PriceShang = cust.PriceTwo;
                    data.TimeXia = modelM.BeginTime3;
                    if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Count() > 0)
                    {
                        if(isSmall)
                        {
                            data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Min(m => m.PriceTwo);
                        }else
                        {
                            data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo > 0).Max(m => m.PriceTwo);
                        }
                        
                        if (db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceTwo == data.PriceShangLow).Count() > 1)
                        {
                            data.StrServ = "请注意，此刻有多家供应商报出了相同的最低价。";
                        }
                    }
                    else
                    {
                        data.PriceShangLow = 0;
                    }

                    if ((TimeSpan.Parse(modelM.BeginTime3) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime3)))
                    {
                        //第三轮进行中
                        data.Lunci = "三";
                        data.EndTime = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime3) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                        data.TimeXia = "";
                    }

                }
                //Tender_ModelCustomer1 cust;
                //var qv = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.EmpGonggaoID == empGong.BGid);
                //if (qv.Count() > 0)
                //{
                //    cust = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Single(m => m.EmpGonggaoID == empGong.BGid);
                //    if (cust.PriceThree != null && cust.PriceThree > 0)
                //    {
                //        return Content("此项目已投标~！");
                //    }
                //    else if (cust.PriceTwo != null && cust.PriceTwo > 0)
                //    {
                //        data.Lunci = "三";
                //        data.TimeXia=modeM.BeginTime3;
                //        TimeSpan ts;
                //        if(TimeSpan.TryParse(modeM.BeginTime3,out ts))
                //        {
                //            if (ts <= System.DateTime.Now.TimeOfDay.Add(new TimeSpan(0,0,2)))
                //            {
                //                data.EndTime = (TimeSpan.Parse(modeM.EndTime3) - System.DateTime.Now.TimeOfDay).ToString("C");
                //            }
                //        }
                //        data.PriceShang = cust.PriceTwo;
                //        data.PriceShangLow = db.Tender_ModelCustomer1.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId).Max(m => m.PriceTwo);
                        
                //    }
                //    else if (cust.PriceOne != null && cust.PriceOne > 0)
                //    {
                //        ViewBag.Lunci = "二";
                //        ViewBag.Jieshu = System.DateTime.Now.TimeOfDay.ToString();
                //        ViewBag.Shanglun = cust.PriceTwo;
                //        ViewBag.ShangDi = "";
                //    }
                //    else
                //    {

                //        ViewBag.Lunci = "一";
                //        ViewBag.Jieshu = System.DateTime.Now.TimeOfDay.ToString();
                //        ViewBag.Shanglun = cust.PriceTwo;
                //        ViewBag.ShangDi = "";
                //    }
                //}
                //else
                //{

                //    if (modeM != null)
                //    {
                //        try
                //        {
                            

                //            data.Lunci = "一";
                //            data.TimeXia = modeM.BeginTime3;
                //            TimeSpan ts;
                //            if (TimeSpan.TryParse(modeM.BeginTime3, out ts))
                //            {
                //                if (ts <= System.DateTime.Now.TimeOfDay.Add(new TimeSpan(0, 0, 2)))
                //                {
                //                    data.EndTime = (TimeSpan.Parse(modeM.EndTime3) - System.DateTime.Now.TimeOfDay).ToString("C");
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        { return Content(ex.Message + " " + modeM.BeginTime1 + " " + System.DateTime.Now.TimeOfDay + " " + System.DateTime.Now.ToShortTimeString()); }

                //        cust = new Tender_ModelCustomer1();
                //        cust.bemp_Gonggao = empGong;
                //    }
                //    else
                //    {

                //        data.Lunci = "一";
                //    }

                //}

            }

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Tender_ModelCustomer1/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Tender_ModelCustomer1 tender_modelcustomer1 = db.Tender_ModelCustomer1.Find(id);
            if (tender_modelcustomer1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid", tender_modelcustomer1.EmpGonggaoID);
            return View(tender_modelcustomer1);
        }

        //
        // POST: /Tender_ModelCustomer1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tender_ModelCustomer1 tender_modelcustomer1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tender_modelcustomer1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpGonggaoID = new SelectList(db.Bemp_GongGaos, "BGid", "BGid", tender_modelcustomer1.EmpGonggaoID);
            return View(tender_modelcustomer1);
        }

        //
        // GET: /Tender_ModelCustomer1/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Tender_ModelCustomer1 tender_modelcustomer1 = db.Tender_ModelCustomer1.Find(id);
            if (tender_modelcustomer1 == null)
            {
                return HttpNotFound();
            }
            return View(tender_modelcustomer1);
        }

        //
        // POST: /Tender_ModelCustomer1/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tender_ModelCustomer1 tender_modelcustomer1 = db.Tender_ModelCustomer1.Find(id);
            db.Tender_ModelCustomer1.Remove(tender_modelcustomer1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [MultipleResponseFormats]
        public ActionResult CustFiles(int id=0)
        {
            Tender_CustomerFiles custF = new Tender_CustomerFiles();
            custF.Gid = id;
            return View(custF);
        }

        [HttpPost]
        public ActionResult CustFiles(Tender_CustomerFiles custf, HttpPostedFileBase[] fileToUpload, int id = 0, int Gid=0)
        {
            if (id == 3)
                return Content("");
            

            HttpPostedFileBase file =null;
            if (fileToUpload != null)
                file=fileToUpload[0];
            if(file!=null&&!String.IsNullOrEmpty(file.FileName))
            {
                custf.CustFile = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (custf.CustFile.Substring(0, 4) + @"\" + custf.CustFile.Substring(4, 4) + @"\")));
                file.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (custf.CustFile.Substring(0, 4) + @"\" + custf.CustFile.Substring(4, 4) + @"\" + custf.CustFile.Substring(8))));
                GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",custf.CustFile,"DbType.String",null },
                    { "@Name",Path.GetFileName(file.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(file.FileName),"DbType.String",null },
                    { "@Size",file.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
            }

            

            db.Tender_CustomerFiless.Add(custf);
            db.SaveChanges();
            //string fPath = @"D://myupload/" + file.FileName;
            //file.SaveAs(fPath);
            //string custT="nul";
            //if(custf!=null)
            //{
            //    custT = custf.CustText;
            //}
            return Content("保存成功");
            //return View(custf);
        }

        [MultipleResponseFormats]
        public ActionResult IndexFile(int id=0)
        {

            return View(db.Tender_CustomerFiless.Where(m => m.Gid == id));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}