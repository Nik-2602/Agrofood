using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class SP_LH_CTGG
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiaBanSP { get; set; }
        public string TrongLuong { get; set; }
        public string MoTaSP { get; set; }
        public int SoLuongT { get; set; }
        public string HinhAnh { get; set; }
        public string HinhAnh1 { get; set; }
        public string MaLH { get; set; }
        public string TenLH { get; set; }

        public string MaCT { get; set; }
        public double? PhanTram { get; set; }
        public string TenSuKien { get; set; }

        public DateTime? ThoiGianBD { get; set; }
        public DateTime? ThoiGianKT { get; set; }
        public SP_LH_CTGG() { }

        public SP_LH_CTGG(string maSP, string tenSP, int giaBan, string trongLuong, string moTaSP, int soLuongT, string hinhAnh, string hinhAnh1, string maLH, string tenLH, double phanTram,string tenSuKien,string mact, DateTime thoiGianBD, DateTime thoiGianKT)
        {
            MaSP = maSP;
            TenSP = tenSP;
            GiaBanSP = giaBan;
            TrongLuong = trongLuong;
            MoTaSP = moTaSP;
            SoLuongT = soLuongT;
            HinhAnh = hinhAnh;
            HinhAnh1 = hinhAnh1;
            MaLH = maLH;
            TenLH = tenLH;
            PhanTram = phanTram;
            TenSuKien = tenSuKien;
            MaCT = mact;
            ThoiGianBD = thoiGianBD;
            ThoiGianKT = thoiGianKT;
        }

    }
}