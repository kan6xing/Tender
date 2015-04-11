using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class Tender_ModelManage2
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MID { get; set; }

        [Display(Name = "价格单位")]
        [Required(ErrorMessage = "*")]
        public string PriceUnit { get; set; }
        public DateTime DateToday { get; set; }

        [Display(Name = "投标开始时间")]
        [Required(ErrorMessage="*")]
        public string BeginTime { get; set; }

        [Display(Name = "投标结束时间")]
        [Required(ErrorMessage = "*")]
        public string EndTime { get; set; }

        //[Display(Name = "第二轮投标开始时间")]
        //public string BeginTime2 { get; set; }

        //[Display(Name = "第二轮投标结束时间")]
        //public string EndTime2 { get; set; }

        //[Display(Name = "第三轮投标开始时间")]
        //public string BeginTime3 { get; set; }

        //[Display(Name = "第三轮投标结束时间")]
        //public string EndTime3 { get; set; }

        public int Tid{get;set;}

        [ForeignKey("Tid")]
        public Tender_GongGao tenderGonggao { get; set; }
    }
}