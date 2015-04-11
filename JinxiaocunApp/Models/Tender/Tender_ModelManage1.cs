using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class Tender_ModelManage1
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MID { get; set; }

        [Required]
        [Display(Name="价格单位")]
        public string PriceUnit { get; set; }

        public DateTime DateToday { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name = "第一轮投标开始时间")]
        public string BeginTime1 { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "第一轮投标结束时间")]
        public string EndTime1 { get; set; }

        [Display(Name = "第二轮投标开始时间")]
        public string BeginTime2 { get; set; }

        [Display(Name = "第二轮投标结束时间")]
        public string EndTime2 { get; set; }

        [Display(Name = "第三轮投标开始时间")]
        public string BeginTime3 { get; set; }

        [Display(Name = "第三轮投标结束时间")]
        public string EndTime3 { get; set; }

        public int Tid{get;set;}

        [ForeignKey("Tid")]
        public Tender_GongGao tenderGonggao { get; set; }
    }
}