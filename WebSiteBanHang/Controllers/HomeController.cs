using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebSiteBanHang.Controllers
{
  public class HomeController : Controller
  {
    //
    // GET: /Home/
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    public ActionResult Index()
    {
      //Lần lượt tạo các viewbag để lấy list sản phẩm từ cơ sở dữ liệu
      //List Diện điện thoại mới nhất
      var lstDTM = db.SANPHAMs.Where(n => n.MaLoaiSP == 1/* && n.Moi == 1*/ && n.DaXoa == false);
      //Gán vào ViewBag
      ViewBag.ListDTM = lstDTM;

      //List LapTop mới nhất
      var lstLT = db.SANPHAMs.Where(n => n.MaLoaiSP == 3 /*&& n.Moi == 1 */&& n.DaXoa == false);
      //Gán vào ViewBag
      ViewBag.ListLTM = lstLT;

      //List Máy tính bảng mới nhất
      var lstMTB = db.SANPHAMs.Where(n => n.MaLoaiSP == 2 /*&& n.Moi == 1*/ && n.DaXoa == false);
      //Gán vào ViewBag
      ViewBag.ListMTBM = lstMTB;

      return View();
    }

    public ActionResult MenuPartial()
    {
      //Truy vấn lấy về 1 list các sản phẩm
      var lstSP = db.SANPHAMs;
      return PartialView(lstSP);
    }
    [HttpGet]
    public ActionResult DangKy()
    {
      ViewBag.CauHoi = new SelectList(LoadCauHoi());

      return View();
    }

    [HttpPost, CaptchaMvc.Attributes.CaptchaVerify("Captcha is not valid")]
    //public ActionResult DangKy(ThanhVien tv,FormCollection f)
    //{
    //    ViewBag.CauHoi = new SelectList(LoadCauHoi());
    //    //Kiểm tra captcha hợp lệ
    //    if (this.IsCaptchaValid("Captcha is not valid"))
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            ViewBag.ThongBao = "Thêm thành công";
    //            //Thêm khách hàng vào csdl
    //            db.ThanhViens.Add(tv);
    //            db.SaveChanges();
    //        }
    //        else
    //        { 
    //            ViewBag.ThongBao = "Thêm thất bại";
    //        }
    //        return View();
    //    }
    //    TempData["Message"] = "Message: blahblah";
    //    ViewBag.ThongBao = "Sai mã captcha";


    //    return View();
    //}
    [HttpGet]
    public ActionResult DangKy1()
    {
      return View();
    }
    [HttpPost]
    //public ActionResult DangKy1(ThanhVien tv)
    //{
    //    return View();
    //}
    //Load câu hỏi bí mật
    public List<string> LoadCauHoi()
    {
      List<string> lstCauHoi = new List<string>();
      lstCauHoi.Add("Con vật mà bạn yêu thích?");
      lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
      lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
      return lstCauHoi;
    }

    //Xây dựng action đăng nhập
    //[HttpPost]
    public ActionResult DangNhap(FormCollection f)
    {
      //Kiểm tra tên đăng nhập và mật khẩu
      string sTaiKhoan = f["txtTenDangNhap"].ToString();
      string sMatKhau = f["txtMatKhau"].ToString();
      NGUOIDUNG tv = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
      if (tv != null)
      {
        var lstQuyen= db.QUYENHANLOAINGUOIDUNGs.Where(n => n.MaLoaiNguoiDung == tv.MaLoaiNguoiDung); 
        string quyen = "";
        if (lstQuyen.Count() != 0)
        {
          foreach (var item in lstQuyen)
          {
            quyen += item.MaChucNang + ",";
          }
          PhanQuyen(tv.MaNguoiDung.ToString(), quyen);
          Session["NGUOIDUNG"] = tv;
          return Content("<script>window.location.reload();</script>");
        }
      }
      return Content("Tài khoản hoặc mật khẩu không đúng!");
    }
    public ActionResult LoiTruyCap()
    {
      return View();
    }
    public void PhanQuyen(string tv, string quyen)
    {
      FormsAuthentication.Initialize();
      var ticket = new FormsAuthenticationTicket(1, tv, DateTime.Now, DateTime.Now.AddHours(3), true, quyen);
      var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));//value (đã được mã hóa)
      if (ticket.IsPersistent)//true nếu cookie đã được cấp 
      {
        cookie.Expires = ticket.Expiration;//cấp thời gian sống cho cookie
      }
      Response.Cookies.Add(cookie);//gán cookie trả về cho client
    }
    public ActionResult DangXuat()
    {
      Session["NGUOIDUNG"] = null;
      return RedirectToAction("Index");
    }
  }
}