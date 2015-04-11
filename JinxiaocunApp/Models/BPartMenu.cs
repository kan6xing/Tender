using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class BPartMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BID { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string BName { get; set; }

        [Display(Name = "别名")]
        public string BOtherName { get; set; }

        [Display(Name = "父节点")]
        public int BParentID { get; set; }

        [Display(Name = "链接")]
        public string BLinkUrl { get; set; }

        [Display(Name = "链接方式")]
        public string BOpenModel { get; set; }
    }
}