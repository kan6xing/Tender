using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class BStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreID { get; set; }

        [Display(Name="仓库编码")]
        [Required]
        [MaxLength(50)]
        public string NumberStore { get; set; }

        [Display(Name="仓库全名")]
        [Required]
        public string FullNameStore { get; set; }

        [Display(Name="仓库简名")]
        public string SmallNameStore { get; set; }

        [Display(Name="拼音码")]
        public string PinyinStore { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name="状态")]
        public string StateStore { get; set; }

        [Display(Name="备注")]
        public string RemarkStore { get; set; }

        //**************发货信息************

        [Display(Name = "联系人")]
        public string LinkManStore { get; set; }

        [Display(Name = "邮政编码")]
        public string PostalcodeStore { get; set; }

        [Display(Name = "联系电话")]
        public string LinkTelStore { get; set; }

        [Display(Name = "发货地址")]
        public string SendAddressStore { get; set; }
    }
}