using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using WebSiteBanHang.ViewModels;
using System.Data.Entity;

namespace WebSiteBanHang.Controllers
{
  public class GioHangController : Controller
  {

    QuanLyBanHangEntities db = new QuanLyBanHangEntities();

    //Lấy giỏ hàng
    public GIOHANG LayGioHangKhachVangLai()
    {
      GIOHANG lstGioHang = Session["GioHang"] as GIOHANG;
      if (lstGioHang == null)
      {
        lstGioHang = new GIOHANG();
        lstGioHang.DaDat = false;
        lstGioHang.ThanhTien = 0;
        db.GIOHANGs.Add(lstGioHang);
        db.SaveChanges();
        Session["GioHang"] = lstGioHang;
      }
      return lstGioHang;
    }
    public GIOHANG LayGioHangKhachDaDangNhap(int maKH)
    {
      //GIOHANG lstGioHang = Session["GioHang"] as GIOHANG;
      //if (lstGioHang == null)
      //{
      GIOHANG lstGioHang = db.GIOHANGs.Where(n => n.MaKH == maKH).SingleOrDefault(x => x.DaDat == false) as GIOHANG;
      //lstGioHang.MaKH = maKH;
      Session["GioHang"] = lstGioHang;
      //}
      return lstGioHang;
    }
    //Tính tổng số lượng
    public int TinhTongSoLuong()
    {
      //Lấy giỏ hàng
      GIOHANG lstGioHang = Session["GioHang"] as GIOHANG;
      if (lstGioHang == null)
      {
        return 0;
      }
      return (int)lstGioHang.CHITIETGIOHANGs.Sum(n => n.SoLuong);
    }
    //Tính Tổng tiền 
    public decimal TinhTongTien()
    {
      //Lấy giỏ hàng
      GIOHANG lstGioHang = Session["GioHang"] as GIOHANG;
      if (lstGioHang == null)
      {
        return 0;
      }
      return (decimal)lstGioHang.ThanhTien;
    }

    public ActionResult GioHangPartial()
    {
      if (TinhTongSoLuong() == 0)
      {
        ViewBag.TongSoLuong = 0;
        ViewBag.TongTien = 0;
        return PartialView();
      }
      ViewBag.TongSoLuong = TinhTongSoLuong();
      ViewBag.TongTien = TinhTongTien();
      return PartialView();
    }

    // GET: /GioHang/
    public ActionResult XemGioHang()
    {
      //Lấy giỏ hàng 
      GIOHANG lstGioHang = null;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
      }
      else
      {
        lstGioHang = LayGioHangKhachVangLai();
      }
      SANPHAM sp = null;
      List<KhachHang_GioHangViewModel> lstSP_KH = new List<KhachHang_GioHangViewModel>();
      foreach (CHITIETGIOHANG ctgh in lstGioHang.CHITIETGIOHANGs)
      {
        sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == ctgh.MaSP);
        KhachHang_GioHangViewModel sp_KhachHang = new KhachHang_GioHangViewModel()
        {
          MaSP = ctgh.MaSP,
          TenSP = sp.TenSP,
          DonGia = sp.DonGia.Value,
          HinhAnh = sp.HinhAnh,
          MaGioHang = lstGioHang.MaGioHang,
          SoLuong = ctgh.SoLuong.Value
        };
        lstSP_KH.Add(sp_KhachHang);
      }
      return View(lstSP_KH);
    }
    //Thêm giỏ hàng Ajax
    public ActionResult ThemGioHangAjax(int MaSP, string strURL)
    {
      SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
      if (sp == null)
      {
        //Trang đường dẫn không hợp lệ
        Response.StatusCode = 404;
        return null;
      }
      GIOHANG lstGioHang = null;
      //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
      }
      else
      {
        lstGioHang = LayGioHangKhachVangLai();
      }
      //Lấy giỏ hàng
      // GIOHANG 
      //Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng 
      CHITIETGIOHANG spCheck = lstGioHang.CHITIETGIOHANGs.SingleOrDefault(n => n.MaSP == MaSP);
      if (spCheck != null)
      {
        //Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
        if (sp.SoLuongTon < spCheck.SoLuong)
        {
          return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
        }
        spCheck.SoLuong++;
        decimal donGiaSanPham = spCheck.SANPHAM.DonGia.Value;
        spCheck.ThanhTienSP = spCheck.SoLuong * donGiaSanPham;
        ViewBag.TongSoLuong = TinhTongSoLuong();
        ViewBag.TongTien = TinhTongTien();
        return PartialView("GioHangPartial");
      }
      //Nếu sản phẩm chưa tồn tại thì add một record vô trong CHITIETGIOHANG
      CHITIETGIOHANG ctgh = new CHITIETGIOHANG();
      ctgh.MaSP = MaSP;
      ctgh.MaGioHang = lstGioHang.MaGioHang;
      ctgh.SoLuong = 1;
      ctgh.ThanhTienSP = 0;
      db.CHITIETGIOHANGs.Add(ctgh);
      db.SaveChanges();
      //ItemGioHang itemGH = new ItemGioHang(MaSP);
      if (sp.SoLuongTon < ctgh.SoLuong)
      {
        return Content("<script>alert(\"Sản phẩm đã hết hàng!\")</script>");
      }
      //lstGioHang.Add(itemGH);
      ViewBag.TongSoLuong = TinhTongSoLuong();
      ViewBag.TongTien = TinhTongTien();
      return PartialView("GioHangPartial");
    }

    [HttpPost]
    public ActionResult CapNhatGioHang(int maGioHang, int maSP, int soLuongMoi)
    {
      //Kiểm tra số lượng tồn 
      SANPHAM spCheck = db.SANPHAMs.Single(n => n.MaSP == maSP);
      if (spCheck.SoLuongTon < soLuongMoi)
      {
        return View("ThongBao");
      }
      //Cập nhật số lượng trong session giỏ hàng 
      //Bước 1: Lấy List<GioHang> từ session["GioHang"]
      GIOHANG lstGioHang = null;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
      }
      else
      {
        lstGioHang = LayGioHangKhachVangLai();
      }
      //Bước 2: Lấy sản phẩm cần cập nhật từ trong list<GioHang> ra
      CHITIETGIOHANG itemGHUpdate = lstGioHang.CHITIETGIOHANGs.SingleOrDefault(n => n.MaSP == maSP);
      if (itemGHUpdate == null)
      {
        return RedirectToAction("Index", "Home");
      }
      //Bước 3: Tiến hành cập nhật lại số lượng cũng thành tiền
      itemGHUpdate.SoLuong = soLuongMoi;
      SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == maSP);
      itemGHUpdate.ThanhTienSP = itemGHUpdate.SoLuong * sp.DonGia;
      db.Entry(itemGHUpdate).State = EntityState.Modified;
      db.SaveChanges();
      return RedirectToAction("XemGioHang");
    }

    public ActionResult XoaSanPhamKhoiGioHang(int maSP, int maGioHang)
    {
      //Kiểm tra session giỏ hàng tồn tại chưa 
      if (Session["GioHang"] == null)
      {
        return RedirectToAction("Index", "Home");
      }
      //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
      SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == maSP);
      if (sp == null)
      {
        //TRang đường dẫn không hợp lệ
        Response.StatusCode = 404;
        return null;
      }
      GIOHANG lstGioHang = null;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
      }
      else
      {
        lstGioHang = LayGioHangKhachVangLai();
      }
      //Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
      CHITIETGIOHANG spCheck = lstGioHang.CHITIETGIOHANGs.SingleOrDefault(n => n.MaSP == maSP);
      //CHITIETGIOHANG spXoa = new CHITIETGIOHANG()
      //{
      //  MaGioHang=lstGioHang.MaGioHang,
      //  MaSP=maSP,
      //  SoLuong=spCheck.SoLuong,
      //  ThanhTienSP=spCheck.ThanhTienSP
      //};

      if (spCheck == null)
      {
        return RedirectToAction("Index", "Home");
      }
      //Xóa item trong giỏ hàng
      //
      //db.Entry(spCheck).State = EntityState.Deleted;
      db.CHITIETGIOHANGs.Attach(spCheck);
      db.CHITIETGIOHANGs.Remove(spCheck);
      //db.CHITIETGIOHANGs.Attach(spXoa);
      //db.CHITIETGIOHANGs.Remove(spXoa);
      db.SaveChanges();
      return RedirectToAction("XemGioHang");
    }
    //Xây dựng chức năng đặt hàng
    //public ActionResult DatHang(KhachHang kh)
    //{
    //    //Kiểm tra session giỏ hàng tồn tại chưa 
    //    if (Session["GioHang"] == null)
    //    {
    //        return RedirectToAction("Index", "Home");
    //    }
    //    KhachHang khang = new KhachHang();
    //    if (Session["TaiKhoan"] == null)
    //    {
    //        //Thêm khách hàng vào bảng khách hàng đối với khách hàng vãng lai (kh chưa có tài khoản)
    //        khang = kh;
    //        db.KhachHangs.Add(khang);
    //        db.SaveChanges();
    //    }
    //    else
    //    { 
    //        //Đối với khách hàng là thành viên
    //        ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
    //        khang.TenKH = tv.HoTen;
    //        khang.DiaChi = tv.DiaChi;
    //        khang.Email = tv.Email;
    //        khang.SoDienThoai = tv.SoDienThoai;
    //        khang.MaThanhVien = tv.MaLoaiTV;
    //        db.KhachHangs.Add(khang);
    //        db.SaveChanges();
    //    }

    //    //Thêm đơn hàng 
    //    DonDatHang ddh = new DonDatHang();
    //    ddh.MaKH = khang.MaKH;
    //    ddh.NgayDat = DateTime.Now;
    //    ddh.TinhTrangGiaoHang = false;
    //    ddh.DaThanhToan = false;
    //    ddh.UuDai = 0;
    //    ddh.DaHuy = false;
    //    ddh.DaXoa = false;
    //    db.DonDatHangs.Add(ddh);
    //    db.SaveChanges();
    //    //Thêm chi tiết đơn đặt hàng
    //    List<ItemGioHang> lstGH = LayGioHang();
    //    foreach (var item in lstGH)
    //    {
    //        ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
    //        ctdh.MaDDH = ddh.MaDDH;
    //        ctdh.MaSP = item.MaSP;
    //        ctdh.TenSP = item.TenSP;
    //        ctdh.SoLuong = item.SoLuong;
    //        ctdh.DonGia = item.DonGia;
    //        db.ChiTietDonDatHangs.Add(ctdh);
    //    }
    //    db.SaveChanges();
    //    Session["GioHang"] = null;
    //    return RedirectToAction("XemGioHang");
    //}
    //Thêm giỏ hàng Ajax
    //public ActionResult ThemGioHangAjax(int MaSP, string strURL)
    //{
    //  //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
    //  SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (sp == null)
    //  {
    //    //TRang đường dẫn không hợp lệ
    //    Response.StatusCode = 404;
    //    return null;
    //  }
    //  //Lấy giỏ hàng
    //  List<ItemGioHang> lstGioHang = LayGioHang();
    //  //Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng 
    //  ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (spCheck != null)
    //  {
    //    //Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
    //    if (sp.SoLuongTon < spCheck.SoLuong)
    //    {
    //      return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
    //    }
    //    spCheck.SoLuong++;
    //    spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
    //    ViewBag.TongSoLuong = TinhTongSoLuong();
    //    ViewBag.TongTien = TinhTongTien();
    //    return PartialView("GioHangPartial");
    //  }

    //  ItemGioHang itemGH = new ItemGioHang(MaSP);
    //  if (sp.SoLuongTon < itemGH.SoLuong)
    //  {
    //    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
    //  }

    //  lstGioHang.Add(itemGH);
    //  ViewBag.TongSoLuong = TinhTongSoLuong();
    //  ViewBag.TongTien = TinhTongTien();
    //  return PartialView("GioHangPartial");
    //}
  }
}