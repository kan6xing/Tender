using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Models
{
    public class Tender_PingShen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TaskID { get; set; }

        [Display(Name = "项目编号")]
        public string SN { get; set; }
        public string SSN { get; set; }

        [Display(Name = "发布单位")]
        public string PartmentT { get; set; }

        [Display(Name = "发布人")]
        public string ProposerT { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "发布日期")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "招标开始日期")]
        public Nullable<System.DateTime> BeginDate { get; set; }

        [Display(Name = "招标结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "供应商资质评定截止日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> AssessEndDate { get; set; }

        [Display(Name = "项目名称")]
        public string Name { get; set; }

        [Display(Name = "招标类型")]
        public string Types { get; set; }

        [Display(Name = "项目简介")]
        public string Descs { get; set; }

        [Display(Name = "付款方式")]
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
    }
}