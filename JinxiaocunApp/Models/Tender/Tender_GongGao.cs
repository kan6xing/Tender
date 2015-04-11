using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public partial class Tender_GongGao
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TaskID { get; set; }

        [Display(Name="项目编号")]
        public string SN { get; set; }
        public string SSN { get; set; }

        [Display(Name = "发布单位")]
        public string PartmentT { get; set; }

        [Display(Name = "发布人")]
        public string ProposerT { get; set; }

        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}")]
        [Display(Name = "发布日期")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "开始日期")]
        public Nullable<System.DateTime> BeginDate { get; set; }

        [Display(Name = "结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "资质评定截止日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> AssessEndDate { get; set; }

        [Display(Name = "项目名称")]
        public string Name { get; set; }

        [Display(Name = "采购类型")]
        public string Types { get; set; }

        [Display(Name = "项目简介")]
        public string Descs { get; set; }

        [Display(Name = "要求付款方式")]
        public string PayType { get; set; }

        [Display(Name = "数量")]
        public Nullable<decimal> Counts { get; set; }

        [Display(Name = "单位")]
        public string Unit { get; set; }

        [Display(Name = "项目结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> ProjectEndDate { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "资质验证")]
        public bool IsShenhe { get; set; }

        [Display(Name="招标模式")]
        public string TenderModel { get; set; }

        [Display(Name="投标开始时间")]
        public string BeginTime { get; set; }

        [Display(Name = "投标结束时间")]
        public string EndTime { get; set; }

        [Display(Name="保证金金额")]
        public Nullable<decimal> CautionMoney { get; set; }

        public Nullable<decimal> PriceLost { get; set; }

        public Nullable<decimal> PriceTalk { get; set; }

        public bool? IsKaib { get; set; }

        public bool? IsPingb { get; set; }

        [Display(Name = "投标状态")]
        public bool IsZhongb { get; set; }

        [Display(Name="删除标记")]
        public bool? IsDelete { get; set; }
        public List<Bemp_GongGao> Bemp_GongGaos { get; set; }

        public List<Tender_GongGao_Item> Tender_GongGao_Items { get; set; }

        public List<Tender_Purchase_GongGao> Tender_Purchase_GongGaos { get; set; }
    }
}
