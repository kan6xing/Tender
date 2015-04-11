using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Models
{
    public class Tender_Purchase_GongGao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        public Nullable<int> TaskID { get; set; }

        [Display(Name="物资名称")]
        public string NameP { get; set; }

        [Display(Name = "规格")]
        public string SpecP { get; set; }

        [Display(Name = "数量")]
        public Nullable<decimal> CountP { get; set; }

        [Display(Name = "单位")]
        public string UnitP { get; set; }

        [Display(Name = "备注说明")]
        public string RemarkP { get; set; }

        [ForeignKey("TaskID")]
        public Tender_GongGao tender_gonggao { get; set; }
    }
}