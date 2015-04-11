using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Models
{
    public class Tender_CompanyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompID { get; set; }

        public int EmpID { get; set; }

        [ForeignKey("EmpID")]
        public BEmplyee bemplyee { get; set; }

        [Display(Name="单位名称")]
        public string CompName { get; set; }

        [Display(Name = "联系人")]
        public string LinkMan { get; set; }

        [Display(Name = "联系方式")]
        public string LinkType { get; set; }

        [Display(Name = "注册资本")]
        public string RegisterMoney { get; set; }

        [Display(Name = "是否通过年检")]
        public bool? PassYearCheck { get; set; }

        [Display(Name = "经营类别")]
        public string CompanyType { get; set; }

        [Display(Name = "法定代表人证明书或授权委托书（须有法人代表签名）")]
        public string DelegateBook { get; set; }

        [Display(Name = "工商营业执照")]
        public string LicenceAtt { get; set; }

        [Display(Name = "组织机构代码证")]
        public string CodeAtt { get; set; }

        [Display(Name = "税务登记证书")]
        public string TaxAtt { get; set; }

        [Display(Name = "经营资质证书")]
        public string CertAtt { get; set; }

        [Display(Name = "资质简介等资料")]
        public string DescsData { get; set; }

        [Display(Name = "其他说明")]
        public string ItRemark { get; set; }
    }
}