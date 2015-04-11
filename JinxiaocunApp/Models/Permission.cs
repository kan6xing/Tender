using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JinxiaocunApp.Models
{
    [Table("webpages_Permission")]
    public class Permission
    {
        public Permission()
        {
            PermissionsInRoles = new List<PermissionsInRoles>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "权限ID")]
        public int PermissionId { get; set; }

        [Display(Name = "名称")]
        public string PermissionName { get; set; }

        [Display(Name = "父ID")]
        public int SuperPerid { get; set; }

        [Display(Name = "标记")]
        public string PermissionToken { get; set; }

        //[ForeignKey("SuperPerid")]
        //public Permission permission { get; set; }

        [ForeignKey("PermissionId")]
        public ICollection<PermissionsInRoles> PermissionsInRoles { set; get; }
    }
}