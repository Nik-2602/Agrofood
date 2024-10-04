using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class SP_LH_CTGG_Detail
    {
        public string MaLH { get; set; }

        public string TenLH { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiaBanSP { get; set; }
        public string TrongLuong { get; set; }
        public string MoTaSP { get; set; }
        public int SoLuongT { get; set; }
        public string HinhAnh { get; set; }
        public string HinhAnh1 { get; set; }
        public string SanXuat { get; set; }
        public string ThuongHieu { get; set; }
        public string MaCT { get; set; }
        public string TenSuKien { get; set; }
        public string ThongTinSuKien { get; set; }
        public DateTime? ThoiGianBD { get; set; }
        public DateTime? ThoiGianKT { get; set; }
        public string Poster { get; set; }

        public double? PhanTram { get; set; }

        public SP_LH_CTGG_Detail() { }

        public SP_LH_CTGG_Detail(string maLH,string tenLH ,string maSP, string tenSP, int giaBan, string trongLuong, string moTaSP, int soLuongT, string hinhAnh, string hinhAnh1, string sanXuat, string thuongHieu, string maCT, string tenSuKien, string thongTinSuKien, DateTime thoiGianBD, DateTime thoiGianKT, string poster, double phanTram)
        {
            MaLH = maLH;
            TenLH = tenLH;
            MaSP = maSP;
            TenSP = tenSP;
            GiaBanSP = giaBan;
            TrongLuong = trongLuong;
            MoTaSP = moTaSP;
            SoLuongT = soLuongT;
            HinhAnh = hinhAnh;
            HinhAnh1 = hinhAnh1;
            SanXuat = sanXuat;
            ThuongHieu = thuongHieu;
            MaCT = maCT;
            TenSuKien = tenSuKien;
            ThongTinSuKien = thongTinSuKien;
            ThoiGianBD = thoiGianBD;
            ThoiGianKT = thoiGianKT;
            Poster = poster;
            PhanTram = phanTram;

        }
    }
}