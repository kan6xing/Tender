using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinxiaocunApp.Models
{
    public class BProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        
        public int ParentID { get; set; }

        [Display(Name="商品编号")]
        [Required]
        public string NumberProdu { get; set; }

        [Display(Name = "商品全名")]
        [Required]
        public string FullNameProdu { get; set; }

        [Display(Name = "简  名")]
        public string SmallNameProdu { get; set; }

        [Display(Name = "拼音码")]
        public string PinyinProdu { get; set; }

        
        [Display(Name = "产地")]
        public string AddressProdu { get; set; }

        
        [Display(Name = "规格")]
        public string GuigeProdu { get; set; }

        
        [Display(Name = "型号")]
        public string XinghaoProdu { get; set; }

        
        [Display(Name = "重量")]
        public decimal WeightProdu { get; set; }

        
        [Display(Name = "编号递增")]
        public bool IsOrderProdu { get; set; }

        
        [Display(Name = "序列号管理")]
        public string NumberManagerProdu { get; set; }

        [Display(Name = "供货信息")]
        public string SupplyInfoProdu { get; set; }

        [Display(Name = "备注")]
        public string RemarkProdu { get; set; }

        //***************基本信息**********

        [Display(Name = "单位名称")]
        public string UnitNameProdu { get; set; }

        [Display(Name = "单位关系")]
        public int UnitRelatinProdu { get; set; }

        [Display(Name = "条码")]
        public string BarcodeProdu { get; set; }

        [Display(Name = "商家编号")]
        public string SellerNumberProdu { get; set; }

        [Display(Name = "零售价")]
        public decimal RetailPriceProdu { get; set; }

        [Display(Name = "预设售价1")]
        public decimal PreparePrice1Produ { get; set; }

        [Display(Name = "预设售价2")]
        public decimal PreparePrice2Produ { get; set;}

        [Display(Name = "预设售价3")]
        public decimal PreparePrice3Produ { get; set; }

        [Display(Name = "预设售价4")]
        public decimal PreparePrice4Produ { get; set; }

        [Display(Name = "预设售价5")]
        public decimal PreparePrice5Produ { get; set; }

        //***************基本信息1**********
        public string UnitNameProdu1 { get; set; }

        public int UnitRelatinProdu1 { get; set; }

        public string BarcodeProdu1 { get; set; }

        public string SellerNumberProdu1 { get; set; }

        public decimal RetailPriceProdu1 { get; set; }

        public decimal PreparePrice1Produ1 { get; set; }

        public decimal PreparePrice2Produ1 { get; set; }

        public decimal PreparePrice3Produ1 { get; set; }

        public decimal PreparePrice4Produ1 { get; set; }

        public decimal PreparePrice5Produ1 { get; set; }

        //***************基本信息2**********
        public string UnitNameProdu2 { get; set; }

        public int UnitRelatinProdu2 { get; set; }

        public string BarcodeProdu2 { get; set; }

        public string SellerNumberProdu2 { get; set; }

        public decimal RetailPriceProdu2 { get; set; }

        public decimal PreparePrice1Produ2 { get; set; }

        public decimal PreparePrice2Produ2 { get; set; }

        public decimal PreparePrice3Produ2 { get; set; }

        public decimal PreparePrice4Produ2 { get; set; }

        public decimal PreparePrice5Produ2 { get; set; }

        //*************image***********

        public string Pic1Produ { get; set; }

        public string Pic2Produ { get; set; }

        public string Pic3Produ { get; set; }

        public string Pic4Produ { get; set; }

        public string Pic5Produ { get; set; }
    }
}