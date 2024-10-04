using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class CTGG_ListProduct
    {

        public string MaCT { get; set; }
        public string TenSuKien { get; set; }

        public DateTime? ThoiGianBD { get; set; }

        public DateTime? ThoiGianKT { get; set; }
        public float PhanTram { get; set; }

        public List<SP_LH> ListSP_LH { get; set; }

        public CTGG_ListProduct()
        {

        }


        public CTGG_ListProduct(string mact, string tenSuKien, float phanTram, List<SP_LH> listSP_LH,DateTime thoiGianBD, DateTime thoiGianKT)
        {
            this.MaCT = mact;
            this.TenSuKien = tenSuKien;
            this.PhanTram = phanTram;
            this.ListSP_LH = listSP_LH;
            this.ThoiGianBD = thoiGianBD;
            this.ThoiGianKT = thoiGianKT;
        }
    }
}