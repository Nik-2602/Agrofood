using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class DoanhThuNhanVien
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public int SoLuong { get; set; }
        public int TongTien { get; set; }

        public DoanhThuNhanVien()
        {

        }

        public DoanhThuNhanVien(string maNV, string hoTen,int soLuongTao, int triGia)
        {
            this.MaNV = maNV;
            this.HoTen = hoTen;
            this.SoLuong = soLuongTao;
            this.TongTien = triGia;
        }
    }
}