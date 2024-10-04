using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        THNNEntities1 db = new THNNEntities1();
        // GET: Admin/SanPham
        public ActionResult Index()
        {
            List<SanPham> sanPhams = db.SanPhams.ToList();
            return View(sanPhams);
        }

        public ActionResult ThemMoiSP()
        {
            List<LoaiHang> danhSachLH = db.LoaiHangs.ToList();
            List<NhaCungCap> danhSachNCC = db.NhaCungCaps.ToList();
            Session["DanhSachLH"] = danhSachLH;
            Session["DanhSachNCC"] = danhSachNCC;
/*            ViewBag.DanhSachLH = danhSachLH;*/
/*            ViewBag.DanhSachNCC = danhSachNCC;*/
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiSP(string LoaiHang, SanPham model, string NhaCungCap, HttpPostedFileBase fileAnh, HttpPostedFileBase fileAnh1)
        {
            List<SanPham> danhSachNV = db.SanPhams.ToList();
            SanPham sanPhamLast = danhSachNV.Last();
            string maSP = "";
            string splitMaSP = "";
            long newMaSPInt = 0;
            string newMaSPString = "";
            maSP = sanPhamLast.MaSP;
            splitMaSP = maSP.Substring(2);
            if (Int64.TryParse(splitMaSP, out newMaSPInt))
            {
                newMaSPInt++; // Thực hiện tăng giá trị
            }
            if (newMaSPInt < 10)
            {
                newMaSPString = "SP00" + Convert.ToString(newMaSPInt);
            }
            else if (newMaSPInt > 10)
            {
                newMaSPString = "SP0" + Convert.ToString(newMaSPInt);
            }

            if (model.TenSP == null || model.TenSP.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên sản phẩm");
                return View(model);
            }

            if (model.TrongLuong == null || model.TrongLuong.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng nhập trọng lượng sản phẩm");
                return View(model);
            }

            if (model.GiaBanSP == 0 || model.GiaBanSP < 0)
            {
                ModelState.AddModelError("", "Vui lòng nhập giá bán sản phẩm");
                return View(model);
            }

            if (model.MoTaSP == null || model.MoTaSP.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng thông tin mô tả sản phẩm");
                return View(model);
            }

            if (model.SoLuongT == 0 || model.SoLuongT < 0)
            {
                ModelState.AddModelError("", "Vui lòng nhập số lượng tồn sản phẩm");
                return View(model);
            }

            if (LoaiHang == null || LoaiHang.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng chọn sản phẩm thuộc loại hàng nào ");
                return View(model);

            }


            if (fileAnh == null || fileAnh.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng cho thông tin hình ảnh về sản phẩm");
                return View(model);
            }
            else
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_product/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.HinhAnh = "/assets/img/img_product/" + fileAnh.FileName;
                }
            }

            if (fileAnh1 == null || fileAnh1.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng cho thông tin hình ảnh khác về sản phẩm");
                return View(model);
            }
            else
            {
                if (fileAnh1.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_product/");
                    string pathEmail = rootFolder + fileAnh1.FileName;
                    fileAnh1.SaveAs(pathEmail);
                    model.HinhAnh1 = "/assets/img/img_product/" + fileAnh1.FileName;
                }
            }
            model.MaSP = newMaSPString;
            string maMH = LoaiHang;
            model.MaLH = maMH;
            model.MaCT = null;
            model.ThuongHieu = NhaCungCap;
            db.SanPhams.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult CapNhatSP(string id)
        {
            SanPham model = db.SanPhams.Find(id);
            Session["SanPham_Update_Admin"] = model;
            List<NhaCungCap> danhSachNCC = db.NhaCungCaps.ToList();
            ViewBag.DanhSachNCC = danhSachNCC;
            return View(model);
        }

        [HttpPost]
        public ActionResult CapNhatSP(SanPham model, string NhaCungCap, HttpPostedFileBase fileAnh, HttpPostedFileBase fileAnh1)
        {
            SanPham modelSP = Session["SanPham_Update_Admin"] as SanPham;
            var updateModel = db.SanPhams.Find(modelSP.MaSP);
            updateModel.MaLH = modelSP.MaLH;
            updateModel.MaSP = modelSP.MaSP;
            updateModel.TenSP = model.TenSP;
            updateModel.GiaBanSP = model.GiaBanSP;
            NhaCungCap nhaCungCap = db.NhaCungCaps.FirstOrDefault(x => x.TenNCC == NhaCungCap);
            string maNCC = nhaCungCap.MaNCC;
            updateModel.TrongLuong = model.TrongLuong;
            updateModel.MoTaSP = model.MoTaSP;
            updateModel.SoLuongT = model.SoLuongT;
            if (fileAnh != null)
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_product/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.HinhAnh = "/assets/img/img_product/" + fileAnh.FileName;
                    updateModel.HinhAnh = model.HinhAnh;
                }
            }
            else
            {
                updateModel.HinhAnh = modelSP.HinhAnh;
            }

            if (fileAnh1 != null)
            {
                if (fileAnh1.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/img_product/");
                    string pathEmail = rootFolder + fileAnh1.FileName;
                    fileAnh1.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.HinhAnh1 = "/assets/img/img_product/" + fileAnh1.FileName;
                    updateModel.HinhAnh1 = model.HinhAnh1;
                }
            }
            else
            {
                updateModel.HinhAnh1 = modelSP.HinhAnh1;
            }
            Session["SanPham_Update_Admin"] = model;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult XoaSP(string id)
        {
            SanPham model2 = db.SanPhams.Find(id);
            db.SanPhams.Remove(model2);
            Session.Remove("SanPham_Update_Admin");
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}