using WebSiteBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSiteBanHang.Controllers
{
  [Authorize(Roles ="31")]
  public class PhanQuyenController : Controller
  {
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    public ActionResult Index()
    {
      return View(db.LOAINGUOIDUNGs.OrderBy(n => n.TenLoaiNguoiDung));
    }
    [HttpGet]
    public ActionResult PhanQuyen(int? id)
    {
      //Lấy mã loại tv dựa vào id
      if (id == null)
      {
        Response.StatusCode = 404;
        return null;
      }
      LOAINGUOIDUNG ltv = db.LOAINGUOIDUNGs.SingleOrDefault(n => n.MaLoaiNguoiDung == id);
      if (ltv == null)
      {
        return HttpNotFound();
      }
      //Lấy ra danh sách quyền để load ra check box
      ViewBag.MaChucNang = db.CHUCNANGs;
      //Lấy ra danh sách quyền của loại thành viên đó
      //Bước 1: Lấy ra những quyền thuộc loại thành viên đó dựa vào bảng LoaiThanhVien_Quyen
      //ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);
      ViewBag.LoaiNguoiDung_ChucNang = db.QUYENHANLOAINGUOIDUNGs.Where(n => n.MaLoaiNguoiDung == id);
      return View(ltv);
    }
    [HttpPost]
    public ActionResult PhanQuyen(int? MaLTV, IEnumerable<QUYENHANLOAINGUOIDUNG> lstChucNangDuocChon)
    {
      var lstQuyenCuaLoaiNguoiDung = db.QUYENHANLOAINGUOIDUNGs.Where(n => n.MaLoaiNguoiDung == MaLTV);
      if (lstQuyenCuaLoaiNguoiDung.Count() != 0)
      {
        db.QUYENHANLOAINGUOIDUNGs.RemoveRange(lstQuyenCuaLoaiNguoiDung);
        db.SaveChanges();
      }
      if (lstChucNangDuocChon != null)
      {
        foreach (var item in lstChucNangDuocChon)
        {
          item.MaLoaiNguoiDung = int.Parse(MaLTV.ToString());
          //Nếu được check thì insert dữ liệu vào bảng phân quyền
          db.QUYENHANLOAINGUOIDUNGs.Add(item);
        }
        db.SaveChanges();
      }
      return RedirectToAction("Index");
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