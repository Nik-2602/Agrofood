using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class DoanhThuLoaiHang
    {
        public string MaLH { get; set; }
        public string TenLH { get; set; }
        public int SoLuong  { get; set; }
        public int TongTien { get; set; }

        public DoanhThuLoaiHang() { }

        public DoanhThuLoaiHang(string maLH ,string tenLH, int soLuong, int tongTien)
        {
            this.MaLH = maLH;
            this.TenLH = tenLH;
            this.SoLuong = soLuong;
            this.TongTien = tongTien;
        }
    }
}