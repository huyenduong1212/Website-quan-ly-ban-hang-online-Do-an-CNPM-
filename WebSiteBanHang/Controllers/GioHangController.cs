using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.ViewModels;
using System.Data.Entity;
using System.Net;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
  public class GioHangController : Controller
  {

    QuanLyBanHangEntities db = new QuanLyBanHangEntities();

    //Lấy giỏ hàng
    public List<ItemGioHang> LayGioHangKhachVangLai()
    {
      //Giỏ hàng đã tồn tại 
      List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
      if (lstGioHang == null)
      {
        //Nếu session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
        lstGioHang = new List<ItemGioHang>();
        Session["GioHang"] = lstGioHang;//session nó đi theo cái lstGioHang luôn, thêm cái gì vô lstGioHang thì session có y chang
      }
      return lstGioHang;
    }
    //public GIOHANG LayGioHangKhachVangLai()
    //{
    //  GIOHANG lstGioHang = Session["GioHang"] as GIOHANG;
    //  if (lstGioHang == null)
    //  {
    //    lstGioHang = new GIOHANG();
    //    lstGioHang.DaDat = false;
    //    lstGioHang.ThanhTien = 0;
    //    //db.GIOHANGs.Add(lstGioHang);
    //    //db.SaveChanges();
    //    Session["GioHang"] = lstGioHang;
    //  }
    //  return lstGioHang;
    //}
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
    public int TinhTongSoLuong()
    {
      //Lấy giỏ hàng
      List<ItemGioHang> lstItemGioHang = null;
      GIOHANG lstGioHang = null;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG kh = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(kh.MaNguoiDung);
      }
      else
      {
        lstItemGioHang = Session["GioHang"] as List<ItemGioHang>;
        if (lstItemGioHang == null)
        {
          return 0;
        }
        return lstItemGioHang.Sum(n => n.SoLuong);
      }
      return (int)lstGioHang.CHITIETGIOHANGs.Sum(n => n.SoLuong);
    }
    //Tính Tổng tiền 
    public decimal TinhTongTien()
    {
      //Lấy giỏ hàng
      GIOHANG lstGioHang = null;
      List<ItemGioHang> lstItemGioHang = null;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG kh = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(kh.MaNguoiDung);
      }
      else
      {
        lstItemGioHang = Session["GioHang"] as List<ItemGioHang>;
        if (lstItemGioHang == null)
        {
          return 0;
        }
        return lstItemGioHang.Sum(n => n.ThanhTien);
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
      List<ItemGioHang> lstItemGioHang = null;
      SANPHAM sp = null;
      List<KhachHang_GioHangViewModel> lstSP_KH = new List<KhachHang_GioHangViewModel>();
      int maGioHang;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
        ViewBag.maGioHang = lstGioHang.MaGioHang;
        ViewBag.maKH = khachHang.MaNguoiDung;
        maGioHang = lstGioHang.MaGioHang;
        foreach (CHITIETGIOHANG ctgh in lstGioHang.CHITIETGIOHANGs)
        {
          sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == ctgh.MaSP);
          KhachHang_GioHangViewModel sp_KhachHang = new KhachHang_GioHangViewModel()
          {
            MaSP = ctgh.MaSP,
            TenSP = sp.TenSP,
            DonGia = sp.DonGia.Value,
            HinhAnh = sp.HinhAnh,
            SoLuong = ctgh.SoLuong.Value
          };
          lstSP_KH.Add(sp_KhachHang);
        }
      }
      else
      {
        lstItemGioHang = LayGioHangKhachVangLai();
        ViewBag.maGioHang = null;
        ViewBag.maKH = null;
        foreach (ItemGioHang itemGioHang in lstItemGioHang)
        {
          KhachHang_GioHangViewModel sp_KhachHang = new KhachHang_GioHangViewModel()
          {
            MaSP = itemGioHang.MaSP,
            TenSP = itemGioHang.TenSP,
            DonGia = itemGioHang.DonGia,
            HinhAnh = itemGioHang.HinhAnh,
            SoLuong = itemGioHang.SoLuong

          };
          lstSP_KH.Add(sp_KhachHang);
        }
      }
      ViewBag.TongSoLuong = TinhTongSoLuong();
      ViewBag.TongTien = TinhTongTien();
      return View(lstSP_KH);
    }
    //Thêm giỏ hàng Ajax
    public ActionResult ThemGioHangAjax(int MaSP, string strURL)
    {
      List<ItemGioHang> lstItemGioHang = null;
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
          spCheck.ThanhTienSP = (decimal)spCheck.SoLuong * donGiaSanPham;
          ViewBag.TongSoLuong = TinhTongSoLuong();
          ViewBag.TongTien = TinhTongTien();
          TempData["result"] = "Thêm vào giỏ hàng thành công";
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
          TempData["result"] = "Sản phẩm đã hết hàng";
          return PartialView("GioHangPartial");
          //return Content("<script>alert(\"Sản phẩm đã hết hàng!\")</script>");
        }
        //lstGioHang.Add(itemGH);
        ViewBag.TongSoLuong = TinhTongSoLuong();
        ViewBag.TongTien = TinhTongTien();
      }
      else
      {
        lstItemGioHang = LayGioHangKhachVangLai();
        ItemGioHang spCheck = lstItemGioHang.SingleOrDefault(n => n.MaSP == MaSP);
        if (spCheck != null)
        {
          //Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
          if (sp.SoLuongTon < spCheck.SoLuong)
          {
            ViewBag.ThongBao = 0;
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            //return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
            TempData["result"] = "Sản phẩm đã hết hàng";
            return PartialView("GioHangPartial");
          }
          spCheck.SoLuong++;
          spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
          ViewBag.TongSoLuong = TinhTongSoLuong();
          ViewBag.TongTien = TinhTongTien();
          TempData["result"] = "Thêm vào giỏ hàng thành công";
          return PartialView("GioHangPartial");
        }
        ItemGioHang itemGH = new ItemGioHang(MaSP);
        if (sp.SoLuongTon < itemGH.SoLuong)
        {
          ViewBag.TongSoLuong = TinhTongSoLuong();
          ViewBag.TongTien = TinhTongTien();
          ViewBag.ThongBao = 0;
          TempData["result"] = "Thêm vào giỏ hàng thành công";
          return PartialView("GioHangPartial");
        }
        lstItemGioHang.Add(itemGH);
        ViewBag.TongSoLuong = TinhTongSoLuong();
        ViewBag.TongTien = TinhTongTien();
      }
      TempData["result"] = "Thêm vào giỏ hàng thành công";
      return PartialView("GioHangPartial");
    }

    [HttpPost]
    public ActionResult CapNhatGioHang(int maGioHang, int maSP, int soLuongMoi)
    {
      List<ItemGioHang> lstItemGioHang = null;
      //Kiểm tra số lượng tồn 
      SANPHAM spCheck = db.SANPHAMs.Single(n => n.MaSP == maSP);
      if (spCheck.SoLuongTon < soLuongMoi)
      {
        return View("ThongBao");
      }
      //Cập nhật số lượng trong session giỏ hàng 
      //Bước 1: Lấy List<GioHang> từ session["GioHang"]
      GIOHANG lstGioHang = null;
      SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == maSP);
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
        CHITIETGIOHANG itemGHUpdate = lstGioHang.CHITIETGIOHANGs.SingleOrDefault(n => n.MaSP == maSP);
        if (itemGHUpdate == null)
        {
          return RedirectToAction("Index", "Home");
        }
        //Bước 3: Tiến hành cập nhật lại số lượng cũng thành tiền
        itemGHUpdate.SoLuong = soLuongMoi;
        itemGHUpdate.ThanhTienSP = itemGHUpdate.SoLuong * sp.DonGia;
        db.Entry(itemGHUpdate).State = EntityState.Modified;
        db.SaveChanges();
        TempData["result"] = "Cập nhật thành công";
      }
      else
      {
        lstItemGioHang = LayGioHangKhachVangLai();
        ItemGioHang itemGioHang = lstItemGioHang.Find(n => n.MaSP == maSP);
        itemGioHang.SoLuong = soLuongMoi;
        itemGioHang.ThanhTien = (decimal)(itemGioHang.SoLuong * sp.DonGia);
        //ItemGioHang itemGioHang = new ItemGioHang(maSP, soLuongMoi);
      }
      //Bước 2: Lấy sản phẩm cần cập nhật từ trong list<GioHang> ra

      return RedirectToAction("XemGioHang");
    }
    public ActionResult XemChiTiet(int? id)
    {
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
      return View(sp);
    }
    public ActionResult XoaSanPhamKhoiGioHang(int maSP, int maGioHang)
    {
      List<ItemGioHang> lstItemGioHang = null;
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
        CHITIETGIOHANG spCheck = lstGioHang.CHITIETGIOHANGs.SingleOrDefault(n => n.MaSP == maSP);
        if (spCheck == null)
        {
          return RedirectToAction("Index", "Home");
        }
        db.CHITIETGIOHANGs.Attach(spCheck);
        db.CHITIETGIOHANGs.Remove(spCheck);
        db.SaveChanges();
        //return Content("<script> alert(\"Xóa thành công!\")</script>");
        return RedirectToAction("XemGioHang");
      }
      else
      {
        lstItemGioHang = LayGioHangKhachVangLai();
        ItemGioHang itemGioHang = lstItemGioHang.Find(n => n.MaSP == maSP);
        lstItemGioHang.Remove(itemGioHang);
      }
      ViewBag.TongSoLuong = TinhTongSoLuong();
      ViewBag.TongTien = TinhTongTien();
      return RedirectToAction("XemGioHang");
    }
    [HttpGet]
    public ActionResult DatHang(int? maKH, int? maGioHang)
    {
      List<KhachHang_GioHangViewModel> lstKHGH = new List<KhachHang_GioHangViewModel>();
      ViewBag.TongTien = TinhTongTien();
      ViewBag.maKH = maKH;
      if (maGioHang != null && maKH!=null)
      {
        foreach (CHITIETGIOHANG ctgh1 in db.CHITIETGIOHANGs.Where(n => n.MaGioHang == maGioHang))
        {
          SANPHAM sp = db.SANPHAMs.Where(n => n.MaSP == ctgh1.MaSP).SingleOrDefault();
          KhachHang_GioHangViewModel thongTinSanPham = new KhachHang_GioHangViewModel
          {
            TenSP = sp.TenSP,
            MaSP = ctgh1.MaSP,
            DonGia = (decimal)sp.DonGia,
            HinhAnh = sp.HinhAnh,
            MaGioHang = (int)maGioHang,
            SoLuong = (int)ctgh1.SoLuong,
          };
          lstKHGH.Add(thongTinSanPham);
        }
      }
      else
      {
        List<ItemGioHang> lstItemGioHang = LayGioHangKhachVangLai();
        foreach(ItemGioHang itemGioHang in lstItemGioHang)
        {
          KhachHang_GioHangViewModel thongTinSanPham = new KhachHang_GioHangViewModel
          {
            TenSP = itemGioHang.TenSP,
            MaSP = itemGioHang.MaSP,
            DonGia = itemGioHang.DonGia,
            HinhAnh = itemGioHang.HinhAnh,
            SoLuong = itemGioHang.SoLuong,
          };
          lstKHGH.Add(thongTinSanPham);
        }
      }
      return View(lstKHGH);
    }
    [HttpPost]
    public ActionResult DatHang1(int? maKH, int? maGioHang, string diaChiNhanHang, string soDienThoaiNhanHang, string ho, string tenLot, string ten, string email)
    {
      GIOHANG lstGioHang = null;
      if (Session["NGUOIDUNG"] != null)
      {
        NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
        lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
        DONDATHANG ddh = new DONDATHANG()
        {
          ThoiDiemDat = DateTime.Now,
          NgayGiaoDuKien = null,
          UuDai = 0,
          TinhTrangGiaoHang = -1,
          MaGioHang = maGioHang,
          MaKH = maKH,
          TongTien = TinhTongTien(),
          DiaChiNhanHang = diaChiNhanHang,
          SoDienThoaiNhanHang = soDienThoaiNhanHang
        };
        db.DONDATHANGs.Add(ddh);
        db.SaveChanges();
        TempData["result"] = "Đặt hàng thành công";
      }
      else
      {
        NGUOIDUNG khachHangKhongCoTaiKhoan = new NGUOIDUNG()
        {
          MaLoaiNguoiDung = 2,
          Ho = ho,
          TenLot = tenLot,
          Ten = ten,
          DiaChi = diaChiNhanHang,
          SoDienThoai = soDienThoaiNhanHang,
          Email = email
        };
        db.NGUOIDUNGs.Add(khachHangKhongCoTaiKhoan);
        db.SaveChanges();
        GIOHANG gioHangCuaKhachVangLai = new GIOHANG()
        {
          MaKH = khachHangKhongCoTaiKhoan.MaNguoiDung,
          ThanhTien = TinhTongTien(),
          DaDat = true,
        };
        db.GIOHANGs.Add(gioHangCuaKhachVangLai);
        db.SaveChanges();
        DONDATHANG ddh = new DONDATHANG()
        {
          ThoiDiemDat = DateTime.Now,
          NgayGiaoDuKien = null,
          UuDai = 0,
          TinhTrangGiaoHang = -1,
          MaGioHang = gioHangCuaKhachVangLai.MaGioHang,
          MaKH = khachHangKhongCoTaiKhoan.MaNguoiDung,
          TongTien = TinhTongTien(),
          DiaChiNhanHang = diaChiNhanHang,
          SoDienThoaiNhanHang = soDienThoaiNhanHang
        };
        db.DONDATHANGs.Add(ddh);
        db.SaveChanges();
        Session["GioHang"] = null;
        TempData["result"] = "Đặt hàng thành công";
      }
      return RedirectToAction("XemGioHang");
    }
    //Xây dựng chức năng đặt hàng
    //[HttpPost]
    //public ActionResult DatHang(int? maKH, int? maGioHang, string TenLot, string Ho, string Ten, string DiaChi, string Email, string SoDienThoai)
    //{
    //  List<ItemGioHang> lstItemGioHang = null;
    //  GIOHANG lstGioHang = null;
    //  if (Session["NGUOIDUNG"] != null)
    //  {
    //    //hiện ra chọn địa chỉ giao hàng
    //    NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
    //    lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
    //    DONDATHANG ddh = new DONDATHANG()
    //    {
    //      ThoiDiemDat = DateTime.Now,
    //      NgayGiaoDuKien = DateTime.Now.AddDays(3),
    //      UuDai = 0,
    //      TinhTrangGiaoHang = 0,
    //      MaGioHang = maGioHang,
    //      MaKH = maKH,
    //      TongTien = TinhTongTien(),
    //    };
    //    db.DONDATHANGs.Add(ddh);
    //    db.SaveChanges();
    //    //return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
    //  }
    //  else
    //  {
    //    //Thêm vào tài khoản loại người dùng số 2
    //    NGUOIDUNG khachHangKhongCoTaiKhoan = new NGUOIDUNG()
    //    {
    //      MaLoaiNguoiDung = 2,
    //      Ho = Ho,
    //      TenLot = TenLot,
    //      Ten = Ten,
    //      DiaChi = DiaChi,
    //      SoDienThoai = SoDienThoai,
    //      Email = Email
    //    };
    //    db.NGUOIDUNGs.Add(khachHangKhongCoTaiKhoan);
    //    db.SaveChanges();
    //    // lstItemGioHang = LayGioHangKhachVangLai();
    //    GIOHANG gioHangCuaKhachVangLai = new GIOHANG()
    //    {
    //      MaKH = khachHangKhongCoTaiKhoan.MaNguoiDung,
    //      ThanhTien = TinhTongTien(),
    //      DaDat = true,
    //    };
    //    db.GIOHANGs.Add(gioHangCuaKhachVangLai);
    //    db.SaveChanges();
    //    DONDATHANG ddh = new DONDATHANG()
    //    {
    //      ThoiDiemDat = DateTime.Now,
    //      NgayGiaoDuKien = DateTime.Now.AddDays(3),
    //      UuDai = 0,
    //      TinhTrangGiaoHang = 0,
    //      MaGioHang = gioHangCuaKhachVangLai.MaGioHang,
    //      MaKH = khachHangKhongCoTaiKhoan.MaNguoiDung,
    //      TongTien = TinhTongTien(),
    //    };
    //    db.DONDATHANGs.Add(ddh);
    //    db.SaveChanges();
    //    Session["GioHang"] = null;
    //  }
    //  return View("XemGioHang");
    //}
  }
}
