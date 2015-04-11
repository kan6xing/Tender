using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class Tender_TongZhi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TZID { get; set; }

        [Display(Name="标题")]
        [Required]
        public string TitleT { get; set; }

        [Display(Name = "发表日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? CreateDateT { get; set; }

        [Display(Name="内容")]
        public string ContentT { get; set; }

        [Display(Name="文件上传")]
        public string CustFile { get; set; }

        public string CustName { get; set; }
    }
}