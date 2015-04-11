using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    [Table("UserProfile")]
    public class BEmplyee
    {
        [Key]
        [Display(Name = "ID")]
        [Column("UserId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpID { get; set; }

        [Column("UserName")]
        [Display(Name = "公司名称")]
        [Required]
        [Remote("IsExist", "RemoteAttr", AdditionalFields = "EmpID", ErrorMessage = "单位名称 重复")]
        public string NumberEmp { get; set; }

        //[Display(Name = "用户全名")]
        //[Required]
        //public string FullNameEmp { get; set; }

        //[Display(Name = "用户简名")]
        //public string SmallNameEmp { get; set; }

        //[Display(Name = "拼音码")]
        //public string PinyinEmp { get; set; }

        //[ForeignKey("bdepartment")]
        //public int bdepartment_BID { get; set; }
        //[Display(Name = "所属部门")]
        //public BDepartment bdepartment { get; set; }

        //[Display(Name = "联系电话")]
        //public string TelEmp { get; set; }

        //[Display(Name="状态")]
        //public string StateEmp { get; set; }

        //[Display(Name="备注")]
        //[DataType(DataType.Text)]
        //public string RemarkEmp { get; set; }

        [Display(Name="注册日期")]
        public DateTime RegisterDate { get; set; }



        //***********新内容*************

        [Display(Name = "联系人")]
        [Required]
        public string LinkMan { get; set; }

        [Display(Name = "联系方式")]
        [Required]
        public string LinkType { get; set; }

        [Display(Name = "注册资本")]
        [Required]
        public string RegisterMoney { get; set; }

        [Display(Name = "是否通过年检")]
        [Required]
        public bool PassYearCheck { get; set; }

        [Display(Name = "经营类别")]
        [Required]
        public string CompanyType { get; set; }

        [Display(Name = "法定代表人证明书或授权委托书（须有法人代表签名）")]
        [Required(ErrorMessage="此项必须上传！")]
        public string DelegateBook { get; set; }

        [Display(Name = "工商营业执照")]
        [Required]
        public string LicenceAtt { get; set; }

        [Display(Name = "组织机构代码证")]
        [Required]
        public string CodeAtt { get; set; }


        [Display(Name = "税务登记证书")]
        [Required]
        public string TaxAtt { get; set; }


        [Display(Name = "经营资质证书")]
        [Required]
        public string CertAtt { get; set; }

        [Display(Name = "企业简介")]
        [Required]
        public string DescsData { get; set; }

        [Display(Name="承诺书")]
        [Required]
        public string PromiseAtt { get; set; }

        [Display(Name = "保密协议")]
        [Required]
        public string SecretAtt { get; set; }

        [Display(Name = "一般纳税人资格证明")]
        public string PeopleAtt { get; set; }

        [Display(Name = "其他说明")]
        public string ItRemark { get; set; }

        [Display(Name = "投标次数")]
        [ScaffoldColumn(false)]
        public int TenderCount { get; set; }

        [Display(Name="中标次数")]
        [ScaffoldColumn(false)]
        public int TenderOKCount { get; set; }

        [ScaffoldColumn(false)][Display(Name="是否通过验证")]
        public bool isPass { get; set; }

        [Display(Name="是否删除")]
        public bool? isDelete { get; set; }

        [Display(Name = "是否删除")]
        public string DeleteCause { get; set; }

        //************ end ***************

        //public int RoleId { get; set; }

        //[ForeignKey("RoleId")]
        public List<Role> Roles { get; set; }


        public List<Bemp_GongGao> Bemp_GongGaos { get; set; }


        public void CopyFrom(BEmplyee bempf)
        {
            //this.FullNameEmp = bempf.FullNameEmp;
            //this.SmallNameEmp = bempf.SmallNameEmp;
            this.NumberEmp = bempf.NumberEmp;
            //this.PinyinEmp = bempf.PinyinEmp;
            //this.bdepartment_BID = bempf.bdepartment_BID;
            //this.TelEmp = bempf.TelEmp;
            //this.StateEmp = bempf.StateEmp;
            //this.RemarkEmp = bempf.RemarkEmp;

            //this.RegisterDate = bempf.RegisterDate;
            this.RegisterMoney = bempf.RegisterMoney;
            this.TaxAtt = bempf.TaxAtt;
            this.TenderCount = bempf.TenderCount;
            this.TenderOKCount = bempf.TenderOKCount;
            this.PassYearCheck = bempf.PassYearCheck;
            this.LinkType = bempf.LinkType;
            this.LinkMan = bempf.LinkMan;
            this.LicenceAtt = bempf.LicenceAtt;
            this.ItRemark = bempf.ItRemark;
            this.isPass = bempf.isPass;
            this.DescsData = bempf.DescsData;
            this.DelegateBook = bempf.DelegateBook;
            this.CompanyType = bempf.CompanyType;
            this.CodeAtt = bempf.CodeAtt;
            this.CertAtt = bempf.CertAtt;

            this.PromiseAtt = bempf.PromiseAtt;
            this.SecretAtt = bempf.SecretAtt;
            this.PeopleAtt = bempf.PeopleAtt;
        }
    }
}