using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class BLinkCompany
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LinkCompanyID { get; set; }

        
        public int ParentID { get; set; }

        //public BLinkCompany linkCompany { get; set; }

        [Required]
        [Display(Name="单位编号")]
        public string NumberLinkComp { get; set; }

        [Required]
        [Display(Name="单位全名")]
        public string FullNameLinkComp { get; set; }

        [Display(Name = "单位简名")]
        public string SmallNameLinkComp { get; set; }

        [Display(Name = "类型")]
        public string TypeLinkComp { get; set; }

        [Display(Name = "拼音码")]
        public string PinyinLinkComp { get; set; }

        [Display(Name = "所属地区")]
        public string AreaLinkComp { get; set; }

        [Display(Name = "地址")]
        public string AddressLinkComp { get; set; }

        [Display(Name = "税号")]
        public string DutyCodeLinkComp { get; set; }

        [Display(Name = "电话")]
        public string TelLinkComp { get; set; }

        [Display(Name = "手机")]
        public string PhoneLinkComp { get; set; }

        [Display(Name = "电子邮件")]
        public string EmailLinkComp { get; set; }

        [Display(Name = "联系人")]
        public string LinkManLinkComp { get; set; }

        [Display(Name = "价格等级")]
        public string PriceLevelLinkComp { get; set; }

        [Display(Name = "状态")]
        public string StateLinkComp { get; set; }

        [Display(Name = "开户银行")]
        public string BankLinkComp { get; set; }

        [Display(Name = "银行账户")]
        public string BankAccountLinkComp { get; set; }

        [Display(Name = "换货天数")]
        public string BarterDaysLinkComp { get; set; }

        [Display(Name = "换货比例(%)")]
        [Range(0,100,ErrorMessage="{0}只能在1-100之间")]
        public int BarterPercentLinkComp { get; set; }

        [Display(Name = "备注")]
        public string RemarkLinkComp { get; set; }
        
    }
}