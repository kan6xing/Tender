using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Models
{
    public class Tender_PingShen_User
    {
        public int TaskID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }


        public string UserName { get; set; }
        public string LinkMan { get; set; }

        public string LinkType { get; set; }

        public bool isNew { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool isPass { get; set; }
    }
}