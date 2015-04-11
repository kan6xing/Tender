using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Models
{
    public class Tender_ModelCustomer2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MID { get; set; }

        [Display(Name="投标报价")]
        [Required]
        [Range(0.01, 999999999, ErrorMessage = "报价必须大于{1}")]
        public decimal PriceLost { get; set; }

        [Display(Name = "价格单位")]
        public string PriceUnit { get; set; }

        [Display(Name = "竞标供应商名称")]
        public string UserName { get; set; }

        [Display(Name = "联系人")]
        public string LinkMan { get; set; }

        [Display(Name = "联系方式")]
        public string LinkType { get; set; }

        public int EmpGonggaoID { get; set; }

        [ForeignKey("EmpGonggaoID")]
        public Bemp_GongGao bemp_Gonggao { get; set; }
    }
}