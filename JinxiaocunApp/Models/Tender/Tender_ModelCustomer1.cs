using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class Tender_ModelCustomer1
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MID { get; set; }

        [Display(Name="第一轮报价")]
        public decimal PriceOne { get; set; }

        [Display(Name = "第二轮报价")]
        public decimal PriceTwo { get; set; }

        
        [Display(Name = "本次报价")]
        [Required]
        [Range(0.01, 999999999, ErrorMessage = "报价必须大于{1}")]
        public decimal PriceThree { get; set; }

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

    public class Tender_CustomerFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TID { get; set; }
        [Display(Name = "报价清单")]
        [Required]
        public string CustText { get; set; }

        [Display(Name = "文件上传")]
        public string CustFile { get; set; }

        public int? Gid { get; set; }
    }
}