using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using WebSiteBanHang.ViewModels;

namespace WebSiteBanHang.Controllers
{
  public class GioHangController : Controller
  {

    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    //Lấy giỏ hàng
    public GIOHANG LayGioHang()
    {
      GIOHANG lstGioHang = Session["GioHang"] as GIOHANG;
      if (lstGioHang == null)
      {
        lstGioHang = db.GIOHANGs.Where(x => x.DaDat == false) as GIOHANG;
        lstGioHang = new GIOHANG();
        lstGioHang.DaDat = false;
        lstGioHang.ThanhTien = 0;
        db.GIOHANGs.Add(lstGioHang);
        db.SaveChanges();
        Session["GioHang"] = lstGioHang;
      }
      return lstGioHang;
    }
    //public List<ItemGioHang> LayGioHang()
    //{
    //  //Giỏ hàng đã tồn tại 
    //  List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
    //  if (lstGioHang == null)
    //  {
    //    //Nếu session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
    //    lstGioHang = new List<ItemGioHang>();
    //    Session["GioHang"] = lstGioHang;
    //  }
    //  return lstGioHang;
    //}
    //Thêm giỏ hàng thông thường (Load lại trang)
    //public ActionResult ThemGioHang(int MaSP, string strURL)
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
    //  GIOHANG lstGioHang = LayGioHang();
    //  //Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng 
    //  //CHITIETGIOHANG spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (spCheck != null)
    //  {
    //    //Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
    //    if (sp.SoLuongTon < spCheck.SoLuong)
    //    {
    //      return View("ThongBao");
    //    }
    //    spCheck.SoLuong++;
    //    spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
    //    return Redirect(strURL);
    //  }

    //  ItemGioHang itemGH = new ItemGioHang(MaSP);
    //  if (sp.SoLuongTon < itemGH.SoLuong)
    //  {
    //    return View("ThongBao");
    //  }

    //  lstGioHang.Add(itemGH);
    //  return Redirect(strURL);
    //}
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
      GIOHANG lstGioHang = LayGioHang();
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
      //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
      SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
      if (sp == null)
      {
        //Trang đường dẫn không hợp lệ
        Response.StatusCode = 404;
        return null;
      }
      //Lấy giỏ hàng
      GIOHANG lstGioHang = LayGioHang();
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
    //Chỉnh sửa giỏ hàng
    //[HttpGet]
    //public ActionResult SuaGioHang(int MaSP)
    //{
    //  //Kiểm tra session giỏ hàng tồn tại chưa 
    //  if (Session["GioHang"] == null)
    //  {
    //    return RedirectToAction("Index", "Home");
    //  }
    //  //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
    //  SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (sp == null)
    //  {
    //    //TRang đường dẫn không hợp lệ
    //    Response.StatusCode = 404;
    //    return null;
    //  }
    //  //Lấy list giỏ hàng từ session
    //  GIOHANG lstGioHang = LayGioHang();
    //  //Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
    //  ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (spCheck == null)
    //  {
    //    return RedirectToAction("Index", "Home");
    //  }
    //  //Lấy list giỏ hàng tạo giao diện
    //  ViewBag.GioHang = lstGioHang;

    //  //Nếu tồn tại rồi
    //  return View(spCheck);
    //}
    //Xử lý cập nhật giỏ hàng
    //[HttpPost]
    //public ActionResult CapNhatGioHang(ItemGioHang itemGH)
    //{
    //  //Kiểm tra số lượng tồn 
    //  SANPHAM spCheck = db.SANPHAMs.Single(n => n.MaSP == itemGH.MaSP);
    //  if (spCheck.SoLuongTon < itemGH.SoLuong)
    //  {
    //    return View("ThongBao");
    //  }
    //  //Cập nhật số lượng trong session giỏ hàng 
    //  //Bước 1: Lấy List<GioHang> từ session["GioHang"]
    //  List<ItemGioHang> lstGH = LayGioHang();
    //  //Bước 2: Lấy sản phẩm cần cập nhật từ trong list<GioHang> ra
    //  ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
    //  //Bước 3: Tiến hành cập nhật lại số lượng cũng thành tiền
    //  itemGHUpdate.SoLuong = itemGH.SoLuong;
    //  itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
    //  return RedirectToAction("XemGioHang");
    //}

    //public ActionResult XoaGioHang(int MaSP)
    //{
    //  //Kiểm tra session giỏ hàng tồn tại chưa 
    //  if (Session["GioHang"] == null)
    //  {
    //    return RedirectToAction("Index", "Home");
    //  }
    //  //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
    //  SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (sp == null)
    //  {
    //    //TRang đường dẫn không hợp lệ
    //    Response.StatusCode = 404;
    //    return null;
    //  }
    //  //Lấy list giỏ hàng từ session
    //  List<ItemGioHang> lstGioHang = LayGioHang();
    //  //Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
    //  ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
    //  if (spCheck == null)
    //  {
    //    return RedirectToAction("Index", "Home");
    //  }
    //  //Xóa item trong giỏ hàng
    //  lstGioHang.Remove(spCheck);
    //  return RedirectToAction("XemGioHang");
    //}
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