using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;
using System.Data.Entity;
using System.Data;

namespace JinxiaocunApp.Controllers
{
    public class Tender_ModelManage2Controller : Controller
    {
        //
        // GET: /Tender_ModelManage2/
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        public ActionResult Index()
        {
            return View();
        }

        [MultipleResponseFormats]
        public ActionResult Create(int TaskID = 1)
        {

            if (TaskID != 1)
            {
                Tender_GongGao tg = db.Tender_GongGaos.Find(TaskID);
                if (tg != null)
                {
                    if (tg.IsShenhe != true)
                    {
                        return Content("尚未进行供应商投标审核,不能投标~！");
                    }

                    Tender_GongGao tgonggao = db.Tender_GongGaos.Find(TaskID);
                    if (tgonggao.TenderModel!=null&&tgonggao.TenderModel.Trim().Equals("三轮报价模式"))
                    {
                        return Content("您已选择三轮报价模式，不能再选择竞价模式");
                    }
                    tgonggao.TenderModel = "竞价模式";
                    db.Entry(tgonggao).State = EntityState.Modified;
                    db.SaveChanges();

                    int mcount = db.Tender_ModelManage2.Where(m => m.Tid == TaskID).Count();
                    if (mcount > 0)
                    {
                        ViewBag.ggM = tg;
                        Tender_ModelManage2 mod2 = db.Tender_ModelManage2.Single(m => m.Tid == TaskID);
                        mod2.Tid = TaskID;
                        return View(mod2);
                        //return Content("已经投标 请不要重复投标");
                    }
                    ViewBag.ggM = tg;
                    Tender_ModelManage2 mod1 = new Tender_ModelManage2();
                    mod1.Tid = TaskID;
                    mod1.PriceUnit = "元/标";
                    return View(mod1);
                    //mod1.tenderGonggao = tg;
                    //ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN");
                    //return View(mod1);
                }
                else
                {
                    return Content("请求页面异常~！");
                }
            }
            return Content("如有问题 请联系管理员");
            //ViewBag.Tid = new SelectList(db.Tender_GongGaos, "TaskID", "SN");
            //return View();

        }

        [MultipleResponseFormats]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tender_ModelManage2 tender_modelmanage1,string btnAction="")
        {
            Tender_ModelManage2 tender_modelmanage2;
            switch (btnAction)
            { 
                case "btn1":
                    if (db.Tender_ModelManage2.Where(m => m.Tid == tender_modelmanage1.Tid).Count() > 0)
                    {
                        tender_modelmanage2 = db.Tender_ModelManage2.Single(m => m.Tid == tender_modelmanage1.Tid);
                        tender_modelmanage2.BeginTime = tender_modelmanage1.BeginTime;
                        tender_modelmanage2.EndTime = tender_modelmanage1.EndTime;
                        tender_modelmanage2.PriceUnit = tender_modelmanage1.PriceUnit;
                        db.Entry(tender_modelmanage2).State = EntityState.Modified;
                        db.SaveChanges();
                        return Content("时间确认成功");
                    }

                    var empGs = db.Bemp_GongGaos.Where(m => m.GongGaoId == tender_modelmanage1.Tid);
                    if(empGs.Count()>0)
                    {
                        foreach(Bemp_GongGao bemp in empGs)
                        {
                            bemp.PriceUnit = tender_modelmanage1.PriceUnit;
                            db.Entry(bemp).State = EntityState.Modified;
                            //db.SaveChanges();    
                        }
                    }

                    tender_modelmanage1.DateToday = System.DateTime.Now.Date;
                    db.Tender_ModelManage2.Add(tender_modelmanage1);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                    return Content("时间确认成功");
                    break;
                case "btn2":
                    break;
                default:
                    break;
            }

            return Content("有问题 ，请联系管理！");
        }

        public ActionResult LoadData(int id = 0)
        {
            Tender_CustomerData data = new Tender_CustomerData();
            data.StrServ = "";
            //Bemp_GongGao empGong = db.Bemp_GongGaos.Include(m => m.Tender_GongGaos).Single(m => m.BGid == id);
            try
            {
                Tender_ModelManage2 modelM = db.Tender_ModelManage2.Single(m => m.Tid == id);

                if ((TimeSpan.Parse(modelM.BeginTime) <= System.DateTime.Now.TimeOfDay && System.DateTime.Now.TimeOfDay <= TimeSpan.Parse(modelM.EndTime)))
                {
                    data.EndTime = Convert.ToDateTime((TimeSpan.Parse(modelM.EndTime) - System.DateTime.Now.TimeOfDay).ToString()).ToString("HH:mm:ss");
                    //data.PriceShangLow = db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost > 0).Min(m => m.PriceLost);

                    //if (db.Tender_ModelCustomer2.Include(m => m.bemp_Gonggao.Tender_GongGaos).Where(m => m.bemp_Gonggao.GongGaoId == empGong.GongGaoId && m.PriceLost == data.PriceShangLow).Count() > 1)
                    //{
                    //    data.StrServ = "请注意，此刻有多家供应商报出了相同的最低价。";
                    //}
                    //data.TimeXia = modelM.BeginTime;
                }
            }catch(Exception ex)
            {

            }
            

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
