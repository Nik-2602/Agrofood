using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Controllers
{
    public class CartController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ThemSanPham(SP_LH_DDH_CTDDH SanPham)
        {
            THNNEntities1 db = new THNNEntities1();
            bool itemExists = false;
            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;

            if(listBuyKH == null || listBuyKH.IsNullOrEmpty())
            {
                listBuyKH = new List<SP_LH_DDH_CTDDH>();
            }

            foreach(var item in listBuyKH)
            {
                if(item.MaSP.Equals(SanPham.MaSP))
                {
                    item.SoLuong = SanPham.SoLuong;
                    itemExists = true;
                    break;
                }
            }

            if(itemExists)
            {
                CTDonDatHang cTDonDatHang = db.CTDonDatHangs.FirstOrDefault(m => m.MaDDH == SanPham.MaDDH && m.MaSP == SanPham.MaSP);
                cTDonDatHang.SoLuong = SanPham.SoLuong;
                cTDonDatHang.TongTien = cTDonDatHang.GiaBan * cTDonDatHang.SoLuong;
                db.SaveChanges();
                return Json(new { success = true}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SanPham != null)
                {

                    CTDonDatHang ctDonHang = new CTDonDatHang();
                    ctDonHang.MaDDH = SanPham.MaDDH;
                    ctDonHang.MaSP = SanPham.MaSP;
                    ctDonHang.SoLuong = SanPham.SoLuong;
                    ctDonHang.GiaBan = SanPham.GiaBanSP;
                    ctDonHang.TongTien = SanPham.TongTien;
                    db.CTDonDatHangs.Add(ctDonHang);
                    db.SaveChanges();
                    if (Session["User"] != null)
                    {
                        var user = Session["User"] as THNN.Models.KhachHang;
                        var query1 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(
                        "SELECT *" +
                        "from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join " +
                        " CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP " +
                        "join LoaiHang lh on lh.MaLH = sp.MaLH  " +
                        "Where ddh.TinhTrang = N'Đang mua hàng' and kh.MaKH = '" + user.MaKH + "'");
                        listBuyKH = query1.ToList();
                        Session["getListBuyKH"] = listBuyKH;

                    }
                }
            }
            

            // Xử lý thêm sản phẩm vào cơ sở dữ liệu ở đây
            
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MuaSanPham(SP_LH_DDH_CTDDH SanPham)
        {
            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            // Xử lý thêm sản phẩm vào cơ sở dữ liệu ở đây
            if (SanPham != null)
            {
                THNNEntities1 db = new THNNEntities1();
                CTDonDatHang ctDonHang = new CTDonDatHang();
                ctDonHang.MaDDH = SanPham.MaDDH;
                ctDonHang.MaSP = SanPham.MaSP;
                ctDonHang.SoLuong = SanPham.SoLuong;
                ctDonHang.GiaBan = SanPham.GiaBanSP;
                ctDonHang.TongTien = SanPham.TongTien;
                db.CTDonDatHangs.Add(ctDonHang);
                db.SaveChanges();
                if (Session["User"] != null)
                {
                    var user = Session["User"] as THNN.Models.KhachHang;
                    var query1 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(
                    "SELECT *" +
                    "from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join " +
                    " CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP join MatHang mh on mh.MaMH = sp.MaMH  " +
                    "join LoaiHang lh on lh.MaLH = sp.MaLH  " +
                    "Where ddh.TinhTrang = N'Đang mua hàng' and kh.MaKH = '" + user.MaKH + "'");
                    listBuyKH = query1.ToList();
                    Session["getListBuyKH"] = listBuyKH;

                }

            }
            return RedirectToAction("Index", "Order");
        }



        [HttpPost]
        public ActionResult XoaSanPham(string maDDH = "", string maSP = "")
        {
            THNNEntities1 db = new THNNEntities1();
            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            CTDonDatHang modelDel = db.CTDonDatHangs.FirstOrDefault(m => m.MaDDH == maDDH && m.MaSP == maSP);
            db.CTDonDatHangs.Remove(modelDel);

            for (int i = 0; i < listBuyKH.Count; i++)
            {
                var item = listBuyKH[i];
                if (item.MaDDH == maDDH && item.MaSP == maSP)
                {
                    listBuyKH.Remove(item);
                }
            }
            Session["getListBuyKh"] = listBuyKH;
            db.SaveChanges();

            return View();
        }

       
    }
}