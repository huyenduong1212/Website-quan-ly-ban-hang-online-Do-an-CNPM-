﻿using System;
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
      IEnumerable<SANPHAM> lstSPLT = db.SANPHAMs.Where(n => n.MaLoaiSP == 3 && n.DaXoa == false);
      //Gán vào ViewBag
      ViewBag.ListLTM = lstLT;

      //List Máy tính bảng mới nhất
      var lstMTB = db.SANPHAMs.Where(n => n.MaLoaiSP == 2 /*&& n.Moi == 1*/ && n.DaXoa == false);
      //Gán vào ViewBag
      ViewBag.ListMTBM = lstMTB;

      return View();
    }
    public ActionResult BannerPartial()
    {
      //IEnumerable<SUKIEN> lstSuKien = db.SUKIENs.Where(n => n.NgayBatDau.Value.Date > DateTime.Now.Date);
      //IEnumerable<SUKIEN> lstSuKien = db.SUKIENs.Where(n => n.NgayBatDau.Value.Date > DateTime.Now.Date);
      IEnumerable<SUKIEN> lstSuKien = db.SUKIENs;
      List<SUKIEN> lstSuKienCurrent = new List<SUKIEN>();
      int day = DateTime.Now.Day;
      int month = DateTime.Now.Month;
      int year = DateTime.Now.Year;
      foreach (SUKIEN sk1 in lstSuKien)
      {
        if (DateTime.Compare(sk1.NgayBatDau.Value, DateTime.Now) < 0 && DateTime.Compare(sk1.NgayKetThuc.Value, DateTime.Now) > 0)
        {
          lstSuKienCurrent.Add(sk1);
        }
      }
      //foreach (SUKIEN sk1 in lstSuKien)
      //{
      //  if (sk1.NgayBatDau.Value.Year < year && sk1.NgayKetThuc.Value.Year > year)
      //  {
      //    if (sk1.NgayBatDau.Value.Month < month && sk1.NgayKetThuc.Value.Month > month)
      //    {
      //      lstSuKienCurrent.Add(sk1);
      //    }
      //    else if (sk1.NgayBatDau.Value.Month == month)
      //    {
      //      if (sk1.NgayBatDau.Value.Day < day && sk1.NgayKetThuc.Value.Day > day)
      //      {
      //        lstSuKienCurrent.Add(sk1);
      //      }
      //    }
      //    else if (sk1.NgayKetThuc.Value.Month == month)
      //    {
      //      if (sk1.NgayBatDau.Value.Day < day && sk1.NgayKetThuc.Value.Day > day)
      //      {
      //        lstSuKienCurrent.Add(sk1);
      //      }
      //    }
      //  }
      return PartialView(lstSuKienCurrent);
    }
    public ActionResult MenuPartial()
    {
      //Truy vấn lấy về 1 list các sản phẩm
      var lstSP = db.SANPHAMs;
      return PartialView(lstSP);
    }
    public ActionResult DangKy()
    {
      //ViewBag.CauHoi = new SelectList(LoadCauHoi());

      return View();
    }
    [HttpPost]
    public ActionResult DangKy(NGUOIDUNG nd, FormCollection f)
    {
      string TaiKhoan = f["TaiKhoan"].ToString();
      NGUOIDUNG nguoidung = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == TaiKhoan);
      if (nguoidung != null)
      {
        return Content("Tài Khoản Đã Tồn Tại");
      }
      else
      {
        nd.MaLoaiNguoiDung = 1;
        nd.TrangThai = true;
        db.NGUOIDUNGs.Add(nd);
        db.SaveChanges();
        NGUOIDUNG nguoidungs = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == nd.TaiKhoan && n.MatKhau == nd.MatKhau);
        Session["NGUOIDUNG"] = nguoidungs;
        return JavaScript("window.location = '" + Url.Action("Index", "Home") + "'");
      }
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
        if (tv.TrangThai == false)
        {
          return Content("Tài khoản đã bị khóa!");
        }
        else
        {
          var lstQuyen = db.QUYENHANLOAINGUOIDUNGs.Where(n => n.MaLoaiNguoiDung == tv.MaLoaiNguoiDung);
          string Quyen = "";
          if (lstQuyen.Count() != 0)
          {
            foreach (var item in lstQuyen)
            {
              Quyen += item.MaChucNang + ",";
            }
            Quyen = Quyen.Substring(0, Quyen.Length - 1);
            PhanQuyen(tv.MaNguoiDung.ToString(), Quyen);
            Session["NGUOIDUNG"] = tv;
            if (tv.MaLoaiNguoiDung == 1)
            {
              return Content("<script>window.location.reload();</script>");
            }
            if (tv.MaLoaiNguoiDung == 3)
            {
              return JavaScript("window.location = '" + Url.Action("Index", "QuanLyTaiKhoan") + "'");
            }
            if (tv.MaLoaiNguoiDung == 4)
            {
              return JavaScript("window.location = '" + Url.Action("Index", "QuanLyTaiKhoan") + "'");
            }
            if (tv.MaLoaiNguoiDung == 5)
            {
              return JavaScript("window.location = '" + Url.Action("Index", "QuanLySanPham") + "'");
            }
            if (tv.MaLoaiNguoiDung == 4)
            {
              return JavaScript("window.location = '" + Url.Action("Index", "QuanLyKhachHang") + "'");
            }
          }
        }
      }
      return Content("Tài khoản hoặc mật khẩu không đúng!");
    }
    public ActionResult LoiTruyCap()
    {
      return View();
    }
    [Authorize(Roles ="31")]
    public void PhanQuyen(string tv, string Quyen)
    {
      FormsAuthentication.Initialize();
      var ticket = new FormsAuthenticationTicket(1, tv, DateTime.Now, DateTime.Now.AddHours(3), true, Quyen);
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