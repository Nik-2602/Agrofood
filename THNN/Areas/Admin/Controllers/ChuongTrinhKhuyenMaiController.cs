using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class ChuongTrinhKhuyenMaiController : Controller
    {
        // GET: Admin/ChuongTrinhKhuyenMai
        public ActionResult Index()
        {
            THNNEntities1 db = new THNNEntities1();
            List<ChuongTrinhGG> danhSachGG = db.ChuongTrinhGGs.ToList();
            return View(danhSachGG);
        }

        public ActionResult ThemMoiSuKien()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiSuKien(ChuongTrinhGG model, HttpPostedFileBase fileAnh)
        {
            THNNEntities1 db = new THNNEntities1();
            List<ChuongTrinhGG> danhSachCTGG = db.ChuongTrinhGGs.ToList();
            ChuongTrinhGG ctggLast = danhSachCTGG.Last();
            String maCT = "";
            String splitMaCT = "";
            long newMaCTInt = 0;
            String newMaCTString = "";
            maCT = ctggLast.MaCT;
            splitMaCT = maCT.Substring(2);
            if (Int64.TryParse(splitMaCT, out newMaCTInt))
            {
                newMaCTInt++; // Thực hiện tăng giá trị
            }
            if (newMaCTInt < 10)
            {
                newMaCTString = "CT00" + Convert.ToString(newMaCTInt);
            }
            else if (newMaCTInt > 10)
            {
                newMaCTString = "CT0" + Convert.ToString(newMaCTInt);
            }

            if(model.PhanTram == 0 || model.PhanTram < 0)
            {
                ModelState.AddModelError("", "Vui lòng nhập phần trăm giảm giá");
                return View(model);
            }

            if (fileAnh != null)
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/sales-slider/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.Poster = "/assets/img/sales-slider/" + fileAnh.FileName;
                }
            }
            else
            {
                model.Poster = null;
            }

            DateTime? date = model.ThoiGianBD;
            string thoiGianBD = date?.ToString("yyyy-MM-dd");
            model.ThoiGianBD = DateTime.Parse(thoiGianBD);

            DateTime? date2 = model.ThoiGianKT;
            string thoiGianKT = date?.ToString("yyyy-MM-dd");
            model.ThoiGianKT = DateTime.Parse(thoiGianKT);


            model.PhanTram = model.PhanTram / 100;
            model.MaCT = newMaCTString;
            db.ChuongTrinhGGs.Add(model);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult CapNhatCTGG(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            //Tìm đối tượng
            ChuongTrinhGG model = db.ChuongTrinhGGs.Find(id);
            Session["Admin_Update-CTGG"] = model;
            return View(model);
        }

        public ActionResult ThemSanPham(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            //Tìm đối tượng
            var query = db.Database.SqlQuery<SanPham>(
               "select sp.* " +
               "from ChuongTrinhGG ctgg right join SanPham sp on ctgg.MaCT = sp.MaCT " +
               "where sp.MaCT = '"+id+"' " +
               "union " +
               "select * from SanPham " +
               "where MaCT IS NULL");
            List<SanPham> list = query.ToList();
            Session["MACT_Update-CTGG"] = id;
            return View(list);
        }

        [HttpPost]
        public ActionResult ThemSanPham(bool itemChecked, string maSP, string maCT)
        {
            THNNEntities1 db = new THNNEntities1();
            //Tìm đối tượng
            SanPham sp = db.SanPhams.Find(maSP);
            if (itemChecked)
            {
                
                sp.MaCT = maCT;
            }
            else
            {
                sp.MaCT = null;
            }
            
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult ThemSanPhamAll(bool ItemChecked, string[] ListSP, string MaCT)
        {
            THNNEntities1 db = new THNNEntities1();
            //Tìm đối tượng
            if(ListSP != null)
            {
                for(var i = 0; i< ListSP.Length; i++)
                {
                    SanPham sp = db.SanPhams.Find(ListSP[i]);
                    if (ItemChecked)
                    {

                        sp.MaCT = MaCT;
                    }
                    else
                    {
                        sp.MaCT = null;
                    }
                }
            }
            

            db.SaveChanges();
            return View();
        }






        [HttpPost]
        public ActionResult CapNhatCTGG(ChuongTrinhGG model, HttpPostedFileBase fileAnh)
        {
            THNNEntities1 db = new THNNEntities1();
            ChuongTrinhGG modelCTGG = Session["Admin_Update-CTGG"] as ChuongTrinhGG;
            //Tìm đối tượng
            var updateModel = db.ChuongTrinhGGs.Find(modelCTGG.MaCT);
            //Gắn giá trị
            updateModel.TenSuKien = model.TenSuKien;
            updateModel.ThoiGianBD = model.ThoiGianBD;
            updateModel.ThoiGianKT = model.ThoiGianKT;
            updateModel.ThongTinSuKien = model.ThongTinSuKien;
            updateModel.PhanTram = model.PhanTram;
            if (fileAnh != null)
            {
                if (fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/assets/img/sales-slider/");
                    string pathEmail = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathEmail);
                    //Luu thuoc tinh url hinhanh
                    model.Poster = "/assets/img/sales-slider/" + fileAnh.FileName;
                    updateModel.Poster = model.Poster;
                }
            }
            //Lưu thay đổi
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XoaCTGG(string id)
        {
            THNNEntities1 db = new THNNEntities1();
            ChuongTrinhGG model2 = db.ChuongTrinhGGs.Find(id);
            db.ChuongTrinhGGs.Remove(model2);
            Session.Remove("Admin_Update-CTGG");
            db.SaveChanges();

            return RedirectToAction("Index");

        }

    }
}