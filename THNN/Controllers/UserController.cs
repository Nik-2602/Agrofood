using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        public bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        public void createNewUser(KhachHang model)
        {

            THNNEntities1 db = new THNNEntities1();
            List<KhachHang> danhSachKH = db.KhachHangs.ToList();
            KhachHang khachHangLast = danhSachKH.Last();
            String maKh = "";
            String splitMaKH = "";
            long newMaKHInt = 0;
            String newMaKHString = "";
            maKh = khachHangLast.MaKH;
            splitMaKH = maKh.Substring(2);
            if (Int64.TryParse(splitMaKH, out newMaKHInt))
            {
                newMaKHInt++; // Thực hiện tăng giá trị
            }
            if (newMaKHInt < 10)
            {
                newMaKHString = "KH00" + Convert.ToString(newMaKHInt);
            }
            else if (newMaKHInt > 10)
            {
                newMaKHString = "KH0" + Convert.ToString(newMaKHInt);
            }

            if (model.HinhAnhKH == null)
            {
                model.HinhAnhKH = "/assets/img/img_user/user.jpeg";
            }


            model.MaKH = newMaKHString;
            db.KhachHangs.Add(model);
            db.SaveChanges();
        }


        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(KhachHang model)
        {
            THNNEntities1 db = new THNNEntities1();
            var taiKhoanUser = model.TaiKhoan;
            var matKhauUser = model.MatKhau;
            var adminCheck = db.NhanViens.SingleOrDefault(x => x.TaiKhoan.Equals(taiKhoanUser) && x.MatKhau.Equals(matKhauUser));
            if (adminCheck != null)
            {
                Session["Admin"] = adminCheck;

                return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
            }
            else
            {
                var userCheck = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan.Equals(taiKhoanUser) && x.MatKhau.Equals(matKhauUser));
                if (userCheck != null)
                {

                    Session["User"] = userCheck;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tài Khoản hoặc mật khẩu không chính xác");
                    return View();
                }
            }
        }

        public ActionResult RegisterUser() {
            return View();
        }

 
        [HttpPost]
        public ActionResult RegisterUser(KhachHang model)
        {

            if (model.HoTen == null || model.HoTen.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền họ tên");
                return View(model);
            }

            if (model.DiaChi == null || model.DiaChi.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền địa chỉ");
                return View(model);
            }

            if (model.SDT == null || model.SDT.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền số điện thoại");
                return View(model);

            }

            if (model.Email == null || model.Email.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng nhập email");
                return View(model);
            }
            if (!IsValidEmail(model.Email))
            {
                ModelState.AddModelError("", "Vui lòng nhập email hợp lệ");
                return View(model);
            }

            if (model.TaiKhoan == null || model.TaiKhoan.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền tài khoản");
                return View(model);
            }

            if (model.MatKhau == null || model.MatKhau.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền mật khẩu");
                return View(model);
            }

            createNewUser(model);
            ViewBag.successMS = 1;

            return RedirectToAction("RegisterUser", new { success = 1 });
        }

        public ActionResult LogoutUser()
        {
            var user = Session["User"] as THNN.Models.KhachHang;
            if (user != null)
            {
                Session.Remove("User");
            }
            return RedirectToAction("Index","Home");
        }

        public ActionResult InforUser()
        {
            var user = Session["User"] as THNN.Models.KhachHang;

            return View(user);
        }

        [HttpPost]
        public ActionResult InforUser(KhachHang model, HttpPostedFileBase HinhAnhKH)
        {
            var user = Session["User"] as THNN.Models.KhachHang;
            THNNEntities1 db = new THNNEntities1();
            var updateModel = db.KhachHangs.Find(user.MaKH);

            updateModel.Email = model.Email;
            updateModel.SDT = model.SDT;
            updateModel.GioiTinh = model.GioiTinh;
            updateModel.NgaySinh = model.NgaySinh;
            updateModel.DiaChi = model.DiaChi;

            if (HinhAnhKH != null)
            {
                if (HinhAnhKH.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_user/");
                    string pathEmail = rootFolder + HinhAnhKH.FileName;
                    HinhAnhKH.SaveAs(pathEmail);
                    model.HinhAnhKH = "/assets/img/img_user/" + HinhAnhKH.FileName;
                }
            }
            updateModel.HinhAnhKH = model.HinhAnhKH;
            db.SaveChanges();
            Session["User"] = updateModel;
            return RedirectToAction("InforUser", new { success = 1 });

        }


        public ActionResult PurchaseOrder(string id)
        {

            List<string> listSearch = Session["SearchList"] as List<string>;
            if (listSearch == null)
            {
                listSearch = new List<string>();
            }


            THNNEntities1 db = new THNNEntities1();


            var queryLH = db.Database.SqlQuery<LoaiHang>(
                "select * from LoaiHang");

            List<THNN.Models.ChuongTrinhGG> listCTGG = db.ChuongTrinhGGs.ToList();
            Session["ListCTGG"] = listCTGG;

            List<LoaiHang> danhSachLH = queryLH.ToList();


            GetControllerHome viewModelAdd = new GetControllerHome(listSearch, null, danhSachLH);
            ViewBag.getControllerHome = viewModelAdd;

            var query = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(" SELECT *" +
                " from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join" +
                "  CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP " +
                " join LoaiHang lh on lh.MaLH = sp.MaLH  " +
                "Where ddh.TinhTrang = N'Đã đặt hàng' and kh.MaKH = '" + id + "'");
            List<SP_LH_DDH_CTDDH> dsDangMuaHang = query.ToList();
            ViewBag.DsMuaHang = dsDangMuaHang;

            var query1 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>("select * " +
                "from DonDatHang " +
                "where TinhTrang = N'Đã đặt hàng' and MaKH = '" + id + "'");
            List<SP_LH_DDH_CTDDH> dsDDHDangMuaHang = query1.ToList();
            ViewBag.DsDDHMuaHang = dsDDHDangMuaHang;




            var query2 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(" SELECT *" +
                " from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join" +
                "  CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP " +
                " join LoaiHang lh on lh.MaLH = sp.MaLH  " +
                "Where ddh.TinhTrang = N'Đang giao hàng' and kh.MaKH = '" + id + "'");
            List<SP_LH_DDH_CTDDH> dsGiaoHang = query2.ToList();
            ViewBag.DsGiaoHang = dsGiaoHang;

            var query3 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>("select * " +
                "from DonDatHang " +
                "where TinhTrang = N'Đang giao hàng' and MaKH = '" + id + "'");
            List<SP_LH_DDH_CTDDH> dsDDHGiaoHang = query3.ToList();
            ViewBag.DsDDHGiaoHang = dsDDHGiaoHang;

            var query4 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>(" SELECT *" +
                " from KhachHang kh join DonDatHang ddh on kh.MaKH = ddh.MaKH join" +
                "  CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH join SanPham sp on sp.MaSP = ctddh.MaSP " +
                " join LoaiHang lh on lh.MaLH = sp.MaLH  " +
                "Where ddh.TinhTrang = N'Hoàn thành' and kh.MaKH = '" + id + "'");
            List<SP_LH_DDH_CTDDH> dsHoanThanh = query4.ToList();
            ViewBag.DsHoanThanh = dsHoanThanh;


            var query5 = db.Database.SqlQuery<SP_LH_DDH_CTDDH>("select * " +
                "from DonDatHang " +
                "where TinhTrang = N'Hoàn thành' and MaKH = '" + id + "'");
            List<SP_LH_DDH_CTDDH> dsDDHHoanThanh = query5.ToList();



            ViewBag.DsDDHHoanThanh = dsDDHHoanThanh;

            return View();
        }


        public ActionResult NhanHang(string id)
        {
            var user = Session["User"] as THNN.Models.KhachHang;
            THNNEntities1 db = new THNNEntities1();
            var updateModel = db.DonDatHangs.Find(id);
            updateModel.TinhTrang = "Hoàn thành";
            db.SaveChanges();
            return RedirectToAction("PurchaseOrder", "User", new { id = user.MaKH });
        }







    }
}