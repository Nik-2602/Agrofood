using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class ChiTietHoaDon
    {
        public string MaHD { get; set; }
        public string MaDDH { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public int TongTien { get; set; }

        public ChiTietHoaDon() { }

        public ChiTietHoaDon(string maHD, string maDDH, string maSP, int soLuong, int giaBan,int tongTien)
        {
            this.MaHD = maHD;
            this.MaDDH = maDDH;
            this.MaSP = maSP;
            this.SoLuong = soLuong;
            this.GiaBan = giaBan;
            this.TongTien = tongTien;
        }
    }
}