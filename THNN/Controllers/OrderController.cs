using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            List<string> listSearch = Session["SearchList"] as List<string>;
            if (listSearch == null)
            {
                listSearch = new List<string>();
            }

            THNNEntities1 db = new THNNEntities1();

            var queryLH = db.Database.SqlQuery<LoaiHang>(
                "select * from LoaiHang");
            List<LoaiHang> danhSachLH = queryLH.ToList();


            GetControllerHome viewModelAdd = new GetControllerHome(listSearch, null, danhSachLH);
            ViewBag.getControllerHome = viewModelAdd;

            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
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
            return View(listBuyKH);
        }


        [HttpPost]
        public ActionResult CapNhatGioHang(List<CTDonDatHang> listCTDDH)
        {

            THNNEntities1 db = new THNNEntities1();
            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            if (listCTDDH != null)
            {
                foreach (var ctdhData in listCTDDH)
                {

                    // Lấy CTDH từ cơ sở dữ liệu
                    var ctdhUpdateModel = db.CTDonDatHangs.FirstOrDefault(x => x.MaDDH == ctdhData.MaDDH && x.MaSP == ctdhData.MaSP);
                    ctdhUpdateModel.MaDDH = ctdhData.MaDDH;
                    ctdhUpdateModel.MaSP = ctdhData.MaSP;
                    ctdhUpdateModel.SoLuong = ctdhData.SoLuong;
                    ctdhUpdateModel.GiaBan = ctdhData.GiaBan;
                    ctdhUpdateModel.TongTien = ctdhData.TongTien;
                    db.SaveChanges();


                }

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



            return Json(new { success = true, message = "Cập nhật giỏ hàng thành công"}, JsonRequestBehavior.AllowGet);
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

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult ThemDDH(DonDatHang ddh, string LoaiThanhToanTT)
        {
            
            THNNEntities1 db = new THNNEntities1();
            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            if (ddh != null)
            {

               
                DonDatHang model = db.DonDatHangs.FirstOrDefault(m => m.MaDDH == ddh.MaDDH);
                model.MaDDH = ddh.MaDDH;
                model.MaKH = ddh.MaKH;
                model.NgayDH = ddh.NgayDH;
                model.NgayGH = ddh.NgayGH;
                model.TenNguoiNhan = ddh.TenNguoiNhan;
                model.DiaChiNhan = ddh.DiaChiNhan;
                model.SDTNhanHang = ddh.SDTNhanHang;
                model.Email = ddh.Email;
                model.HTThanhToan = ddh.HTThanhToan;
                model.HTGiaoHang = ddh.HTGiaoHang;
                model.TriGia = ddh.TriGia;

                db.SaveChanges();

                if (ddh.HTThanhToan.Equals("Thanh Toán Trực Tuyến") & LoaiThanhToanTT.Equals("vnpay"))
                {
                    dataResponse dataResponse = new dataResponse(ddh.TriGia, model.MaDDH);

                    return Json(new { success = true, data = dataResponse }, JsonRequestBehavior.AllowGet);
                }

                model.TinhTrang = ddh.TinhTrang;
                db.SaveChanges();
            }
            foreach(var item in listBuyKH)
            {
                SanPham sanPham = db.SanPhams.FirstOrDefault(m => m.MaSP == item.MaSP);
                sanPham.SoLuongT = sanPham.SoLuongT - item.SoLuong;
                db.SaveChanges();
            }

            //send email
            SendEmail(ddh, listBuyKH);
            listBuyKH.Clear();
            Session["getListBuyKH"] = listBuyKH;


            return RedirectToAction("Index");
        }

        public void SendEmail(DonDatHang ddh, List<SP_LH_DDH_CTDDH> listBuyKH)
        {
            var strSanPham = "";
            var emailKhachHang = "";
            DateTime? ngayDH = new DateTime();
            foreach (var item in listBuyKH)
            {
                strSanPham += "<tr>";
                strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\">" + item.TenSP + "</td>";
                strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + item.SoLuong + "</td>";
                strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\"><span>" + item.GiaBanSP + "&nbsp;<span>₫</span></span></td>";
                strSanPham += "</tr>";
                emailKhachHang = item.Email;
                ngayDH = item.NgayDH;
            }

            string formattedDate = ngayDH?.ToString("dd-MM-yyyy");

            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/send2.html"));
            contentCustomer = contentCustomer.Replace("{{MaDon}}", ddh.MaDDH);
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", formattedDate);
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ddh.TriGia.ToString());
            contentCustomer = contentCustomer.Replace("{{PhuongThucTT}}", ddh.HTThanhToan);
            contentCustomer = contentCustomer.Replace("{{TongCong}}", ddh.TriGia.ToString());
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", ddh.TenNguoiNhan);
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", ddh.DiaChiNhan);
            contentCustomer = contentCustomer.Replace("{{SoDT}}", ddh.SDTNhanHang);
            contentCustomer = contentCustomer.Replace("{{Email}}", emailKhachHang);

            Common.Common.SendMail("Agrofood", "Đơn hàng #" + ddh.MaDDH, contentCustomer.ToString(), emailKhachHang);


        }



    }
}