using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class SP_LH_DDH_CTDDH
    {
        public string MaKH { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string HinhAnhKH { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string MaDDH { get; set; }

        public DateTime NgayDH { get; set; }
        public DateTime NgayGH { get; set; }

        public string TenNguoiNhan { get; set; }
        public string DiaChiNhan { get; set; }
        public string SDTNhanHang { get; set; }
        public string HTThanhToan { get; set; }
        public string HTGiaoHang { get; set; }
        public int TriGia { get; set; }
        public string TinhTrang { get; set; }

        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public int TongTien { get; set; }
        public string TenSP { get; set; }
        public int GiaBanSP { get; set; }
        public string TrongLuong { get; set; }
        public string MotaSP { get; set; }
        public int SoLuongT { get; set; }
        public string HinhAnh { get; set; }
        public string HinhAnh1 { get; set; }
        public string MaLH { get; set; }
        public string TenLH { get; set; }


        public SP_LH_DDH_CTDDH() { }

        public SP_LH_DDH_CTDDH(string maKH, string hoTen, DateTime? ngaySinh, string gioiTinh, string diaChi, string email, string soDT, string hinhAnhKH, string taiKhoan, string matKhau, string maDDH, DateTime ngayDH, DateTime ngayGH, string tenNguoiNhan, string diaChiNhan, string sDTNhanHang, string hTThanhToan, string hTGiaoHang, int triGia, string tinhTrang, string maSP, int soLuong, int giaBan, int tongTien, string tenSP, int giaBanSP, string trongLuong, string motaSP, int soLuongT, string hinhAnh, string hinhAnh1, string maLH, string tenLH)
        {
            MaKH = maKH;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            Email = email;
            SDT = soDT;
            HinhAnhKH = hinhAnhKH;
            TaiKhoan = taiKhoan;
            MatKhau = matKhau;
            MaDDH = maDDH;
            NgayDH = ngayDH;
            NgayGH = ngayGH;
            TenNguoiNhan = tenNguoiNhan;
            DiaChiNhan = diaChiNhan;
            SDTNhanHang = sDTNhanHang;
            HTThanhToan = hTThanhToan;
            HTGiaoHang = hTGiaoHang;
            TriGia = triGia;
            TinhTrang = tinhTrang;
            MaSP = maSP;
            SoLuong = soLuong;
            GiaBan = giaBan;
            TongTien = tongTien;
            TenSP = tenSP;
            GiaBanSP = giaBanSP;
            TrongLuong = trongLuong;
            MotaSP = motaSP;
            SoLuongT = soLuongT;
            HinhAnh = hinhAnh;
            HinhAnh1 = hinhAnh1;
            MaLH = maLH;
            TenLH = tenLH;
        }

    }
}