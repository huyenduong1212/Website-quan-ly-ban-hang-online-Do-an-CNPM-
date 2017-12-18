using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;
using WebSiteBanHang.ViewModels;

namespace WebSiteBanHang.Controllers
{
  public class SanPhamController : Controller
  {
    //
    // GET: /SanPham/
    //Sử dụng partial view
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();

    //Tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩm theo 2 style khác nhau
    [ChildActionOnly]
    public ActionResult SanPhamStyle1Partial()
    {

      return PartialView();
    }
    [ChildActionOnly]
    public ActionResult SanPhamStyle2Partial()
    {

      return PartialView();
    }
    //Xây dựng trang xem chi tiết 
    public ActionResult XemChiTiet(int? id)
    {
      int TongDiem = 0;
      //Kiểm tra tham số truyền vào có rổng hay không
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      //Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
      SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
      if (sp == null)
      {
        //Thông báo nếu như không có sản phẩm đó
        return HttpNotFound();
      }
      IEnumerable<DANHGIA> lstdanhgia = db.DANHGIAs.Where(n => n.MaSP == id);
      List<KhachHangDanhGia_ViewModels> lstThongTinKHDanhGia = new List<KhachHangDanhGia_ViewModels>();
      KhachHangDanhGia_ViewModels thongtinkhachhang = null;
      foreach (DANHGIA itemDanhGia in lstdanhgia)
      {
        NGUOIDUNG nguoidung = db.NGUOIDUNGs.Where(n => n.MaNguoiDung == itemDanhGia.MaNguoiDungKhachHang).SingleOrDefault();
        thongtinkhachhang = new KhachHangDanhGia_ViewModels
        {
          MaSP = (int)id,
          DiemDanhGia = (int)itemDanhGia.DiemDanhGia,
          HoTen = nguoidung.Ho + " " + nguoidung.TenLot + " " + nguoidung.Ten,
          MaBL = itemDanhGia.MaBL,
          NoiDungBL = itemDanhGia.NoiDungBL,
          MaKH = nguoidung.MaNguoiDung,
          ThoiDiem = DateTime.Now
        };
        TongDiem += thongtinkhachhang.DiemDanhGia;
        lstThongTinKHDanhGia.Add(thongtinkhachhang);
      }
      ViewBag.thongTinNguoiDung = lstThongTinKHDanhGia;
      ViewBag.DiemTrungBinh = TongDiem/lstThongTinKHDanhGia.Count;
      return View(sp);
    }
    public ActionResult GuiDanhGia(string comment, string rating, int maSP)
    {
      NGUOIDUNG nguoidung = Session["NGUOIDUNG"] as NGUOIDUNG;
      int count = 0;
      IEnumerable<GIOHANG> lstgiohang = db.GIOHANGs.Where(n => n.MaKH == nguoidung.MaNguoiDung && n.DaDat == true);
      foreach (GIOHANG gh in lstgiohang)
      {
        CHITIETGIOHANG ctgh = db.CHITIETGIOHANGs.Where(n => n.MaGioHang == gh.MaGioHang && n.MaSP == maSP).SingleOrDefault();
        if(ctgh!=null)
        { 
          count++;
          break;
        }
      }
      if (count == 0)
      {
        //TempData["result"] = "Bạn chưa mua sản phẩm này, không thể đánh giá";
        //RedirectToAction("XemChiTiet", new { @id = maSP });
        //return Content("<script>window.location.reload();</script>");
        return Content("<script>window.location.href= window.location;</ script>");
        //return RedirectToAction("Index", "Home");
        //return RedirectToAction("XemChiTiet", new { @id = maSP });
        //return Content("<script>window.location.reload();</script>");
        //return RedirectToAction("XemChiTiet","SanPham",new { @id=maSP});

      }
      DANHGIA danhgia = new DANHGIA();
      
      danhgia.NoiDungBL = comment;
      danhgia.DiemDanhGia = Convert.ToInt32(rating);
      danhgia.MaSP = maSP;
      danhgia.MaNguoiDungKhachHang = nguoidung.MaNguoiDung;
      danhgia.ThoiDiem = DateTime.Now;
      db.DANHGIAs.Add(danhgia);
      db.SaveChanges();
      return RedirectToAction("Index", "Home");
      //return RedirectToAction("XemChiTiet", new { @id = maSP });
      //return RedirectToAction("XemChiTiet", new { @id = maSP });
      //return Content("<script>window.location.reload();</script>");
    }
    public ActionResult SanPham(int? MaLoaiSP, int? page)
    {
      if (MaLoaiSP == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var lstSP = db.SANPHAMs.Where(n => n.MaLoaiSP == MaLoaiSP);
      if (lstSP.Count() == 0)
      {
        return HttpNotFound();
      }
      //Thực hiện chức năng phân trang
      //Tạo biến số sản phẩm trên trang
      int PageSize = 5;
      //Tạo biến thứ 2: Số trang hiện tại
      int PageNumber = (page ?? 1);
      ViewBag.MaLoaiSP = MaLoaiSP;
      return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
    }
    //Xây dựng 1 action load sản phẩm theo mã loại sản phẩm và mã nhà sản xuất
    //public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
    //{
    //  //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
    //  //{
    //  //    return RedirectToAction("Index","Home");
    //  //}

    //  if (MaLoaiSP == null || MaNSX == null)
    //  {
    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //  }
    //  /*Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và mã nhà sản xuất (2 trường
    //  trong bảng sản phẩm */
    //  var lstSP = db.SANPHAMs.Where(n => n.MaLoaiSP == MaLoaiSP/* && n.MaNSX == MaNSX*/);
    //  if (lstSP.Count() == 0)
    //  {
    //    //Thông báo nếu như không có sản phẩm đó
    //    return HttpNotFound();
    //  }
    //  //Thực hiện chức năng phân trang
    //  //Tạo biến số sản phẩm trên trang
    //  int PageSize = 2;
    //  //Tạo biến thứ 2: Số trang hiện tại
    //  int PageNumber = (page ?? 1);
    //  ViewBag.MaLoaiSP = MaLoaiSP;
    //  ViewBag.MaNSX = MaNSX;
    //  return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
    //}
  }
}