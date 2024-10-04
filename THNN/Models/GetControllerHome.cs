using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class GetControllerHome
    {
        public List<string> SearchList { get; set; }
        public List<SP_LH_CTGG> DanhSachSanPham { get; set; }

        public List<LoaiHang> DanhSachLoaiHang { get; set; }



        public GetControllerHome() { }

        public GetControllerHome(List<string> searchList, List<SP_LH_CTGG> danhSachSanPham, List<LoaiHang> danhSachLoaiHang)
        {
            SearchList = searchList;
            DanhSachSanPham = danhSachSanPham;
            DanhSachLoaiHang = danhSachLoaiHang;
        }
    }
}