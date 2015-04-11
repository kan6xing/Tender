using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public partial class Tender_ShenQing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TaskID { get; set; }
        public string CompanyT { get; set; }
        public string PartmentT { get; set; }
        public string ProposerT { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Types { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<decimal> Counts { get; set; }
        public string Unit { get; set; }
        public string SN { get; set; }
        public string Name { get; set; }
        public string Descs { get; set; }
        public string Remark { get; set; }

        public bool? IsGongg { get; set; }
    }
}
