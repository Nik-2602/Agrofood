using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class BanHangController : Controller
    {
        THNNEntities1 db = new THNNEntities1();
        // GET: Admin/BanHang
        public ActionResult Index()
        {
            var query = db.Database.SqlQuery<DonDatHang>("select * from DonDatHang" +
                " where TinhTrang = N'Hoàn thành' or TinhTrang = N'Đã đặt hàng'");
            List<DonDatHang> dsDonDatHang = query.ToList();
            return View(dsDonDatHang);
        }



        public ActionResult TaoHD(string id)
        {
            DonDatHang ddh = db.DonDatHangs.Find(id);
            ddh.TinhTrang = "Đang giao hàng";
            db.SaveChanges();

            var query = db.Database.SqlQuery<CTDonDatHang>("select * from CTDonDatHang" +
                " where MaDDH = '" + id + "'");
            List<CTDonDatHang> dsCTDonDatHang = query.ToList();
            HoaDon hoaDonLast = db.HoaDons.ToList().Last();
            string maHD = "";
            string splitMaHD = "";
            long newMaHDInt = 0;
            string newMaHDString = "";
            maHD = hoaDonLast.MaHD;
            splitMaHD = maHD.Substring(2);
            if (Int64.TryParse(splitMaHD, out newMaHDInt))
            {
                newMaHDInt++; // Thực hiện tăng giá trị
            }
            if (newMaHDInt < 10)
            {
                newMaHDString = "HD00" + Convert.ToString(newMaHDInt);
            }
            else if (newMaHDInt > 10)
            {
                newMaHDString = "HD0" + Convert.ToString(newMaHDInt);
            }
            string maNV = "";
            var admin = Session["Admin"] as THNN.Models.NhanVien;
            if (admin != null)
            {
                maNV = admin.MaNV;
            }
            string maDDH = ddh.MaDDH;
            DateTime ngayBan = ddh.NgayGH;
            int triGia = ddh.TriGia;
            HoaDon newHD = new HoaDon();
            newHD.MaHD = newMaHDString;
            newHD.MaNV = maNV;
            newHD.MaDDH = maDDH;
            newHD.NgayBan = ngayBan;
            newHD.TriGia = triGia;
            db.HoaDons.Add(newHD);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult XemHD(string id)
        {
            var query = db.Database.SqlQuery<CTDonDatHang>("select * from CTDonDatHang where MaDDH ='" + id + "'");
            List<CTDonDatHang> dsDonDatHang = query.ToList();

            return View(dsDonDatHang);
        }
    }
}