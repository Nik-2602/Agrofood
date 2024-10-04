using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Areas.Admin.Controllers
{
    public class NhapHangController : Controller
    {
        // GET: Admin/NhapHang

        THNNEntities1 db = new THNNEntities1();
        public ActionResult Index()
        {
            List<PhieuNhap> dsPN = db.PhieuNhaps.ToList();
            return View(dsPN);
        }


        public ActionResult XemPN(string id)
        {
            var query = db.Database.SqlQuery<CTPhieuNhap>("select * from CTPhieuNhap where MaN ='" + id + "'");
            List<CTPhieuNhap> dsNhapHang = query.ToList();

            return View(dsNhapHang);
        }

    }
}