using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;
using Microsoft.Reporting.WinForms;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;

namespace THNN.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {

        public void dataStatistical()
        {
            THNNEntities1 db = new THNNEntities1();
            var query2 = db.Database.SqlQuery<DoanhThuLoaiHang>(
                "select lh.MaLH,lh.TenLH,sum(ctddh.SoLuong) as'SoLuong', sum(ctddh.SoLuong * ctddh.GiaBan) as 'TongTien' " +
                "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH" +
                " join SanPham sp on sp.MaSP = ctddh.MaSP " +
                "join LoaiHang lh on lh.MaLH = sp.MaLH " +
                "group by lh.TenLH,lh.MaLH");
            List<DoanhThuLoaiHang> listDoanhThuLH = query2.ToList();
            ViewBag.DoanhThuLH = listDoanhThuLH;
            Session["FinalDataReportLoaiHang"] = listDoanhThuLH;


            var query3 = db.Database.SqlQuery<DoanhThuSanPham>(
                "select sp.MaSP,sp.TenSP,lh.TenLH, sum(ctddh.SoLuong) as'SoLuong', sum(ctddh.SoLuong * ctddh.GiaBan) as 'TongTien' " +
                "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
                "join SanPham sp on sp.MaSP = ctddh.MaSP " +
                "join LoaiHang lh on lh.MaLH = sp.MaLH " +
                "group by sp.TenSP,lh.TenLH,sp.MaSP");
            List<DoanhThuSanPham> listDoanhThuSP = query3.ToList();
            ViewBag.DoanhThuSP = listDoanhThuSP;
            Session["FinalDataReportSanPham"] = listDoanhThuSP;

            var query4 = db.Database.SqlQuery<DoanhThuNhanVien>(
                "select nv.MaNV, nv.HoTen,count(hd.MaHD) as 'SoLuong' ,sum(ctddh.SoLuong * ctddh.GiaBan) as 'TongTien' " +
                "from HoaDon hd " +
                "join NhanVien nv on nv.MaNV = hd.MaNV " +
                "join CTDonDatHang ctddh on ctddh.MaDDH = hd.MaDDH " +
                "group by nv.MaNV, nv.HoTen");
            List<DoanhThuNhanVien> listDoanhThuNV = query4.ToList();
            ViewBag.DoanhThuNV = listDoanhThuNV;
            Session["FinalDataReportNhanVien"] = listDoanhThuNV;

            var query5 = db.Database.SqlQuery<DoanhThuCTGG>(
                "select ctgg.MaCT,ctgg.TenSuKien,sum(ctddh.SoLuong) as'SoLuong', sum(ctddh.SoLuong * ctddh.GiaBan) as 'TongTien' " +
                "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
                "join SanPham sp on sp.MaSP = ctddh.MaSP " +
                "join LoaiHang lh on lh.MaLH = sp.MaLH " +
                "join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
                "group by ctgg.MaCT,ctgg.TenSuKien");
            List<DoanhThuCTGG> listDoanhThuCTGG = query5.ToList();
            ViewBag.DoanhThuCTGG = listDoanhThuCTGG;
            Session["FinalDataReportCTGG"] = listDoanhThuCTGG;


        }

        public List<object> FilterDataMethod(string type, string table, DateTime NgayBD, DateTime NgayKT)
        {
            THNNEntities1 db = new THNNEntities1();
            switch (table)
            {
                case "Đơn Đặt Hàng":
                    var whereClausesDDH = new Dictionary<string, string>
                    {
                        { "Ngày hôm qua", "WHERE hd.NgayBan = DATEADD(day, -1, CAST(GETDATE() AS date))" },
                        { "Ngày hiện tại", "WHERE hd.NgayBan = CAST(GETDATE() AS date)" },
                        { "Tháng hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(GETDATE())" },
                        { "Tháng trước", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(DATEADD(month, -1, GETDATE()))" },
                        { "Năm hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE())" },
                        { "Năm trước", "WHERE YEAR(hd.NgayBan) = YEAR(DATEADD(year, -1, GETDATE()))" },
                        {"Khoảng thời gian","where hd.NgayBan between '"+NgayBD+"'AND '"+NgayKT+"'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryDDH = "select * " +
                        "from HoaDon hd ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesDDH.ContainsKey(type))
                    {
                        sqlQueryDDH += whereClausesDDH[type];
                    }


                    // Thực hiện truy vấn
                    var queryDDH = db.Database.SqlQuery<HoaDon>(sqlQueryDDH);
                    List<HoaDon> hoaDons = queryDDH.ToList();
                    return hoaDons.Cast<object>().ToList();

                case "Loại Hàng":
                    var whereClausesLH = new Dictionary<string, string>
                    {
                        { "Ngày hôm qua", "WHERE hd.NgayBan = DATEADD(day, -1, CAST(GETDATE() AS date))" },
                        { "Ngày hiện tại", "WHERE hd.NgayBan = CAST(GETDATE() AS date)" },
                        { "Tháng hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(GETDATE())" },
                        { "Tháng trước", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(DATEADD(month, -1, GETDATE()))" },
                        { "Năm hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE())" },
                        { "Năm trước", "WHERE YEAR(hd.NgayBan) = YEAR(DATEADD(year, -1, GETDATE()))" },
                        {"Khoảng thời gian","where hd.NgayBan between '"+NgayBD+"'AND '"+NgayKT+"'" }
                    };

                                // Xây dựng truy vấn SQL cơ bản
                                string sqlQueryLH = "SELECT lh.MaLH, lh.TenLH, SUM(ctddh.SoLuong) AS 'SoLuong', sum(ctddh.SoLuong * ctddh.GiaBan) AS 'TongTien' " +
                                                  "FROM HoaDon hd " +
                                                  "JOIN DonDatHang ddh ON hd.MaDDH = ddh.MaDDH " +
                                                  "JOIN CTDonDatHang ctddh ON ctddh.MaDDH = ddh.MaDDH " +
                                                  "JOIN SanPham sp ON sp.MaSP = ctddh.MaSP " +
                                                  "JOIN LoaiHang lh ON lh.MaLH = sp.MaLH ";

                                // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                                if (whereClausesLH.ContainsKey(type))
                                {
                                     sqlQueryLH += whereClausesLH[type];
                                }

                                    // Thêm phần GROUP BY
                                    sqlQueryLH += " GROUP BY lh.TenLH, lh.MaLH";

                                // Thực hiện truy vấn
                                var queryLH = db.Database.SqlQuery<DoanhThuLoaiHang>(sqlQueryLH);
                                List<DoanhThuLoaiHang> loaiHangs = queryLH.ToList();
                                return loaiHangs.Cast<object>().ToList();
                    
                case "Sản Phẩm":
                    var whereClausesSP = new Dictionary<string, string>
                    {
                        { "Ngày hôm qua", "WHERE hd.NgayBan = DATEADD(day, -1, CAST(GETDATE() AS date))" },
                        { "Ngày hiện tại", "WHERE hd.NgayBan = CAST(GETDATE() AS date)" },
                        { "Tháng hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(GETDATE())" },
                        { "Tháng trước", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(DATEADD(month, -1, GETDATE()))" },
                        { "Năm hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE())" },
                        { "Năm trước", "WHERE YEAR(hd.NgayBan) = YEAR(DATEADD(year, -1, GETDATE()))" },
                        {"Khoảng thời gian","where hd.NgayBan between '"+NgayBD+"'AND '"+NgayKT+"'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQuerySP = "select sp.MaSP,sp.TenSP,lh.TenLH, sum(ctddh.SoLuong) as'SoLuong',  sum(sp.GiaBanSP) as 'TongTien' " +
                        "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                        "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
                        "join SanPham sp on sp.MaSP = ctddh.MaSP " +
                        "join LoaiHang lh on lh.MaLH = sp.MaLH ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesSP.ContainsKey(type))
                    {
                        sqlQuerySP += whereClausesSP[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQuerySP += " group by sp.TenSP,lh.TenLH,sp.MaSP";

                    // Thực hiện truy vấn
                    var querySP = db.Database.SqlQuery<DoanhThuSanPham>(sqlQuerySP);
                    List<DoanhThuSanPham> sanPhams = querySP.ToList();
                    return sanPhams.Cast<object>().ToList();
                case "Nhân Viên":
                    var whereClausesNV = new Dictionary<string, string>
                    {
                        { "Ngày hôm qua", "WHERE hd.NgayBan = DATEADD(day, -1, CAST(GETDATE() AS date))" },
                        { "Ngày hiện tại", "WHERE hd.NgayBan = CAST(GETDATE() AS date)" },
                        { "Tháng hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(GETDATE())" },
                        { "Tháng trước", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(DATEADD(month, -1, GETDATE()))" },
                        { "Năm hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE())" },
                        { "Năm trước", "WHERE YEAR(hd.NgayBan) = YEAR(DATEADD(year, -1, GETDATE()))" },
                        {"Khoảng thời gian","where hd.NgayBan between '"+NgayBD+"'AND '"+NgayKT+"'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryNV = "select nv.MaNV, nv.HoTen,count(hd.MaHD) as 'SoLuong' ,sum(hd.TriGia) as 'TongTien' " +
                        "from HoaDon hd join NhanVien nv on nv.MaNV = hd.MaNV ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesNV.ContainsKey(type))
                    {
                        sqlQueryNV += whereClausesNV[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQueryNV += " group by nv.MaNV, nv.HoTen";

                    // Thực hiện truy vấn
                    var queryNV = db.Database.SqlQuery<DoanhThuNhanVien>(sqlQueryNV);
                    List<DoanhThuNhanVien> nhanViens = queryNV.ToList();
                    return nhanViens.Cast<object>().ToList();
                case "Chương Trình Giảm Giá":
                    var whereClausesCTGG = new Dictionary<string, string>
                    {
                        { "Ngày hôm qua", "WHERE hd.NgayBan = DATEADD(day, -1, CAST(GETDATE() AS date))" },
                        { "Ngày hiện tại", "WHERE hd.NgayBan = CAST(GETDATE() AS date)" },
                        { "Tháng hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE()) AND MONTH(hd.NgayBan) = MONTH(GETDATE())" },
                        { "Tháng trước", "WHERE YEAR(hd.NgayBan) = YEAR(DATEADD(month, -1, GETDATE())) AND MONTH(hd.NgayBan) = MONTH(DATEADD(month, -1, GETDATE()))" },
                        { "Năm hiện tại", "WHERE YEAR(hd.NgayBan) = YEAR(GETDATE())" },
                        { "Năm trước", "WHERE YEAR(hd.NgayBan) = YEAR(DATEADD(year, -1, GETDATE()))" },
                        {"Khoảng thời gian","where hd.NgayBan between '"+NgayBD+"'AND '"+NgayKT+"'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryCTGG = "select ctgg.MaCT,ctgg.TenSuKien,sum(ctddh.SoLuong) as'SoLuong', sum(hd.TriGia) as 'TongTien' " +
                        "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                        "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
                        "join SanPham sp on sp.MaSP = ctddh.MaSP " +
                        "join LoaiHang lh on lh.MaLH = sp.MaLH " +
                        "join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesCTGG.ContainsKey(type))
                    {
                        sqlQueryCTGG += whereClausesCTGG[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQueryCTGG += " group by ctgg.MaCT,ctgg.TenSuKien";

                    // Thực hiện truy vấn
                    var queryCTGG = db.Database.SqlQuery<DoanhThuCTGG>(sqlQueryCTGG);
                    List<DoanhThuCTGG> ctggs = queryCTGG.ToList();
                    return ctggs.Cast<object>().ToList();
            }
            return null;
        }

        public List<object> SearchDataMethod(string type, string table, string value)
        {
            THNNEntities1 db = new THNNEntities1();
            switch (table)
            {
                case "Đơn Đặt Hàng":
                    var whereClausesDDH = new Dictionary<string, string>
                    {
                        { "Mã Hóa Đơn", " where hd.MaHD like '%"+value+"%'" },
                        { "Mã Đơn Đặt Hàng", " where hd.MaDDH like '%"+value+"%'" },
                        { "Mã Nhân Viên", "where hd.MaNV like '%"+value+"%'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryDDH = "select * " +
                        "from HoaDon hd ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesDDH.ContainsKey(type))
                    {
                        sqlQueryDDH += whereClausesDDH[type];
                    }


                    // Thực hiện truy vấn
                    var queryDDH = db.Database.SqlQuery<HoaDon>(sqlQueryDDH);
                    List<HoaDon> hoaDons = queryDDH.ToList();
                    return hoaDons.Cast<object>().ToList();

                case "Loại Hàng":
                    var whereClausesLH = new Dictionary<string, string>
                    {
                        { "Mã Loại Hàng", " where lh.MaLH like '%"+value+"%'" },
                        { "Tên Loại Hàng", " where lh.TenLH like N'%"+value+"%'" }

                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryLH = "SELECT lh.MaLH, lh.TenLH, SUM(ctddh.SoLuong) AS 'SoLuong',  sum(ctddh.SoLuong * ctddh.GiaBan) AS 'TongTien' " +
                                      "FROM HoaDon hd " +
                                      "JOIN DonDatHang ddh ON hd.MaDDH = ddh.MaDDH " +
                                      "JOIN CTDonDatHang ctddh ON ctddh.MaDDH = ddh.MaDDH " +
                                      "JOIN SanPham sp ON sp.MaSP = ctddh.MaSP " +
                                      "JOIN LoaiHang lh ON lh.MaLH = sp.MaLH ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesLH.ContainsKey(type))
                    {
                        sqlQueryLH += whereClausesLH[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQueryLH += " GROUP BY lh.TenLH, lh.MaLH";

                    // Thực hiện truy vấn
                    var queryLH = db.Database.SqlQuery<DoanhThuLoaiHang>(sqlQueryLH);
                    List<DoanhThuLoaiHang> loaiHangs = queryLH.ToList();
                    return loaiHangs.Cast<object>().ToList();

                case "Sản Phẩm":
                    var whereClausesSP = new Dictionary<string, string>
                    {
                        { "Mã Sản Phẩm"," where sp.MaSP like '%"+value+"%'" },
                        { "Tên Sản Phẩm"," where sp.TenSP like N'%"+value+"%'" },
                        { "Tên Loại Hàng"," where lh.TenLH like N'%"+value+"%'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQuerySP = "select sp.MaSP,sp.TenSP,lh.TenLH, sum(ctddh.SoLuong) as'SoLuong', sum(sp.GiaBanSP) as 'TongTien' " +
                        "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                        "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
                        "join SanPham sp on sp.MaSP = ctddh.MaSP " +
                        "join LoaiHang lh on lh.MaLH = sp.MaLH ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesSP.ContainsKey(type))
                    {
                        sqlQuerySP += whereClausesSP[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQuerySP += " group by sp.TenSP,lh.TenLH,sp.MaSP";

                    // Thực hiện truy vấn
                    var querySP = db.Database.SqlQuery<DoanhThuSanPham>(sqlQuerySP);
                    List<DoanhThuSanPham> sanPhams = querySP.ToList();
                    return sanPhams.Cast<object>().ToList();
                case "Nhân Viên":
                    var whereClausesNV = new Dictionary<string, string>
                    {
                        { "Mã Nhân Viên", " where nv.MaNV like '%"+value+"%'"},
                        { "Tên Nhân Viên"," where nv.HoTen like N'%"+value+"%'" }

                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryNV = "select nv.MaNV, nv.HoTen,count(hd.MaHD) as 'SoLuong' ,sum(ctddh.SoLuong * ctddh.GiaBan) as 'TongTien' " +
                        "from HoaDon hd join NhanVien nv on nv.MaNV = hd.MaNV " +
                        "join CTDonDatHang ctddh on ctddh.MaDDH = hd.MaDDH ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesNV.ContainsKey(type))
                    {
                        sqlQueryNV += whereClausesNV[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQueryNV += " group by nv.MaNV, nv.HoTen";

                    // Thực hiện truy vấn
                    var queryNV = db.Database.SqlQuery<DoanhThuNhanVien>(sqlQueryNV);
                    List<DoanhThuNhanVien> nhanViens = queryNV.ToList();
                    return nhanViens.Cast<object>().ToList();
                case "Chương Trình Giảm Giá":
                    var whereClausesCTGG = new Dictionary<string, string>
                    {
                        { "Mã Chương Trình", " where ctgg.MaCT like N'%"+value+"%'"  },
                        { "Tên Chương Trình", " where ctgg.TenSuKien like N'%"+value+"%'" }
                    };

                    // Xây dựng truy vấn SQL cơ bản
                    string sqlQueryCTGG = "select ctgg.MaCT,ctgg.TenSuKien,sum(ctddh.SoLuong) as'SoLuong',  sum(ctddh.SoLuong * ctddh.GiaBan) as 'TongTien' " +
                        "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
                        "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
                        "join SanPham sp on sp.MaSP = ctddh.MaSP " +
                        "join LoaiHang lh on lh.MaLH = sp.MaLH " +
                        "join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT ";

                    // Kiểm tra và áp dụng điều kiện WHERE từ từ điển
                    if (whereClausesCTGG.ContainsKey(type))
                    {
                        sqlQueryCTGG += whereClausesCTGG[type];
                    }

                    // Thêm phần GROUP BY
                    sqlQueryCTGG += " group by ctgg.MaCT,ctgg.TenSuKien";

                    // Thực hiện truy vấn
                    var queryCTGG = db.Database.SqlQuery<DoanhThuCTGG>(sqlQueryCTGG);
                    List<DoanhThuCTGG> ctggs = queryCTGG.ToList();
                    return ctggs.Cast<object>().ToList();
            }
            return null;
        }


        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            THNNEntities1 db = new THNNEntities1();
            var query1 = db.Database.SqlQuery<HoaDon>(
               "select * " +
                "from HoaDon ");
            List<HoaDon> list = query1.ToList();
            Session["FinalDataReportHoaDon"] = list;
            dataStatistical();
            return View(list);
        }

        [HttpPost]
        public ActionResult FilterData(string type, string table, DateTime NgayBD, DateTime NgayKT)
        {
            List<object> listObject = FilterDataMethod(type, table, NgayBD, NgayKT);
            if (table.Equals("Đơn Đặt Hàng"))
            {
                List<HoaDon> listHoaDon = listObject.Cast<HoaDon>().ToList();
                Session["FinalDataReportHoaDon"] = listHoaDon;
                return Json(new { success = true,data = listHoaDon }, JsonRequestBehavior.AllowGet);
            }
            else if (table.Equals("Loại Hàng"))
            {
                List<DoanhThuLoaiHang> lisDoanhThuLH = listObject.Cast<DoanhThuLoaiHang>().ToList();
                Session["FinalDataReportLoaiHang"] = lisDoanhThuLH;
                return Json(new { success = true, data = lisDoanhThuLH }, JsonRequestBehavior.AllowGet);
            }
            else if (table.Equals("Sản Phẩm"))
            {
                List<DoanhThuSanPham> lisDoanhThuSP = listObject.Cast<DoanhThuSanPham>().ToList();
                Session["FinalDataReportSanPham"] = lisDoanhThuSP;
                return Json(new { success = true, data = lisDoanhThuSP }, JsonRequestBehavior.AllowGet);
            }
            else if (table.Equals("Nhân Viên"))
            {
                List<DoanhThuNhanVien> lisDoanhThuNV = listObject.Cast<DoanhThuNhanVien>().ToList();
                Session["FinalDataReportNhanVien"] = lisDoanhThuNV;
                return Json(new { success = true, data = lisDoanhThuNV }, JsonRequestBehavior.AllowGet);
            }else
            {
                List<DoanhThuCTGG> lisDoanhThuCTGG = listObject.Cast<DoanhThuCTGG>().ToList();
                Session["FinalDataReportCTGG"] = lisDoanhThuCTGG;
                return Json(new { success = true, data = lisDoanhThuCTGG }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchData(string type, string table, string value)
        {
            List<object> listObject = SearchDataMethod(type, table, value);
            if (table.Equals("Đơn Đặt Hàng"))
            {
                List<HoaDon> listHoaDon = listObject.Cast<HoaDon>().ToList();
                Session["FinalDataReportHoaDon"] = listHoaDon;
                return Json(new { success = true, data = listHoaDon }, JsonRequestBehavior.AllowGet);
            }
            else if (table.Equals("Loại Hàng"))
            {
                List<DoanhThuLoaiHang> lisDoanhThuLH = listObject.Cast<DoanhThuLoaiHang>().ToList();
                Session["FinalDataReportLoaiHang"] = lisDoanhThuLH;
                return Json(new { success = true, data = lisDoanhThuLH }, JsonRequestBehavior.AllowGet);
            }
            else if (table.Equals("Sản Phẩm"))
            {
                List<DoanhThuSanPham> lisDoanhThuSP = listObject.Cast<DoanhThuSanPham>().ToList();
                Session["FinalDataReportSanPham"] = lisDoanhThuSP;
                return Json(new { success = true, data = lisDoanhThuSP }, JsonRequestBehavior.AllowGet);
            }
            else if (table.Equals("Nhân Viên"))
            {
                List<DoanhThuNhanVien> lisDoanhThuNV = listObject.Cast<DoanhThuNhanVien>().ToList();
                Session["FinalDataReportNhanVien"] = lisDoanhThuNV;
                return Json(new { success = true, data = lisDoanhThuNV }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<DoanhThuCTGG> lisDoanhThuCTGG = listObject.Cast<DoanhThuCTGG>().ToList();
                return Json(new { success = true, data = lisDoanhThuCTGG }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reports(string ReportType, string MainTable)
        {
            THNNEntities1 db = new THNNEntities1();
            LocalReport localReport = new LocalReport();
            if (MainTable.Equals("Đơn Đặt Hàng"))
            {
                localReport.ReportPath = Server.MapPath("~/Areas/Admin/Report/ReportDoanhThuHoaDon.rdlc");
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSet1";
                List<HoaDon> listFinalReport = Session["FinalDataReportHoaDon"] as List<HoaDon>;
                reportDataSource.Value = listFinalReport.ToList();
                localReport.DataSources.Add(reportDataSource);
                
            }
            if (MainTable.Equals("Loại Hàng"))
            {
                localReport.ReportPath = Server.MapPath("~/Areas/Admin/Report/ReportDoanhThuLoaiHang.rdlc");
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSet1";
                List<DoanhThuLoaiHang> listFinalReport = Session["FinalDataReportLoaiHang"] as List<DoanhThuLoaiHang>;
                reportDataSource.Value = listFinalReport.ToList();
                localReport.DataSources.Add(reportDataSource);

            }

            if (MainTable.Equals("Sản Phẩm"))
            {
                localReport.ReportPath = Server.MapPath("~/Areas/Admin/Report/ReportDoanhThuSanPham.rdlc");
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSet1";
                List<DoanhThuSanPham> listFinalReport = Session["FinalDataReportSanPham"] as List<DoanhThuSanPham>;
                reportDataSource.Value = listFinalReport.ToList();
                localReport.DataSources.Add(reportDataSource);

            }
            if (MainTable.Equals("Nhân Viên"))
            {
                localReport.ReportPath = Server.MapPath("~/Areas/Admin/Report/ReportDoanhThuNhanVien.rdlc");
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSet1";
                List<DoanhThuNhanVien> listFinalReport = Session["FinalDataReportNhanVien"] as List<DoanhThuNhanVien>;
                reportDataSource.Value = listFinalReport.ToList();
                localReport.DataSources.Add(reportDataSource);

            }

            if (MainTable.Equals("Chương Trình Giảm Giá"))
            {
                localReport.ReportPath = Server.MapPath("~/Areas/Admin/Report/ReportDoanhThuCTGG.rdlc");
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSet1";
                List<DoanhThuCTGG> listFinalReport = Session["FinalDataReportCTGG"] as List<DoanhThuCTGG>;
                reportDataSource.Value = listFinalReport.ToList();
                localReport.DataSources.Add(reportDataSource);

            }



            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;
            if (reportType == "Excel")
            {
                mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                encoding = "UTF-8";
                fileNameExtension = "xlsx";
            }
            else if (reportType == "Word")
            {
                mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                encoding = "UTF-8";
                fileNameExtension = "docx";
            }
            else if (reportType == "PDF")
            {
                mimeType = "application/pdf";
                encoding = "UTF-8";
                fileNameExtension = "pdf";
            }

            else
            {
                mimeType = "image/jpg";
                encoding = "UTF-8";
                fileNameExtension = "jpg";
            }

            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
/*            Response.AddHeader("content-disposition", "attachment;filename=doanhthu_report." + fileNameExtension);
            return File(renderedByte, fileNameExtension);*/

            var fileBase64 = Convert.ToBase64String(renderedByte);

            // Trả về kết quả dưới dạng JSON
            var result = new
            {
                FileContents = fileBase64,
                MimeType = mimeType,
                FileName = "doanhthu_report." + fileNameExtension
            };

            return Json(result);


        }


        public ActionResult DetailHoaDon(string keySearch)
        {
            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<ChiTietHoaDon>(
               "select hd.MaHD,hd.MaDDH,ctddh.MaSP,ctddh.SoLuong,ctddh.GiaBan,ctddh.TongTien " +
                "from HoaDon hd join CTDonDatHang ctddh on hd.MaDDH = ctddh.MaDDH " +
                "where hd.MaHD = '"+ keySearch + "'" );
            List<ChiTietHoaDon> list = query.ToList();
            return View(list);
        }

        public ActionResult DetailLoaiHang(string keySearch)
        {
            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<ChiTietHoaDon>(
               "select hd.MaHD,hd.MaDDH,ctddh.MaSP,ctddh.SoLuong,ctddh.GiaBan,ctddh.TongTien " +
               " from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
               " join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH" +
               " join SanPham sp on sp.MaSP = ctddh.MaSP" +
               " join LoaiHang lh on lh.MaLH = sp.MaLH" +
               " where lh.MaLH = '"+keySearch+"'");
            List<ChiTietHoaDon> list = query.ToList();
            return View(list);
        }

        public ActionResult DetailNhanVien(string keySearch)
        {
            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<ChiTietHoaDon>(
               "select hd.MaHD,hd.MaDDH,ctddh.MaSP,ctddh.SoLuong,ctddh.GiaBan,ctddh.TongTien" +
               " from HoaDon hd" +
               " join NhanVien nv on nv.MaNV = hd.MaNV " +
               "join CTDonDatHang ctddh on ctddh.MaDDH = hd.MaDDH" +
               " where nv.MaNV = '"+keySearch+"'");
            List<ChiTietHoaDon> list = query.ToList();
            return View(list);
        }

        public ActionResult DetailChuongTrinhGiamGia(string keySearch)
        {
            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<ChiTietHoaDon>(
               "select hd.MaHD,hd.MaDDH,ctddh.MaSP,ctddh.SoLuong,ctddh.GiaBan,ctddh.TongTien " +
               "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
               "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH" +
               " join SanPham sp on sp.MaSP = ctddh.MaSP" +
               "  join LoaiHang lh on lh.MaLH = sp.MaLH" +
               " join ChuongTrinhGG ctgg on ctgg.MaCT = sp.MaCT " +
               "where ctgg.MaCT = '"+keySearch+"'");
            List<ChiTietHoaDon> list = query.ToList();
            return View(list);
        }

        public ActionResult DetailSanPham(string keySearch)
        {
            THNNEntities1 db = new THNNEntities1();
            var query = db.Database.SqlQuery<ChiTietHoaDon>(
               "select hd.MaHD,hd.MaDDH,ctddh.MaSP,ctddh.SoLuong,ctddh.GiaBan,ctddh.TongTien " +
               "from HoaDon hd join DonDatHang ddh on hd.MaDDH = ddh.MaDDH " +
               "join CTDonDatHang ctddh on ctddh.MaDDH = ddh.MaDDH " +
               "join SanPham sp on sp.MaSP = ctddh.MaSP " +
               "join LoaiHang lh on lh.MaLH = sp.MaLH " +
               "where sp.MaSP = '"+keySearch+"'");
            List<ChiTietHoaDon> list = query.ToList();
            return View(list);
        }


    }
}