using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Models
{
    public class Bemp_GongGao
    {
        public Bemp_GongGao()
        {
            this.PayType = "";
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BGid { get; set; }
        public int UserId { get; set; }

       
        public int GongGaoId { get; set; }

        [ForeignKey("UserId")]
        public BEmplyee bemplyees { get; set; }

        [ForeignKey("GongGaoId")]
        public Tender_GongGao Tender_GongGaos { get; set; }

        public Nullable<decimal> LostPrice { get; set; }

        [Display(Name = "中标价格")]
        public Nullable<decimal> TalkPrice { get; set; }

        public string UserName { get; set; }

        [Display(Name="价格单位")]
        public string PriceUnit { get; set; }

        public string LinkMan { get; set; }

        public string LinkType { get; set; }

        //[Display(Name="承诺付款方式")][Required(ErrorMessage="{0} 必须填写。 ")]
        public string PayType { get; set; }

        [Display(Name = "承诺交货周期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date,ErrorMessage="{0}格式错误。 ")]
        [Required(ErrorMessage = "{0} 必须填写。 ")]
        public DateTime? HandDate { get; set; }

        [Display(Name="中标状态")]
        public bool IsZhongb { get; set; }

        [Display(Name = "验证状态")]
        public bool IsPassShen { get; set; }

        
    }
}