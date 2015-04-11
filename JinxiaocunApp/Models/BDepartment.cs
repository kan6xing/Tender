using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace JinxiaocunApp.Models
{
    public class BDepartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BID { get; set; }

        [Required]
        [Display(Name="部门编码")]
        [Remote("IsExist", "RemoteAttr", AdditionalFields = "BID", ErrorMessage = "部门编码 重复")]
        public string BNumber { get; set; }

        [Required]
        [Display(Name = "部门全称")]
        [Remote("IsExist", "RemoteAttr",AdditionalFields="BID", ErrorMessage = "部门全称 重复")]
        public string BFullName { get; set; }

        [Display(Name = "部门简称")]
        public string BSmallName { get; set; }

        [Display(Name = "拼音码")]
        public string BPinyin { get; set; }

        [Display(Name="备注")]
        [DataType(DataType.Text)]
        public string BRemark { get; set; }

        //public List<BEmplyee> bemplyee { get; set; }
    }
}