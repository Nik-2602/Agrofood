using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class DoanhThuSanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string TenLH { get; set; }
        public int SoLuong { get; set; }
        public int TongTien { get; set; }

        public DoanhThuSanPham() { }

        public DoanhThuSanPham( string maSP,string tenSP,string tenLH, int soLuong, int tongTien)
        {
            this.MaSP = maSP;
            this.TenSP = tenSP;
            this.TenLH = tenLH;
            this.SoLuong = soLuong;
            this.TongTien = tongTien;
        }
    }
}