using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;

namespace THNN.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailProduct(string TenSP = "")
        {
            List<string> listSearch = Session["SearchList"] as List<string>;
            if (listSearch == null)
            {
                listSearch = new List<string>();
            }

            THNNEntities1 db = new THNNEntities1();

            var queryLH = db.Database.SqlQuery<LoaiHang>("select * from LoaiHang");


            List<LoaiHang> danhSachLH = queryLH.ToList();
            GetControllerHome viewModelAdd = new GetControllerHome(listSearch, null, danhSachLH);
            ViewBag.getControllerHome = viewModelAdd;


            var query = db.Database.SqlQuery<SP_LH_CTGG_Detail>(
                "SELECT * " +
                "FROM SanPham sp " +
                "JOIN LoaiHang lh ON lh.MaLH = sp.MaLH " +
                " left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "WHERE sp.TenSP = N'" + TenSP + "'");
            SP_LH_CTGG_Detail sp = query.FirstOrDefault();

            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var query1 = db.Database.SqlQuery<SP_LH_CTGG>(
                "SELECT * " +
                "FROM SanPham sp " +
                "JOIN LoaiHang lh ON lh.MaLH = sp.MaLH " +
                 " left join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "WHERE lh.TenLH = N'" + sp.TenLH + "'");
            List<SP_LH_CTGG> ListSPTuongTu = query1.ToList();

            ViewBag.listProductTT = ListSPTuongTu;


            if (Session["User"] != null)
            {
                var user = Session["User"] as THNN.Models.KhachHang;
                var query2 = db.Database.SqlQuery<DonDatHang>(
                "select *" +
                "from DonDatHang " +
                "Where TinhTrang = N'Đang mua hàng' and MaKH = '" + user.MaKH + "'");
                List<DonDatHang> listDDH = query2.ToList();
                Session["getListBuyDDH"] = listDDH;

            }

            return View(sp);
        }


      
    }
}