using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;


namespace WebSiteBanHang.Controllers
{
  public class KhachHangController : Controller
  {
    //
    // GET: /KhachHang/
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SuaThongTin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SuaThongTin(NGUOIDUNG nd)
        {
            var entry = db.Entry(nd);
            entry.State = EntityState.Modified;
            entry.Property(e => e.MaLoaiNguoiDung).IsModified = false;
            entry.Property(e => e.TaiKhoan).IsModified = false;
            entry.Property(e => e.MatKhau).IsModified = false;
            db.SaveChanges();
            
            

            return RedirectToAction("Index");
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(NGUOIDUNG nd)
        {
            var entry = db.Entry(nd);
            entry.State = EntityState.Modified;
            entry.Property(e => e.Ho).IsModified = false;
            entry.Property(e => e.TenLot).IsModified = false;
            entry.Property(e => e.Ten).IsModified = false;
            entry.Property(e => e.GioiTinh).IsModified = false;
            entry.Property(e => e.DiaChi).IsModified = false;
            entry.Property(e => e.SoDienThoai).IsModified = false;
            entry.Property(e => e.Email).IsModified = false;
            entry.Property(e => e.MaLoaiNguoiDung).IsModified = false;
            entry.Property(e => e.TaiKhoan).IsModified = false;
            entry.Property(e => e.TrangThai).IsModified = false;

            db.SaveChanges();
            Session["NGUOIDUNG"] = null;
            return RedirectToAction("Index", "Home");
        }
        //public ActionResult Index()
        //{
        //    //Truy vấn dữ liệu thông qua câu lệnh
        //    //Đối lstKH sẽ lấy toàn bộ dữ liệu từ bản khách hàng
        //    //Cách 1: Lấy dữ liệu là 1 danh sách khách hàng
        //    //var lstKH = from KH in db.KhachHangs select KH;
        //    //Cách 2: Dùng phương thức hổ trợ sẵn
        //    var lstKH = db.KhachHangs;
        //    return View(lstKH);

        //}
        //public ActionResult Index1()
        //{
        //  var lstKH = from KH in db.KhachHangs select KH;
        //  return View(lstKH);
        //}
        //public ActionResult TruyVan1DoiTuong()
        //{
        //  //Cách 1: Truy vấn 1 đối tượng bằng câu lệnh truy vấn
        //  //Bước1: Lấy ra danh sách khách hàng 
        //  var lstKH = from kh in db.KhachHangs where kh.MaKH == 2 select kh;
        //  //Buoc2: 
        //  //KhachHang khang = lstKH.FirstOrDefault();
        //  //Lấy đối tượng khách hàng dựa trên phương thức hổ trợ
        //  KhachHang khang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);
        //  return View(khang);
        //}
        //public ActionResult SortDuLieu()
        //{
        //  //Phương thức sắp xếp dữ liệu
        //  List<KhachHang> lstKH = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
        //  return View(lstKH);

        //}
        //public ActionResult GroupDuLieu()
        //{
        //  //Group dữ liệu
        //  List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
        //  return View(lstKH);
        //}

    }
}