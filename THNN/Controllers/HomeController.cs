using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Controllers
{
    public class HomeController : Controller
    {
        public List<SP_LH_DDH_CTDDH> LayGioHang()
        {
            List<SP_LH_DDH_CTDDH> lstGioHang = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<SP_LH_DDH_CTDDH>();
                Session["getListBuyKH"] = lstGioHang;
            }
            return lstGioHang;
        }

        public List<CTGG_ListProduct> getListSales()
        {
            using (var db = new THNNEntities1())
            {
                var query = db.Database.SqlQuery<SP_LH_CTGG>(
                    "SELECT lh.TenLH, sp.TenSP, sp.GiaBanSP, sp.HinhAnh, ctgg.PhanTram, ctgg.TenSuKien, " +
                    "sp.MaSP, sp.MaLH, sp.SoLuongT, sp.TrongLuong, sp.MoTaSP, sp.HinhAnh1, ctgg.MaCT, ctgg.ThoiGianBD, ctgg.ThoiGianKT " +
                    "FROM SanPham sp " +
                    "JOIN LoaiHang lh ON lh.MaLH = sp.MaLH " +
                    "LEFT JOIN ChuongTrinhGG ctgg ON ctgg.MaCT = sp.MaCT"
                );

                var querySPLHCTGG = query.ToList();

                // Lọc những phần tử mà MaCT khác null
                var filteredQuery = querySPLHCTGG.Where(item => item.MaCT != null).ToList();

                // Nhóm sản phẩm theo MaCT
                var groupedProducts = filteredQuery.GroupBy(item => new { item.MaCT, item.TenSuKien, item.PhanTram,item.ThoiGianBD, item.ThoiGianKT })
                                                   .Select(g => new CTGG_ListProduct
                                                   {
                                                       MaCT = g.Key.MaCT,
                                                       TenSuKien = g.Key.TenSuKien,
                                                       PhanTram = (float)g.Key.PhanTram * 100,
                                                       ThoiGianBD = g.Key.ThoiGianBD,
                                                       ThoiGianKT = g.Key.ThoiGianKT,
                                                       ListSP_LH = g.Select(item => new SP_LH
                                                       {
                                                           MaSP = item.MaSP,
                                                           MaLH = item.MaLH,
                                                           SoLuongT = item.SoLuongT,
                                                           TenLH = item.TenLH,
                                                           TenSP = item.TenSP,
                                                           GiaBanSP = item.GiaBanSP,
                                                           TrongLuong = item.TrongLuong,
                                                           MoTaSP = item.MoTaSP,
                                                           HinhAnh1 = item.HinhAnh1,
                                                           HinhAnh = item.HinhAnh
                                                       }).ToList()
                                                   }).ToList();

                return groupedProducts;
            }
        }

        public void HandleOrderUser()
        {
            THNNEntities1 db = new THNNEntities1();
            var user = Session["User"] as THNN.Models.KhachHang;
            var query1 = db.Database.SqlQuery<DonDatHang>(
            "select *" +
            "from DonDatHang " +
            "Where TinhTrang = N'Đang mua hàng' and MaKH = '" + user.MaKH + "'");
            List<DonDatHang> listDDH = query1.ToList();
            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            if (listDDH.Count == 0)
            {
                List<DonDatHang> dsDDH = db.DonDatHangs.ToList();
                DonDatHang donDatHangLast = dsDDH.Last();
                string maDDH = "";
                string splitMaDDH = "";
                long newMaDDHInt = 0;
                string newMaDDHString = "";
                maDDH = donDatHangLast.MaDDH;
                splitMaDDH = maDDH.Substring(3);
                if (Int64.TryParse(splitMaDDH, out newMaDDHInt))
                {
                    newMaDDHInt++; // Thực hiện tăng giá trị
                }
                if (newMaDDHInt < 10)
                {
                    newMaDDHString = "DDH0" + Convert.ToString(newMaDDHInt);
                }
                else if (newMaDDHInt > 10)
                {
                    newMaDDHString = "DDH" + Convert.ToString(newMaDDHInt);
                }
                string maKH = user.MaKH;
                DateTime ngayDH = DateTime.Now;
                DateTime dateNgayDH = DateTime.Parse(ngayDH.ToString("yyyy-MM-dd"));
                DateTime ngayGH = ngayDH.AddDays(3);
                DateTime dateNgayGH = DateTime.Parse(ngayGH.ToString("yyyy-MM-dd"));
                string tenNN = user.HoTen;
                string DiaChiNhan = user.DiaChi;
                string SDTN = user.SDT;
                string email = user.Email;
                string HTThanhToan = "Thanh toán bằng tiền mặt";
                string HTGiaoHang = "Giao hàng tận nơi";
                int triGia = 0;
                string tinhTrang = "Đang mua hàng";
                DonDatHang newDDH = new DonDatHang();
                newDDH.MaDDH = newMaDDHString;
                newDDH.MaKH = maKH;
                newDDH.NgayDH = dateNgayDH;
                newDDH.NgayGH = dateNgayGH;
                newDDH.TenNguoiNhan = tenNN;
                newDDH.DiaChiNhan = DiaChiNhan;
                newDDH.SDTNhanHang = SDTN;
                newDDH.Email = email;
                newDDH.HTThanhToan = HTThanhToan;
                newDDH.HTGiaoHang = HTGiaoHang;
                newDDH.TriGia = triGia;
                newDDH.TinhTrang = tinhTrang;

                db.DonDatHangs.Add(newDDH);
                db.SaveChanges();

                var query2 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(
               "SELECT *" +
               "from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join " +
               " CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP " +
               "join LoaiHang lh on lh.MaLH = sp.MaLH  " +
               "Where ddh.TinhTrang = N'Đang mua hàng' and kh.MaKH = '" + user.MaKH + "'");
                listBuyKH = query2.ToList();
                Session["getListBuyDDH"] = listDDH;
                Session["getListBuyKH"] = listBuyKH;
            }
            else
            {
                var query2 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(
                               "SELECT *" +
                               "from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join " +
                               " CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP " +
                               "join LoaiHang lh on lh.MaLH = sp.MaLH  " +
                               "Where ddh.TinhTrang = N'Đang mua hàng' and kh.MaKH = '" + user.MaKH + "'");
                listBuyKH = query2.ToList();
                Session["getListBuyKH"] = listBuyKH;
                Session["getListBuyDDH"] = listDDH;
            }

        }
        public ActionResult Index()
        {
            List<string> listSearch = Session["SearchList"] as List<string>;
            if (listSearch == null)
            {
                listSearch = new List<string>();
            }


            List<CTGG_ListProduct> listSalesProgram = getListSales();
            Session["ListProgram"] = listSalesProgram;



            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<SP_LH_CTGG>(
                "SELECT lh.TenLH, sp.TenSP, sp.GiaBanSP, sp.HinhAnh,ctgg.PhanTram, ctgg.TenSuKien " +
                "FROM SanPham sp  JOIN LoaiHang lh ON lh.MaLH = sp.MaLH " +
                "left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT");

            var queryLH = db.Database.SqlQuery<LoaiHang>(
                "select * from LoaiHang");

            List<THNN.Models.ChuongTrinhGG> listCTGG = db.ChuongTrinhGGs.ToList();
            Session["ListCTGG"] = listCTGG;

            List<SP_LH_CTGG> danhSachSP = query.ToList();
            List<LoaiHang> danhSachLH = queryLH.ToList();


            GetControllerHome viewModelAdd = new GetControllerHome(listSearch, danhSachSP, danhSachLH);
            ViewBag.getControllerHome = viewModelAdd;

            if (Session["User"] != null)
            {
                HandleOrderUser();
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


    }
}