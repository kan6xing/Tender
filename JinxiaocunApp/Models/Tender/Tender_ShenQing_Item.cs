using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public partial class Tender_ShenQing_Item
    {
        public Nullable<int> TaskID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Descs { get; set; }
        public string Atts { get; set; }

        public bool IsOpen { get; set; }

        [ForeignKey("TaskID")]
        public Tender_ShenQing tenderShenqing { get; set; }
    }
}
