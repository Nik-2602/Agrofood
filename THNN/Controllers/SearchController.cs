using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
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

           

            List<SP_LH_CTGG> listProductSearch = Session["getListSPSearch"] as List<THNN.Models.SP_LH_CTGG>;
            GetControllerHome viewModelAdd = new GetControllerHome(listSearch, listProductSearch, danhSachLH);
            ViewBag.getControllerHome = viewModelAdd;

            return View();
        }


        [HttpPost]
        public ActionResult SearchOnSearchBox(String search)
        {
            List<string> listSearch = Session["SearchList"] as List<string>;
            if (listSearch == null)
            {
                listSearch = new List<string>();
            }

            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<SP_LH_CTGG>(
                "SELECT lh.TenLH, sp.TenSP, sp.GiaBanSP, sp.HinhAnh,ctgg.PhanTram, ctgg.TenSuKien " +
                "FROM SanPham sp  JOIN LoaiHang lh ON lh.MaLH = sp.MaLH " +
                "left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "Where sp.TenSP LIKE N'%" + search + "%'");

            List<SP_LH_CTGG> danhSachSP = query.ToList();

            var queryLH = db.Database.SqlQuery<LoaiHang>("select * from LoaiHang");
            List<LoaiHang> danhSachLH = queryLH.ToList();




            GetControllerHome viewModelAdd = new GetControllerHome(listSearch, danhSachSP, danhSachLH);
            listSearch.Add(search);
            Session["SearchList"] = listSearch;
            ViewBag.getControllerHome = viewModelAdd;

            return View();
        }

        [HttpPost]
        public ActionResult SearchOnCategory(string search)
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

            var query1 = db.Database.SqlQuery<SP_LH_CTGG>(
                "SELECT lh.TenLH, sp.TenSP, sp.GiaBanSP, sp.HinhAnh,ctgg.PhanTram, ctgg.TenSuKien" +
                " FROM SanPham sp JOIN LoaiHang lh ON lh.MaLH = sp.MaLH left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "Where lh.TenLH = N'" + search + "'");

            List<SP_LH_CTGG> danhSachSPCategory = query1.ToList();
            Session["getListSPSearch"] = danhSachSPCategory;
            // Thêm kết quả tìm kiếm mới vào danh sách
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchOnSales(string search)
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
            var query1 = db.Database.SqlQuery<SP_LH_CTGG>(
                "SELECT lh.TenLH, sp.TenSP, sp.GiaBanSP, sp.HinhAnh,ctgg.PhanTram, ctgg.TenSuKien" +
                " FROM SanPham sp JOIN LoaiHang lh ON lh.MaLH = sp.MaLH left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "Where ctgg.MaCT = N'" + search + "'");

            List<SP_LH_CTGG> danhSachSPCategory = query1.ToList();
            Session["getListSPSearch"] = danhSachSPCategory;
            // Thêm kết quả tìm kiếm mới vào danh sách
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchOnSlider(string search)
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
            var query1 = db.Database.SqlQuery<SP_LH_CTGG>(
                "SELECT lh.TenLH, sp.TenSP, sp.GiaBanSP, sp.HinhAnh,ctgg.PhanTram, ctgg.TenSuKien" +
                " FROM SanPham sp JOIN LoaiHang lh ON lh.MaLH = sp.MaLH left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "Where ctgg.Poster = N'" + search + "'");

            List<SP_LH_CTGG> danhSachSPCategory = query1.ToList();
            Session["getListSPSearch"] = danhSachSPCategory;
            // Thêm kết quả tìm kiếm mới vào danh sách
            return View("Index");
        }
    }
}