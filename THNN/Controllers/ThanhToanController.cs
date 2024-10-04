using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THNN.Models;
using System.Globalization;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Security.Policy;


namespace THNN.Controllers
{
    public class ThanhToanController : Controller
    {

        public string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public string returnUrl = $"https://localhost:44390/ThanhToan/PaymentConfirm";
        public string tmnCode = "KQ1WYAS1";
        public string hashSecret = "WJNM81QOFAQ5E583TNFUJHDBUU69UD2M";
        // GET: ThanhToan
        public ActionResult Index(int amount, string orderInfo)
        {
            ViewBag.amount = amount;
            ViewBag.orderInfo = orderInfo;
            return View();
        }

        public ActionResult Success(string PaymentTime, string TotalPrice, string OrderInfor, long VnpayTranId)
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
            DateTime parsedDate = DateTime.ParseExact(PaymentTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.paymentTime = formattedDate;
            ViewBag.totalPrice = TotalPrice;
            ViewBag.orderInfor = OrderInfor;
            ViewBag.vnpayTranId = VnpayTranId;

            List<SP_LH_DDH_CTDDH> listBuyKH = Session["getListBuyKH"] as List<SP_LH_DDH_CTDDH>;
            foreach (var item in listBuyKH)
            {
                SanPham sanPham = db.SanPhams.FirstOrDefault(m => m.MaSP == item.MaSP);
                sanPham.SoLuongT = sanPham.SoLuongT - item.SoLuong;
                db.SaveChanges();
            }
            DonDatHang model = db.DonDatHangs.FirstOrDefault(m => m.MaDDH == OrderInfor);
            if (model != null)
            {
                model.TinhTrang = "Đã đặt hàng";

                
                db.SaveChanges();
                SendEmail(model, listBuyKH);
                listBuyKH.Clear();
                Session["getListBuyKH"] = listBuyKH;

            }

            return View();
        }


        public ActionResult FailPay()
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
            return View();
        }

        [HttpPost]
        public ActionResult submitOrder(int amount, string orderInfo)
        {
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();
            Paylib pay = new Paylib();

            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", clientIPAddress);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", orderInfo);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", returnUrl);
            pay.AddRequestData("vnp_TxnRef", orderInfo);

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return Redirect(paymentUrl);
        }

        [HttpGet]
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.HasKeys())
            {
                //lấy toàn bộ dữ liệu trả về
                var queryString = Request.QueryString.ToString();
                var json = HttpUtility.ParseQueryString(queryString);

                string paymentTime = json["vnp_PayDate"].ToString();
                string totalPrice = json["vnp_Amount"].ToString();
                string orderInfor = json["vnp_OrderInfo"].ToString(); 
                long vnpayTranId = Convert.ToInt64(json["vnp_TransactionNo"]); 
                string vnp_ResponseCode = json["vnp_ResponseCode"].ToString(); 
                string vnp_SecureHash = json["vnp_SecureHash"].ToString(); 
                var pos = queryString.IndexOf("&vnp_SecureHash");

               
                bool checkSignature = ValidateSignature(queryString.Substring(0, pos), vnp_SecureHash, hashSecret); 
                if (checkSignature && tmnCode == json["vnp_TmnCode"].ToString())
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        return RedirectToAction("Success","ThanhToan", new
                        {
                            PaymentTime = paymentTime,
                            TotalPrice = totalPrice,
                            OrderInfor = orderInfor,
                            VnpayTranId = vnpayTranId
                        });
                    }
                    else
                    {
                        
                        return RedirectToAction("FailPay", "ThanhToan");
                    }
                }
                else
                {
                    return RedirectToAction("FailPay", "ThanhToan");
                }
            }
            return RedirectToAction("FailPay", "ThanhToan");

        }
        public bool ValidateSignature(string rspraw, string inputHash, string secretKey)
        {
            string myChecksum = Paylib.HmacSHA512(secretKey, rspraw);
            System.Diagnostics.Debug.WriteLine("RSPRAW: " + rspraw);
            System.Diagnostics.Debug.WriteLine("InputHash: " + inputHash);
            System.Diagnostics.Debug.WriteLine("MyChecksum: " + myChecksum);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public void SendEmail(DonDatHang ddh, List<SP_LH_DDH_CTDDH> listBuyKH)
        {
            var strSanPham = "";
            var emailKhachHang = "";
            DateTime? ngayDH = new DateTime();
            foreach (var item in listBuyKH)
            {
                strSanPham += "<tr>";
                strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\">" + item.TenSP + "</td>";
                strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + item.SoLuong + "</td>";
                strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\"><span>" + item.GiaBanSP + "&nbsp;<span>₫</span></span></td>";
                strSanPham += "</tr>";
                emailKhachHang = item.Email;
                ngayDH = item.NgayDH;
            }

            string formattedDate = ngayDH?.ToString("dd-MM-yyyy");

            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/assets/template/send2.html"));
            contentCustomer = contentCustomer.Replace("{{MaDon}}", ddh.MaDDH);
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", formattedDate);
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ddh.TriGia.ToString());
            contentCustomer = contentCustomer.Replace("{{PhuongThucTT}}", ddh.HTThanhToan);
            contentCustomer = contentCustomer.Replace("{{TongCong}}", ddh.TriGia.ToString());
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", ddh.TenNguoiNhan);
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", ddh.DiaChiNhan);
            contentCustomer = contentCustomer.Replace("{{SoDT}}", ddh.SDTNhanHang);
            contentCustomer = contentCustomer.Replace("{{Email}}", emailKhachHang);

            Common.Common.SendMail("Agrofood", "Đơn hàng #" + ddh.MaDDH, contentCustomer.ToString(), emailKhachHang);


        }
    }
}