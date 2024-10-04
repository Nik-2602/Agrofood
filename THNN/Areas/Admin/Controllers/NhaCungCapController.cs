using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class NhaCungCapController : Controller
    {
        THNNEntities1 db = new THNNEntities1();

        // GET: Admin/NhaCungCap
        public ActionResult Index()
        {
            List<NhaCungCap> danhSachNCC = db.NhaCungCaps.ToList();
            return View(danhSachNCC);
        }

        public ActionResult ThemMoiNCC()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ThemMoiNCC(NhaCungCap model)
        {
            List<NhaCungCap> danhSachNCC = db.NhaCungCaps.ToList();
            NhaCungCap nccLast = danhSachNCC.Last();
            string maNCC = "";
            string splitMaNCC = "";
            long newMaNCCInt = 0;
            string newMaNCCString = "";
            maNCC = nccLast.MaNCC;
            splitMaNCC = maNCC.Substring(3);
            if (Int64.TryParse(splitMaNCC, out newMaNCCInt))
            {
                newMaNCCInt++;
            }

            if (newMaNCCInt < 10)
            {
                newMaNCCString = "NCC0" + Convert.ToString(newMaNCCInt);
            }
            else if (newMaNCCInt > 10)
            {
                newMaNCCString = "NCC" + Convert.ToString(newMaNCCInt);
            }

            if (model.TenNCC == null || model.TenNCC.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng tên nhà cung cấp");
                return View(model);
            }

            if (model.DiaChi == null || model.DiaChi.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền địa chỉ");
                return View(model);
            }

            if (model.DienThoai == null || model.DienThoai.Equals(""))
            {
                ModelState.AddModelError("", "Vui lòng điền số điện thoại");
                return View(model);
            }
            model.MaNCC = newMaNCCString;
            db.NhaCungCaps.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CapNhatNCC(string id)
        {
            //Tìm đối tượng
            NhaCungCap model = db.NhaCungCaps.Find(id);
            Session["Admin_Update-NCC"] = model;
            return View(model);
        }

        [HttpPost]
        public ActionResult CapNhatNCC(NhaCungCap model)
        {

            NhaCungCap modelNCC = Session["Admin_Update-NCC"] as NhaCungCap;
            //Tìm đối tượng
            var updateModel = db.NhaCungCaps.Find(modelNCC.MaNCC);
            //Gắn giá trị
            updateModel.MaNCC = modelNCC.MaNCC;
            updateModel.TenNCC = model.TenNCC;
            updateModel.DiaChi = model.DiaChi;
            updateModel.DienThoai = model.DienThoai;
            //Lưu thay đổi
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XoaNCC(string id)
        {
            NhaCungCap model2 = db.NhaCungCaps.Find(id);
            db.NhaCungCaps.Remove(model2);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

    }
}