using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang

        public bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        public ActionResult Index()
        {
            THNNEntities1 db = new THNNEntities1();
            List<KhachHang> danhSachKH = db.KhachHangs.ToList();
            return View(danhSachKH);
        }

        public ActionResult ThemMoiKH()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ThemMoiKH(KhachHang model, HttpPostedFileBase fileAnh)
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

            if (model.HoTen == null || model.HoTen.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền họ tên");
                return View(model);
            }

            if (model.NgaySinh == null || model.NgaySinh.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền ngày sinh");
                return View(model);
            }

            if (model.GioiTinh == null || model.GioiTinh.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng chọn giới tính");
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

            if (fileAnh != null)
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_user/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.HinhAnhKH = "/assets/img/img_user/" + fileAnh.FileName;
                }
            }
            else
            {
                model.HinhAnhKH = "/assets/img/img_user/user.jpeg";
            }

            DateTime? date = model.NgaySinh;
            string ngaySinh = date?.ToString("yyyy-MM-dd");
            model.NgaySinh = DateTime.Parse(ngaySinh);

            model.MaKH = newMaKHString;
            db.KhachHangs.Add(model);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult CapNhatKH(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            //Tìm đối tượng
            KhachHang model = db.KhachHangs.Find(id);
            Session["Admin_Update-KH"] = model;
            return View(model);
        }


        [HttpPost]
        public ActionResult CapNhatKH(KhachHang model, HttpPostedFileBase fileAnh)
        {
            THNNEntities1 db = new THNNEntities1();
            KhachHang modelKhachHang = Session["Admin_Update-KH"] as KhachHang;
            //Tìm đối tượng
            var updateModel = db.KhachHangs.Find(modelKhachHang.MaKH);
            //Gắn giá trị
            updateModel.HoTen = model.HoTen;
            updateModel.NgaySinh = model.NgaySinh;
            updateModel.GioiTinh = model.GioiTinh;
            updateModel.DiaChi = model.DiaChi;
            updateModel.Email = model.Email;
            updateModel.SDT = model.SDT;
            if (fileAnh != null)
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_user/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.HinhAnhKH = "/assets/img/img_user/" + fileAnh.FileName;
                    updateModel.HinhAnhKH = model.HinhAnhKH;
                }
            }
            updateModel.TaiKhoan = model.TaiKhoan;
            updateModel.MatKhau = model.MatKhau;
            //Lưu thay đổi
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XoaKH(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            KhachHang model2 = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(model2);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}