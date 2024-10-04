using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class DoanhThuCTGG
    {
        public string MaCT { get; set; }
        public string TenSuKien { get; set; }
        public int SoLuong { get; set; }
        public int TongTien { get; set; }

        public DoanhThuCTGG() { }

        public DoanhThuCTGG(string mact,string tenSK, int soLuong, int tongTien)
        {
            this.MaCT = mact;
            this.TenSuKien = tenSK;
            this.SoLuong = soLuong;
            this.TongTien = tongTien;
        }

    }
}