using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class SP_LH
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
        public SP_LH() { }

        public SP_LH( string maSP, string tenSP, int giaBan, string trongLuong, string moTaSP, int soLuongT, string hinhAnh, string hinhAnh1, string maLH, string tenLH)
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
        }
    }
}