using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: Admin/NhanVien

        public bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        public ActionResult Index()
        {
            THNNEntities1 db = new THNNEntities1();
            List<NhanVien> danhSachNV = db.NhanViens.ToList();
            return View(danhSachNV);
        }

        public ActionResult ThemMoiNV()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ThemMoiNV(NhanVien model, HttpPostedFileBase fileAnh, string LoaiNV)
        {
            THNNEntities1 db = new THNNEntities1();
            List<NhanVien> danhSachNV = db.NhanViens.ToList();
            NhanVien nhanVienLast = danhSachNV.Last();
            string maNV = "";
            string splitMaNV = "";
            long newMaNVInt = 0;
            string newMaNVString = "";
            maNV = nhanVienLast.MaNV;
            splitMaNV = maNV.Substring(2);
            if (Int64.TryParse(splitMaNV, out newMaNVInt))
            {
                newMaNVInt++; // Thực hiện tăng giá trị
            }
            if (newMaNVInt < 10)
            {
                newMaNVString = "NV00" + Convert.ToString(newMaNVInt);
            }
            else if (newMaNVInt > 10)
            {
                newMaNVString = "NV0" + Convert.ToString(newMaNVInt);
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

            if (model.SoDT == null || model.SoDT.Equals(""))
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
                    model.HinhAnh = "/assets/img/img_user/" + fileAnh.FileName;
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng chọn hình ảnh đại diện");
                return View(model);
            }

            DateTime? date = model.NgaySinh;
            string ngaySinh = date?.ToString("yyyy-MM-dd");
            model.NgaySinh = DateTime.Parse(ngaySinh);

            string getLoaiNV = Request.Form["LoaiNV"];
            string loaiNV = "";
            if (getLoaiNV.Equals("Admin"))
            {
                loaiNV = "Admin";
            }
            else
            {
                loaiNV = "Nhân viên kho";
            }

            model.LoaiNV = loaiNV;

            model.MaNV = newMaNVString;
            db.NhanViens.Add(model);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult CapNhatNV(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            //Tìm đối tượng
            NhanVien model = db.NhanViens.Find(id);
            Session["Admin_Update-NV"] = model;
            return View(model);
        }


        [HttpPost]
        public ActionResult CapNhatNV(NhanVien model, HttpPostedFileBase fileAnh, string LoaiNV)
        {
            THNNEntities1 db = new THNNEntities1();
            NhanVien modelNV = Session["Admin_Update-NV"] as NhanVien;
            //Tìm đối tượng
            var updateModel = db.NhanViens.Find(modelNV.MaNV);
            //Gắn giá trị
            updateModel.HoTen = model.HoTen;
            updateModel.NgaySinh = model.NgaySinh;
            updateModel.GioiTinh = model.GioiTinh;
            updateModel.DiaChi = model.DiaChi;
            updateModel.Email = model.Email;
            updateModel.SoDT = model.SoDT;
            if (fileAnh != null)
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_user/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.HinhAnh = "/assets/img/img_user/" + fileAnh.FileName;
                    updateModel.HinhAnh = model.HinhAnh;
                }
            }
            updateModel.TaiKhoan = model.TaiKhoan;
            updateModel.MatKhau = model.MatKhau;
            if (LoaiNV == null && LoaiNV == "")
            {
                updateModel.LoaiNV = modelNV.LoaiNV;
            }
            else if (LoaiNV.Equals("Admin"))
            {
                updateModel.LoaiNV = "Admin";
            }
            else
            {
                updateModel.LoaiNV = "Nhân viên kho";
            }
            //Lưu thay đổi
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult XoaNV(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            NhanVien model2 = db.NhanViens.Find(id);
            db.NhanViens.Remove(model2);
            Session.Remove("Admin_Update-NV");
            db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}