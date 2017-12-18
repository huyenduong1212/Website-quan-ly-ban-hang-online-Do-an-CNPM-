using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
  public class SuKienController : Controller
  {
    // GET: SuKien
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    [Authorize(Roles ="16")]
    public ActionResult Index()
    {
      IEnumerable<SUKIEN> lstSuKien = db.SUKIENs;
      return View(lstSuKien);
    }
    public ActionResult ThemSuKien()
    {
      return View();
    }
    [HttpPost]
    public ActionResult ThemSuKien(SUKIEN sk, HttpPostedFileBase[] HinhAnh)
    {
      int loi = 0;
      for (int i = 0; i < HinhAnh.Count(); i++)
      {
        if (HinhAnh[i] != null)
        {
          //Kiểm tra nội dung hình ảnh
          if (HinhAnh[i].ContentLength > 0)
          {
            //Kiểm tra định dạng hình ảnh
            if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
            {
              ViewBag.upload += "Hình ảnh" + i + " không hợp lệ <br />";
              loi++;
            }
            else
            {
              //Kiểm tra hình ảnh tồn tại

              //Lấy tên hình ảnh
              var fileName = Path.GetFileName(HinhAnh[0].FileName);
              //Lấy hình ảnh chuyển vào thư mục hình ảnh 
              var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
              //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
              if (System.IO.File.Exists(path))
              {
                ViewBag.upload1 = "Hình " + i + "đã tồn tại <br />";
                loi++;
              }
            }
          }
        }
        else
        {
          db.SUKIENs.Add(sk);
          db.SaveChanges();
          return RedirectToAction("Index");
        }
      }
      if (loi > 0)
      {
        return View(sk);
      }
      sk.HinhAnh = HinhAnh[0].FileName;
      //Kiểm tra hình tổn tại trong csdl chưa
      if (HinhAnh[0].ContentLength > 0)
      {
        //Lấy tên hình ảnh
        var fileName = Path.GetFileName(HinhAnh[0].FileName);
        //Lấy hình ảnh chuyển vào thư mục hình ảnh 
        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
        //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
        if (System.IO.File.Exists(path))
        {
          ViewBag.upload = "Hình đã tồn tại";
          return View();
        }
        else
        {
          //Lấy hình ảnh đưa vào thư mục HinhAnhSP
          HinhAnh[0].SaveAs(path);
          sk.HinhAnh = fileName;
        }
      }
      db.SUKIENs.Add(sk);
      db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult SuaSuKien(int? maSuKien)
    {
      if (maSuKien == null)
      {
        Response.StatusCode = 404;
        return null;
      }
      SUKIEN sp = db.SUKIENs.SingleOrDefault(n => n.MaSuKien == maSuKien);
      if (sp == null)
      {
        return HttpNotFound();
      }
      //Load dropdownlist Mã Sự kiện và dropdownlist loại sp
      return View(sp);
    }
    [ValidateInput(false)]
    [HttpPost]
    public ActionResult SuaSuKien(SUKIEN sk, HttpPostedFileBase[] HinhAnh)
    {
      var suKien = db.SUKIENs.Find(sk.MaSuKien);
      int loi = 0;
      for (int i = 0; i < HinhAnh.Count(); i++)
      {
        if (HinhAnh[i] != null)
        {
          //Kiểm tra nội dung hình ảnh
          if (HinhAnh[i].ContentLength > 0)
          {
            //Kiểm tra định dạng hình ảnh
            if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
            {
              ViewBag.upload += "Hình ảnh" + i + " không hợp lệ <br />";
              loi++;
            }
            else
            {
              //Kiểm tra hình ảnh tồn tại

              //Lấy tên hình ảnh
              var fileName = Path.GetFileName(HinhAnh[0].FileName);
              //Lấy hình ảnh chuyển vào thư mục hình ảnh 
              var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
              //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
              if (System.IO.File.Exists(path))
              {
                ViewBag.upload1 = "Hình " + i + "đã tồn tại <br />";
                loi++;
              }
            }
          }
        }
        else
        {

          suKien.TenSuKien = sk.TenSuKien;
          suKien.MoTa = sk.MoTa;
          suKien.HinhAnh = sk.HinhAnh;
          suKien.UuDai = sk.UuDai;
          suKien.NgayBatDau = sk.NgayBatDau;
          suKien.NgayKetThuc = sk.NgayKetThuc;
          db.SaveChanges();
          return RedirectToAction("Index");
        }
      }
      if (loi > 0)
      {
        return View(sk);
      }
      suKien.HinhAnh = HinhAnh[0].FileName;
      //Kiểm tra hình tổn tại trong csdl chưa
      if (HinhAnh[0].ContentLength > 0)
      {
        //Lấy tên hình ảnh
        var fileName = Path.GetFileName(HinhAnh[0].FileName);
        //Lấy hình ảnh chuyển vào thư mục hình ảnh 
        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
        //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
        if (System.IO.File.Exists(path))
        {
          ViewBag.upload = "Hình đã tồn tại";
          return View();
        }
        else
        {
          //Lấy hình ảnh đưa vào thư mục HinhAnhSP
          HinhAnh[0].SaveAs(path);
          suKien.HinhAnh = fileName;
        }
      }
      suKien.TenSuKien = sk.TenSuKien;
      suKien.MoTa = sk.MoTa;
      suKien.HinhAnh = HinhAnh[0].FileName;
      suKien.UuDai = sk.UuDai;
      suKien.NgayBatDau = sk.NgayBatDau;
      suKien.NgayKetThuc = sk.NgayKetThuc;
      db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult XoaSuKien(int? maSuKien)
    {
      if (maSuKien == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      SUKIEN model = db.SUKIENs.SingleOrDefault(n => n.MaSuKien == maSuKien);
      if (model == null)
      {
        return HttpNotFound();
      }
      SUKIEN sp = db.SUKIENs.SingleOrDefault(n => n.MaSuKien == maSuKien);
      db.SUKIENs.Attach(sp);
      db.SUKIENs.Remove(sp);
      db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult SanPhamSuKien(int? maSuKien, int? page)
    {
      if (maSuKien == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var lstSP = db.SANPHAMs.Where(n => n.MaSuKien == maSuKien);
      if (lstSP.Count() == 0)
      {
        return HttpNotFound();
      }
      //Thực hiện chức năng phân trang
      //Tạo biến số sản phẩm trên trang
      int PageSize = 5;
      //Tạo biến thứ 2: Số trang hiện tại
      int PageNumber = (page ?? 1);
      ViewBag.MaSuKien = maSuKien;
      return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
    }
  }
}