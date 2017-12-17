using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using System.Net.Mail; // send mail

namespace WebSiteBanHang.Controllers
{
  [Authorize(Roles = "10")]
  public class QuanLyDonHangController : Controller
  {
    //
    // GET: /QuanLyDonHang/
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    
    public ActionResult ChoDuyet()
    {
      var lst = db.DONDATHANGs.Where(n => n.TinhTrangGiaoHang == -1).OrderBy(p => p.ThoiDiemDat);
      return View(lst);
    }
    [HttpGet]
    public ActionResult DuyetDonHang(int? id)
    {
      //Kiểm tra xem id hợp lệ không
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      DONDATHANG model = db.DONDATHANGs.SingleOrDefault(n => n.MaDDH == id);
      NGUOIDUNG khachHang = db.NGUOIDUNGs.Single(n => n.MaNguoiDung == model.MaKH);
      //Kiểm tra đơn hàng có tồn tại không
      if (model == null)
      {
        return HttpNotFound();
      }
      //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
      var lstChiTietDH = db.CHITIETDONDATHANGs.Where(n => n.MaDDH == id);
      ViewBag.ListChiTietDH = lstChiTietDH;
      ViewBag.TenKH = khachHang.Ho+" "+khachHang.TenLot+ " " + khachHang.Ten;
      return View(model);
    }
    public ActionResult DaDuyet()
    {
      var lst = db.DONDATHANGs.Where(n => n.TinhTrangGiaoHang == 0).OrderBy(p => p.ThoiDiemLap);
      return View(lst);
    }
    public ActionResult DaTuChoi()
    {
      var lst = db.DONDATHANGs.Where(n => n.TinhTrangGiaoHang == 1).OrderBy(p => p.ThoiDiemDat);
      return View(lst);
    }
    public ActionResult DaGiao()
    {
      var lst = db.DONDATHANGs.Where(n => n.TinhTrangGiaoHang == 2).OrderBy(p => p.ThoiDiemDat);
      return View(lst);
    }
    public ActionResult ChuaThanhToan()
    {
      //Lấy danh sách các đơn hàng Chưa duyệt
      var lst = db.DONDATHANGs.OrderBy(n => n.ThoiDiemDat);
      return View(lst);
    }
    public ActionResult ChuaGiao()
    {
      //Lấy danh sách đơn hàng chưa giao 
      var lstDSDHCG = db.DONDATHANGs.Where(n => n.TinhTrangGiaoHang == 0).OrderBy(n => n.NgayGiaoDuKien);
      return View(lstDSDHCG);
    }
    public ActionResult DaGiaoDaThanhToan()
    {
      //Lấy danh sách đơn hàng chưa giao 
      var lstDSDHCG = db.DONDATHANGs.Where(n => n.TinhTrangGiaoHang == 1);
      return View(lstDSDHCG);
    }
    //[HttpGet]
    //public ActionResult DuyetDonHang(int? id)
    //{
    //  //Kiểm tra xem id hợp lệ không
    //  if (id == null)
    //  {
    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //  }
    //  DONDATHANG model = db.DONDATHANGs.SingleOrDefault(n => n.MaDDH == id);
    //  //Kiểm tra đơn hàng có tồn tại không
    //  if (model == null)
    //  {
    //    return HttpNotFound();
    //  }
    //  //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
    //  var lstChiTietDH = db.CHITIETDONDATHANGs.Where(n => n.MaDDH == id);
    //  ViewBag.ListChiTietDH = lstChiTietDH;
    //  return View(model);
    //}
    [HttpPost]
    public ActionResult DuyetDonHang(DONDATHANG ddh)
    {
      //Truy vấn lấy ra dữ liệu của đơn hàn đó 
      DONDATHANG ddhUpdate = db.DONDATHANGs.Single(n => n.MaDDH == ddh.MaDDH);
      //ddhUpdate.DaThanhToan = ddh.DaThanhToan;
      ddhUpdate.TinhTrangGiaoHang = 0;
      ddhUpdate.MaNV = (Session["NGUOIDUNG"] as NGUOIDUNG).MaNguoiDung;
      ddhUpdate.ThoiDiemLap = DateTime.Now;
      ddhUpdate.NgayGiaoDuKien = ddh.NgayGiaoDuKien;
      db.SaveChanges();
      //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
      var lstChiTietDH = db.CHITIETDONDATHANGs.Where(n => n.MaDDH == ddh.MaDDH);
      //Cập nhật số lượng tồn của từng sản phẩm
      foreach(var item in lstChiTietDH)
      {
        SANPHAM spUpdate = db.SANPHAMs.Single(n => n.MaSP == item.MaSP);
        spUpdate.SoLuongTon -= item.SoLuong;
      }
      ViewBag.ListChiTietDH = lstChiTietDH;
      //Gửi khách hàng 1 mail để xác nhận việc thanh toán 
      GuiEmail("Xác nhận đơn hàng từ xuancongduy.com", "hntxuanit15spkt@gmail.com", "nguyennhi.cave@gmail.com", "15110376", "Đơn hàng của bạn đã được đặt thành công!");
      return View(ddhUpdate);
    }
    public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
    {
      // goi email
      MailMessage mail = new MailMessage();
      mail.To.Add(ToEmail); // Địa chỉ nhận
      mail.From = new MailAddress(ToEmail); // Địa chửi gửi
      mail.Subject = Title;  // tiêu đề gửi
      mail.Body = Content;                 // Nội dung
      mail.IsBodyHtml = true;
      SmtpClient smtp = new SmtpClient();
      smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
      smtp.Port = 587;               //port của Gmail
      smtp.UseDefaultCredentials = false;
      smtp.Credentials = new System.Net.NetworkCredential
      (FromEmail, PassWord);//Tài khoản password người gửi
      smtp.EnableSsl = true;   //kích hoạt giao tiếp an toàn SSL
      smtp.Send(mail);   //Gửi mail đi
    }


    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (db != null)
          db.Dispose();
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}