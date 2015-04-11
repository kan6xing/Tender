using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace JinxiaocunApp.Models
{
    [Table("webpages_Roles")]
    public class Role
    {
        public Role()
        {
            Emplyees = new List<BEmplyee>();
            PermissionsInRoles = new List<PermissionsInRoles>();
        }

        [Key]
        [Display(Name = "ID")]
        public int RoleId { get; set; }

        [Display(Name = "名称")]
        [StringLength(256)]
        public string RoleName { get; set; }

        [Display(Name = "描述")]
        public string RoleDesc { get; set; }

        
        //public List<BMembership> Members { get; set; }

        [ForeignKey("RoleId")]
        public ICollection<PermissionsInRoles> PermissionsInRoles { set; get; }

        //public int UserId { get; set; }

        [UIHint("IEnumerable")]
        public List<BEmplyee> Emplyees { get; set; }


        public string GetUsers()
        {
            
            
            string allN = "";
            foreach (BEmplyee emp in Emplyees)
            {
                //allN += emp.FullNameEmp + ",";
                allN += emp.NumberEmp + ",";
            }

            
            if (!string.IsNullOrEmpty(allN))
            {
                allN = allN.Substring(0, allN.Length - 1);
            }
            return allN;
        }

        
    }
}